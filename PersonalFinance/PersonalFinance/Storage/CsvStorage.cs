using System.Globalization;
using PersonalFinance.Accounts;
using PersonalFinance.Currencies;

namespace PersonalFinance.Storage
{
    public class CsvStorage : IDataStorage
    {
        public void SaveAccounts(List<AccountBase> accounts, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine("AccountType,Name,CurrentBalance,CurrencyCode");

                    foreach (var account in accounts)
                    {
                        string accountType;
                        if (account is CreditCard)
                        {
                            accountType = "CreditCard";
                        }
                        else
                        {
                            accountType = "DebitCard";
                        }

                        string line = $"{accountType},{account.Name},{account.CurrentBalance.ToString(CultureInfo.InvariantCulture)},{account.AccountCurrency.Code}";
                        writer.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка збереження CSV: {ex.Message}", ex);
            }
        }

        public List<AccountBase> LoadAccounts(string filePath)
        {
            var accounts = new List<AccountBase>();

            if (!File.Exists(filePath))
            {
                return accounts;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string header = reader.ReadLine();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        string[] parts = line.Split(',');
                        if (parts.Length >= 4)
                        {
                            string type = parts[0];
                            string name = parts[1];
                            decimal balance = decimal.Parse(parts[2], CultureInfo.InvariantCulture);

                            CurrencyType currency;
                            switch (parts[3])
                            {
                                case "USD":
                                    currency = CurrencyType.USD; 
                                    break;
                                case "EUR":
                                    currency = CurrencyType.EUR;
                                    break;
                                case "UAH":
                                    currency = CurrencyType.UAH;
                                    break;
                                default:
                                    currency = new CurrencyType(parts[3], parts[3]);
                                    break;
                            }

                            if (type == "CreditCard")
                            {
                                accounts.Add(new CreditCard(name, balance, currency, 5000, 0.05m));
                            }
                            else
                            {
                                accounts.Add(new DebitCard(name, balance, currency));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка завантаження CSV: {ex.Message}", ex);
            }

            return accounts;
        }
    }
}