using System.Globalization;
using PersonalFinance.Accounts;
using PersonalFinance.Currencies;

namespace PersonalFinance.Storage
{
    public class CsvStorage : IDataStorage
    {
        public async Task SaveAccountsAsync(List<AccountBase> accounts, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    await writer.WriteLineAsync("AccountType,Name,CurrentBalance,CurrencyCode");

                    foreach (AccountBase account in accounts)
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
                        await writer.WriteLineAsync(line);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка збереження CSV: {ex.Message}", ex);
            }
        }

        public async Task<List<AccountBase>> LoadAccountsAsync(string filePath)
        {
            List<AccountBase> accounts = new List<AccountBase>();

            if (!File.Exists(filePath))
            {
                return accounts;
            }

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string header = await reader.ReadLineAsync();

                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        ProcessCsvLine(line, accounts);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка завантаження CSV: {ex.Message}", ex);
            }

            return accounts;
        }

        private void ProcessCsvLine(string line, List<AccountBase> accounts)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return;
            }

            string[] parts = line.Split(',');
            if (parts.Length < 4)
            {
                return;
            }

            string type = parts[0];
            string name = parts[1];
            decimal balance = decimal.Parse(parts[2], CultureInfo.InvariantCulture);
            
            CurrencyType currency = DetermineCurrency(parts[3]);

            if (type == "CreditCard")
            {
                accounts.Add(new CreditCard(name, balance, currency, 5000, 0.05m));
            }
            else
            {
                accounts.Add(new DebitCard(name, balance, currency));
            }
        }

        private CurrencyType DetermineCurrency(string currencyCode)
        {
            switch (currencyCode)
            {
                case "USD": 
                    return CurrencyType.USD;
                case "EUR":
                    return CurrencyType.EUR;
                case "UAH": 
                    return CurrencyType.UAH;
                default: 
                    return new CurrencyType(currencyCode, currencyCode);
            }
        }
    }
}