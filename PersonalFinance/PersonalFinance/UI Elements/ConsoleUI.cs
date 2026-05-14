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
            TransactionsListOutput(account.Transactions, account.AccountCurrency);
        }

        public static void TransactionsListOutput(IReadOnlyList<Transaction> transactions, CurrencyType accountCurrency)
        {
            if (transactions.Count == 0)
            {
                Console.WriteLine("Транзакцій немає.");
                return;
            }

            for (int i = 0; i < transactions.Count; i++)
            {
                Transaction tr = transactions[i];
                Console.Write($"{i + 1}. {tr.Date.ToShortDateString()} - {tr.Name} [{tr.TransactionCategory.Name}]: ");
                DrawColoredMoneyAmount(tr, accountCurrency);
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

        public static int ShowInteractiveMenu(string title, string[] options)
        {
            int selectedIndex = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"\n=== {title} ===\n");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"> {options[i]} ");
                    }
                    else
                    {
                        Console.WriteLine($"  {options[i]} ");
                    }
                    Console.ResetColor();
                }

                ConsoleKey key = Console.ReadKey(true).Key;

                if (key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selectedIndex--;
                    if (selectedIndex < 0) selectedIndex = options.Length - 1;
                }
                else if (key == ConsoleKey.DownArrow)
                {
                    selectedIndex++;
                    if (selectedIndex >= options.Length) selectedIndex = 0;
                }
            }

            Console.Clear();
            return selectedIndex;
        }
    }
}
