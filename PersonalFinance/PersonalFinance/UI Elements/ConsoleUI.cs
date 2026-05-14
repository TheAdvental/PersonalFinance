using PersonalFinance.Accounts;
using PersonalFinance.Transactions;
using PersonalFinance.Currencies;

namespace PersonalFinance.UI_Elements
{
    public class ConsoleUI
    {
        public static void TransactionsOutput(AccountBase account)
        {
            Console.WriteLine($"Історія транзакцій з рахунку {account.Name}:");

            foreach (Transaction tr in account.Transactions)
            {
                Console.Write($"{tr.Date.ToShortDateString()} - {tr.Name}: ");

                DrawColoredMoneyAmount(tr, account.AccountCurrency);

                Console.WriteLine();
            }
        }

        public static void DrawColoredMoneyAmount(Transaction tr, CurrencyType currency)
        {
            if (tr.TransactionType == TransactionType.Income)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"+{tr.MoneyAmount}{currency.Symbol}");
            }
            else if (tr.TransactionType == TransactionType.Expense)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"-{tr.MoneyAmount}{currency.Symbol}");
            }

            Console.ResetColor();
        }

        public static void ShowSuccessMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static void ShowErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}
