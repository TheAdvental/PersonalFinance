using System.Text;
using PersonalFinance.Accounts;
using PersonalFinance.Budgets;
using PersonalFinance.Currencies;
using PersonalFinance.Storage;
using PersonalFinance.Transactions;
using PersonalFinance.UI_Elements;

namespace PersonalFinance;

public partial class FinanceApp
{
    private List<AccountBase> accounts = new List<AccountBase>();
    private List<Budget> budgets = new List<Budget>();
    private List<TransactionCategory> appCategories = new List<TransactionCategory>();
    private IDataStorage storage = new JsonStorage();
    private string storagePath = "data.json";
    private bool isRunning = true;
    private AccountBase currentAccount;

    public FinanceApp()
    {
    }

    public async Task RunAsync()
    {
        await InitializeAsync();

        if (accounts.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("=========================================================");
            Console.WriteLine("||                                                     ||");
            Console.WriteLine("||        ВІТАЄМО У PERSONAL FINANCE MANAGER!          ||");
            Console.WriteLine("||                                                     ||");
            Console.WriteLine("=========================================================\n");
            Console.WriteLine("Схоже, ви тут вперше. Давайте почнемо зі створення вашого");
            Console.WriteLine("першого рахунку!\n");
            CreateAccount();
        }

        while (isRunning)
        {
            List<string> options = new List<string>();
            foreach (AccountBase acc in accounts)
            {
                options.Add($"Рахунок: {acc.Name} ({acc.CurrentBalance} {acc.AccountCurrency.Symbol})");
            }
            options.Add("Створити новий рахунок");
            options.Add("Керування категоріями");
            options.Add("Зберегти дані та вийти");

            int choice = ConsoleUI.ShowInteractiveMenu("ГОЛОВНЕ МЕНЮ", options.ToArray());

            await ProcessMainMenuChoiceAsync(choice);
        }
    }

    private async Task ProcessMainMenuChoiceAsync(int choice)
    {
        int baseIndex = accounts.Count;
        
        if (choice < baseIndex)
        {
            currentAccount = accounts[choice];
            RunAccountMenu();
        }
        else if (choice == baseIndex)
        {
            CreateAccount();
        }
        else if (choice == baseIndex + 1)
        {
            ManageCategories();
        }
        else if (choice == baseIndex + 2)
        {
            await SaveAndExitAsync();
        }
    }

    private void RunAccountMenu()
    {
        bool inAccountMenu = true;
        string[] accOptions = {
            "Переглянути транзакції",
            "Додати витрату (Expense)",
            "Додати дохід (Income)",
            "Видалити транзакцію",
            "Відсортувати та відфільтрувати",
            "Показати статистику",
            "Назад до головного меню"
        };

        Dictionary<int, Action> accountActions = new Dictionary<int, Action>
        {
            { 0, ShowTransactionHistory },
            { 1, AddExpense },
            { 2, AddIncome },
            { 3, DeleteTransaction },
            { 4, FilterTransactions },
            { 5, ShowStatistics },
            { 6, () => inAccountMenu = false }
        };

        while (inAccountMenu && isRunning)
        {
            int choice = ConsoleUI.ShowInteractiveMenu($"МЕНЮ РАХУНКУ: {currentAccount.Name} (Баланс: {currentAccount.CurrentBalance}{currentAccount.AccountCurrency.Symbol})", accOptions);
            
            if (accountActions.ContainsKey(choice))
            {
                accountActions[choice].Invoke();
            }

            if (inAccountMenu && isRunning)
            {
                Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
                Console.ReadKey();
            }
        }
    }

    private async Task InitializeAsync()
    {
        appCategories.AddRange(new[]
        {
            TransactionCategory.Groceries, TransactionCategory.Entertainment,
            TransactionCategory.Sports, TransactionCategory.Travels,
            TransactionCategory.Charity, TransactionCategory.Debts,
            TransactionCategory.Salary, TransactionCategory.Other
        });
        try
        {
            accounts = await storage.LoadAccountsAsync(storagePath);
            if (accounts.Count > 0)
            {
                ConsoleUI.ShowSuccessMessage($"Успішно завантажено {accounts.Count} рахунків.");
            }
        }
        catch (Exception ex)
        {
            ConsoleUI.ShowErrorMessage($"Не вдалося завантажити дані: {ex.Message}");
        }

        foreach (AccountBase acc in accounts)
        {
            acc.OnTransactionAdded += OnTransactionAddedHandler;
        }

        Budget testBudget = new Budget(1000m, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(30), TransactionCategory.Groceries);
        testBudget.OnBudgetExceeded += OnBudgetExceededHandler;
        budgets.Add(testBudget);
    }

    private void OnBudgetExceededHandler(Budget budget)
    {
        ConsoleUI.ShowErrorMessage($"\n[!!!] УВАГА: Бюджет перевищено! Ліміт: {budget.LimitAmount}, Витрачено: {budget.CurrentSpend}");
    }

    private void OnTransactionAddedHandler(Transaction tr, AccountBase senderAccount)
    {
        foreach (Budget budget in budgets)
        {
            budget.ProcessNewTransaction(tr, senderAccount);
        }
    }

    private async Task SaveAndExitAsync()
    {
        await storage.SaveAccountsAsync(accounts, storagePath);
        Console.WriteLine("Завершення роботи. До побачення!");
        isRunning = false;
    }

    private bool ParseDecimalSafe(string input, out decimal result)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            result = 0;
            return false;
        }
        string normalized = input.Replace(',', '.');
        return decimal.TryParse(normalized, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result);
    }
}
