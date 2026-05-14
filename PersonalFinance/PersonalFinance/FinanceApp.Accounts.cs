using PersonalFinance.Accounts;
using PersonalFinance.Currencies;
using PersonalFinance.UI_Elements;

namespace PersonalFinance;

public partial class FinanceApp
{
    private void CreateAccount()
    {
        Console.Write("Назва рахунку: ");
        string name = Console.ReadLine();
        Console.Write("Початковий баланс (напр. 1000,50): ");
        string balanceInput = Console.ReadLine();

        string[] currencies = { "UAH (Гривня)", "USD (Долар)", "EUR (Євро)" };
        int currChoice = ConsoleUI.ShowInteractiveMenu("ВИБЕРІТЬ ВАЛЮТУ РАХУНКУ", currencies);

        string[] types = { "Дебетовий", "Кредитний" };
        int typeChoice = ConsoleUI.ShowInteractiveMenu("ВИБЕРІТЬ ТИП РАХУНКУ", types);

        ProcessAccountTypeSelection(typeChoice, name, balanceInput, currChoice);
    }

    private void ProcessAccountTypeSelection(int typeChoice, string name, string balanceInput, int currChoice)
    {
        if (typeChoice == 1) 
        {
            Console.Write("Кредитний ліміт: ");
            string limitInput = Console.ReadLine();
            Console.Write("Відсоткова ставка (напр. 5 для 5%): ");
            TryCreateCreditAccount(name, balanceInput, currChoice, limitInput, Console.ReadLine());
        }
        else 
        {
            TryCreateDebitAccount(name, balanceInput, currChoice);
        }
    }

    private void TryCreateDebitAccount(string name, string balanceInput, int currChoice)
    {
        if (ParseDecimalSafe(balanceInput, out decimal balance))
        {
            CurrencyType curr = GetCurrencyFromChoice(currChoice);
            AccountBase newAcc = new DebitCard(name, balance, curr);
            FinalizeAccountCreation(newAcc);
        }
        else
        {
            ConsoleUI.ShowErrorMessage("Помилка: Неправильний формат числа для балансу.");
        }
    }

    private void TryCreateCreditAccount(string name, string balInput, int currChoice, string limitInput, string rateInput)
    {
        if (ParseDecimalSafe(balInput, out decimal bal))
        {
            ProcessCreditLimits(name, bal, currChoice, limitInput, rateInput);
        }
        else
        {
            ConsoleUI.ShowErrorMessage("Помилка: Неправильний формат числа для балансу.");
        }
    }

    private void ProcessCreditLimits(string name, decimal bal, int currChoice, string limitInput, string rateInput)
    {
        if (ParseDecimalSafe(limitInput, out decimal limit) && ParseDecimalSafe(rateInput, out decimal rate))
        {
            CurrencyType curr = GetCurrencyFromChoice(currChoice);
            AccountBase newAcc = new CreditCard(name, bal, curr, limit, rate);
            FinalizeAccountCreation(newAcc);
        }
        else
        {
            ConsoleUI.ShowErrorMessage("Помилка: Неправильний формат числа для ліміту або ставки.");
        }
    }

    private void FinalizeAccountCreation(AccountBase newAcc)
    {
        newAcc.OnTransactionAdded += OnTransactionAddedHandler;
        accounts.Add(newAcc);
        ConsoleUI.ShowSuccessMessage("Рахунок успішно створено.");
    }

    private CurrencyType GetCurrencyFromChoice(int currChoice)
    {
        if (currChoice == 1) 
        {
            return CurrencyType.USD;
        }
        if (currChoice == 2) 
        {
            return CurrencyType.EUR;
        }
        return CurrencyType.UAH;
    }
}
