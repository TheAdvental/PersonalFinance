using NUnit.Framework;
using PersonalFinance.Transactions;

namespace PersonalFinance.Tests;

[TestFixture]
public class CategoryTests
{
    [Test]
    public void IsSubCategoryOf_WhenDirectChild_ShouldReturnTrue()
    {
        TransactionCategory parentCat = new TransactionCategory("Food");
        TransactionCategory childCat = new TransactionCategory("Groceries", parentCat);

        bool isSub = childCat.IsSubCategoryOf(parentCat);

        Assert.That(isSub, Is.True);
    }

    [Test]
    public void IsSubCategoryOf_WhenGrandChild_ShouldReturnTrue()
    {
        TransactionCategory grandParentCat = new TransactionCategory("Living");
        TransactionCategory parentCat = new TransactionCategory("Food", grandParentCat);
        TransactionCategory childCat = new TransactionCategory("Groceries", parentCat);

        bool isSub = childCat.IsSubCategoryOf(grandParentCat);

        Assert.That(isSub, Is.True);
    }

    [Test]
    public void IsSubCategoryOf_WhenUnrelated_ShouldReturnFalse()
    {
        TransactionCategory cat1 = new TransactionCategory("Food");
        TransactionCategory cat2 = new TransactionCategory("Entertainment");

        bool isSub = cat1.IsSubCategoryOf(cat2);

        Assert.That(isSub, Is.False);
    }

    [Test]
    public void IsSubCategoryOf_WhenSelf_ShouldReturnTrue()
    {
        TransactionCategory cat = new TransactionCategory("Food");

        bool isSub = cat.IsSubCategoryOf(cat);

        Assert.That(isSub, Is.True);
    }
}
