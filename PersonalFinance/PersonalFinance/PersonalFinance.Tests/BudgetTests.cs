using NUnit.Framework;
using PersonalFinance.Budgets;
using PersonalFinance.Accounts;
using PersonalFinance.Currencies;
using PersonalFinance.Transactions;

namespace PersonalFinance.Tests;

[TestFixture]
public class BudgetTests
{
    [Test]
    public void ProcessNewTransaction_WhenExpenseExceedsLimit_ShouldTriggerEvent()
    {
        TransactionCategory category = TransactionCategory.Groceries;
        Budget budget = new Budget(1000m, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(10), category);
        DebitCard account = new DebitCard("Test", 5000m, CurrencyType.UAH);
        Transaction expense = new Transaction("Food", 1200m, TransactionType.Expense, category);
        
        bool eventTriggered = false;
        budget.OnBudgetExceeded += (b) => eventTriggered = true;

        budget.ProcessNewTransaction(expense, account);

        Assert.That(eventTriggered, Is.True);
        Assert.That(budget.CurrentSpend, Is.EqualTo(1200m));
    }

    [Test]
    public void ProcessNewTransaction_WhenIncome_ShouldNotCountTowardsSpend()
    {
        TransactionCategory category = TransactionCategory.Groceries;
        Budget budget = new Budget(1000m, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(10), category);
        DebitCard account = new DebitCard("Test", 5000m, CurrencyType.UAH);
        Transaction income = new Transaction("Salary", 2000m, TransactionType.Income, category);

        budget.ProcessNewTransaction(income, account);

        Assert.That(budget.CurrentSpend, Is.EqualTo(0m));
    }

    [Test]
    public void ProcessNewTransaction_WhenOutsideDateRange_ShouldNotCount()
    {
        TransactionCategory category = TransactionCategory.Groceries;
        Budget budget = new Budget(1000m, DateTime.Now.AddDays(1), DateTime.Now.AddDays(10), category);
        DebitCard account = new DebitCard("Test", 5000m, CurrencyType.UAH);
        Transaction expense = new Transaction("Food", 500m, TransactionType.Expense, category);
        
        budget.ProcessNewTransaction(expense, account);

        Assert.That(budget.CurrentSpend, Is.EqualTo(0m));
    }

    [Test]
    public void ProcessNewTransaction_WhenDifferentCategory_ShouldNotCount()
    {
        Budget budget = new Budget(1000m, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(10), TransactionCategory.Entertainment);
        DebitCard account = new DebitCard("Test", 5000m, CurrencyType.UAH);
        Transaction expense = new Transaction("Food", 500m, TransactionType.Expense, TransactionCategory.Groceries);

        budget.ProcessNewTransaction(expense, account);

        Assert.That(budget.CurrentSpend, Is.EqualTo(0m));
    }

    [Test]
    public void ProcessNewTransaction_WhenSubCategory_ShouldCountTowardsSpend()
    {
        TransactionCategory parentCat = new TransactionCategory("Food");
        TransactionCategory childCat = new TransactionCategory("Groceries", parentCat);

        Budget budget = new Budget(1000m, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(10), parentCat);
        DebitCard account = new DebitCard("Test", 5000m, CurrencyType.UAH);
        Transaction expense = new Transaction("Supermarket", 500m, TransactionType.Expense, childCat);

        budget.ProcessNewTransaction(expense, account);

        Assert.That(budget.CurrentSpend, Is.EqualTo(500m));
    }
}
