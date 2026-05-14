using PersonalFinance.Accounts;
using PersonalFinance.Transactions;
using PersonalFinance.UI_Elements;

namespace PersonalFinance;

public partial class FinanceApp
{
    private void FilterTransactions()
    {
        if (currentAccount.Transactions.Count == 0)
        {
            ConsoleUI.ShowErrorMessage("Немає транзакцій для фільтрації.");
            return;
        }

        List<Transaction> tempDocs = new List<Transaction>(currentAccount.Transactions);

        string[] filterOptions = { "Показати тільки доходи", "Показати тільки витрати", "Відсортувати за сумою (Спадання)", "Показати все" };
        int fChoice = ConsoleUI.ShowInteractiveMenu("ФІЛЬТРАЦІЯ", filterOptions);

        List<Transaction> result = ApplyFilter(tempDocs, fChoice);

        Console.WriteLine("\n--- Результат ---");
        ConsoleUI.TransactionsListOutput(result, currentAccount.AccountCurrency);
    }

    private List<Transaction> ApplyFilter(List<Transaction> tempDocs, int fChoice)
    {
        if (fChoice == 0) 
        {
            return FilterByType(tempDocs, TransactionType.Income);
        }
        if (fChoice == 1) 
        {
            return FilterByType(tempDocs, TransactionType.Expense);
        }
        return ProcessOtherFilters(tempDocs, fChoice);
    }

    private List<Transaction> ProcessOtherFilters(List<Transaction> tempDocs, int fChoice)
    {
        if (fChoice == 2) 
        {
            return SortByAmountDesc(tempDocs);
        }
        return new List<Transaction>(tempDocs);
    }

    private List<Transaction> FilterByType(List<Transaction> tempDocs, TransactionType type)
    {
        List<Transaction> result = new List<Transaction>();
        foreach (Transaction t in tempDocs)
        {
            if (t.TransactionType == type) 
            {
                result.Add(t);
            }
        }
        return result;
    }

    private List<Transaction> SortByAmountDesc(List<Transaction> tempDocs)
    {
        tempDocs.Sort((a, b) => b.MoneyAmount.CompareTo(a.MoneyAmount));
        return new List<Transaction>(tempDocs);
    }

    private void ShowStatistics()
    {
        decimal totalInc = CalculateTotal(currentAccount, TransactionType.Income);
        decimal totalExp = CalculateTotal(currentAccount, TransactionType.Expense);

        PrintStatistics(totalInc, totalExp);
    }

    private decimal CalculateTotal(AccountBase acc, TransactionType type)
    {
        decimal total = 0;
        foreach (Transaction t in acc.Transactions)
        {
            if (t.TransactionType == type) 
            {
                total += t.MoneyAmount;
            }
        }
        return total;
    }

    private void PrintStatistics(decimal totalInc, decimal totalExp)
    {
        Console.WriteLine($"\n--- Фінансова статистика: {currentAccount.Name} ---");
        Console.WriteLine($"Всього зароблено (Income): {totalInc}{currentAccount.AccountCurrency.Symbol}");
        Console.WriteLine($"Всього витрачено (Expense): {totalExp}{currentAccount.AccountCurrency.Symbol}");
        Console.WriteLine($"Чистий грошовий потік: {totalInc - totalExp}{currentAccount.AccountCurrency.Symbol}");
    }
}
