using NUnit.Framework;
using PersonalFinance.Accounts;
using PersonalFinance.Currencies;
using PersonalFinance.Transactions;
using PersonalFinance.CustomExceptions;

namespace PersonalFinance.Tests;

[TestFixture]
public class AccountTests
{
    [Test]
    public void DebitCard_ProcessIncome_ShouldIncreaseBalance()
    {
        DebitCard account = new DebitCard("Test Debit", 1000m, CurrencyType.UAH);
        Transaction income = new Transaction("Salary", 500m, TransactionType.Income, TransactionCategory.Entertainment);

        account.ProcessTransaction(income);

        Assert.That(account.CurrentBalance, Is.EqualTo(1500m));
        Assert.That(account.Transactions.Count, Is.EqualTo(1));
    }

    [Test]
    public void DebitCard_ProcessExpense_ShouldDecreaseBalance()
    {
        DebitCard account = new DebitCard("Test Debit", 1000m, CurrencyType.UAH);
        Transaction expense = new Transaction("Groceries", 200m, TransactionType.Expense, TransactionCategory.Groceries);

        account.ProcessTransaction(expense);

        Assert.That(account.CurrentBalance, Is.EqualTo(800m));
    }

    [Test]
    public void CreditCard_ProcessExpense_WithinLimit_ShouldDecreaseBalance()
    {
        CreditCard account = new CreditCard("Test Credit", 0m, CurrencyType.UAH, 5000m, 5m);
        Transaction expense = new Transaction("Groceries", 200m, TransactionType.Expense, TransactionCategory.Groceries);

        account.ProcessTransaction(expense);

        Assert.That(account.CurrentBalance, Is.EqualTo(-200m));
    }

    [Test]
    public void CreditCard_ProcessExpense_ExceedingLimit_ShouldThrowInsufficientFundsException()
    {
        CreditCard account = new CreditCard("Test Credit", 0m, CurrencyType.UAH, 1000m, 5m);
        Transaction expense = new Transaction("TV", 1500m, TransactionType.Expense, TransactionCategory.Entertainment);

        Assert.Throws<InsufficientFundsException>(() => account.ProcessTransaction(expense));
        Assert.That(account.CurrentBalance, Is.EqualTo(0m));
        Assert.That(account.Transactions.Count, Is.EqualTo(0));
    }

    [Test]
    public void CreditCard_Interests_ShouldApplyFeeWhenBalanceIsNegative()
    {
        CreditCard account = new CreditCard("Test Credit", -1000m, CurrencyType.UAH, 5000m, 5m);

        account.Interests();

        Assert.That(account.CurrentBalance, Is.EqualTo(-1050m));
        Assert.That(account.Transactions.Count, Is.EqualTo(1));
        Assert.That(account.Transactions[0].Name, Is.EqualTo("Комісія по кредиту"));
    }

    [Test]
    public void CreditCard_Interests_ShouldNotApplyFeeWhenBalanceIsPositive()
    {
        CreditCard account = new CreditCard("Test Credit", 1000m, CurrencyType.UAH, 5000m, 5m);

        account.Interests();

        Assert.That(account.CurrentBalance, Is.EqualTo(1000m));
        Assert.That(account.Transactions.Count, Is.EqualTo(0));
    }

    [Test]
    public void Account_RemoveTransaction_ShouldRevertBalance()
    {
        DebitCard account = new DebitCard("Test", 1000m, CurrencyType.UAH);
        Transaction expense = new Transaction("Test", 300m, TransactionType.Expense, TransactionCategory.Groceries);
        account.ProcessTransaction(expense);
        
        Assert.That(account.CurrentBalance, Is.EqualTo(700m));

        account.RemoveTransaction(expense);

        Assert.That(account.CurrentBalance, Is.EqualTo(1000m));
        Assert.That(account.Transactions.Count, Is.EqualTo(0));
    }
}
