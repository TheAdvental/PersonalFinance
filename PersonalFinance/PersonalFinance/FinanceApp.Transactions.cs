using PersonalFinance.Accounts;
using PersonalFinance.Transactions;
using PersonalFinance.UI_Elements;

namespace PersonalFinance;

public partial class FinanceApp
{
    private void ShowTransactionHistory()
    {
        ConsoleUI.TransactionsOutput(currentAccount);
    }

    private void AddExpense()
    {
        Console.Write("Введіть назву витрати: ");
        string expenseName = Console.ReadLine();
        Console.Write("Сума: ");
        string amountInput = Console.ReadLine();
        
        int catChoice = ShowCategoryMenu();
        TryAddExpense(expenseName, amountInput, catChoice);
    }

    private void TryAddExpense(string expenseName, string amountInput, int catChoice)
    {
        if (ParseDecimalSafe(amountInput, out decimal amount))
        {
            TransactionCategory cat = GetCategoryFromChoice(catChoice);
            Transaction tr = new Transaction(expenseName, amount, TransactionType.Expense, cat);
            ProcessTransactionSafe(tr, "Витрату додано!");
        }
        else
        {
            ConsoleUI.ShowErrorMessage("Помилка: Неправильний формат числа для суми.");
        }
    }

    private void ProcessTransactionSafe(Transaction tr, string successMsg)
    {
        try 
        {
            currentAccount.ProcessTransaction(tr);
            ConsoleUI.ShowSuccessMessage(successMsg);
        }
        catch (Exception ex)
        {
            ConsoleUI.ShowErrorMessage($"Помилка: {ex.Message}");
        }
    }

    private void AddIncome()
    {
        Console.Write("Назва доходу: ");
        string incName = Console.ReadLine();
        Console.Write("Сума: ");
        string amountInput = Console.ReadLine();

        int catChoice = ShowCategoryMenu();
        TryAddIncome(incName, amountInput, catChoice);
    }

    private void TryAddIncome(string incName, string amountInput, int catChoice)
    {
        if (ParseDecimalSafe(amountInput, out decimal incAmount))
        {
            TransactionCategory cat = GetCategoryFromChoice(catChoice);
            Transaction tr = new Transaction(incName, incAmount, TransactionType.Income, cat);
            currentAccount.ProcessTransaction(tr);
            ConsoleUI.ShowSuccessMessage("Дохід додано!");
        }
        else
        {
            ConsoleUI.ShowErrorMessage("Помилка: Неправильний формат числа для суми.");
        }
    }

    private void DeleteTransaction()
    {
        ConsoleUI.TransactionsOutput(currentAccount);
        
        if (currentAccount.Transactions.Count == 0) 
        {
            return;
        }
        Console.Write("Введіть номер транзакції для видалення: ");
        TryRemoveTransaction(Console.ReadLine());
    }

    private void TryRemoveTransaction(string input)
    {
        if (!int.TryParse(input, out int transNum)) 
        {
            return;
        }
        if (IsValidTransactionIndex(currentAccount, transNum))
        {
            RemoveTransactionSafe(transNum - 1);
        }
    }

    private bool IsValidTransactionIndex(AccountBase delAccount, int transNum)
    {
        return transNum > 0 && transNum <= delAccount.Transactions.Count;
    }

    private void RemoveTransactionSafe(int index)
    {
        try
        {
            currentAccount.RemoveTransaction(currentAccount.Transactions[index]);
            ConsoleUI.ShowSuccessMessage("Транзакцію успішно видалено!");
        }
        catch (Exception ex)
        {
            ConsoleUI.ShowErrorMessage("Помилка видалення: " + ex.Message);
        }
    }
}
