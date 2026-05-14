using NUnit.Framework;
using PersonalFinance.Accounts;
using PersonalFinance.Currencies;
using PersonalFinance.Storage;
using PersonalFinance.Transactions;

namespace PersonalFinance.Tests
{
    [TestFixture]
    public class SerializationTests
    {
        private string testFilePath = "test_data.json";

        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }
        }

        [Test]
        public async Task JsonStorage_ShouldSerializeAndDeserializeAccounts()
        {
            JsonStorage storage = new JsonStorage();
            List<AccountBase> accounts = new List<AccountBase>();
            
            DebitCard debit = new DebitCard("Test Debit", 1500m, CurrencyType.USD);
            Transaction income = new Transaction("Salary", 2000m, TransactionType.Income, TransactionCategory.Salary);
            debit.ProcessTransaction(income);
            accounts.Add(debit);

            CreditCard credit = new CreditCard("Test Credit", 500m, CurrencyType.EUR, 1000m, 0.05m);
            accounts.Add(credit);

            await storage.SaveAccountsAsync(accounts, testFilePath);
            List<AccountBase> loadedAccounts = await storage.LoadAccountsAsync(testFilePath);

            Assert.That(loadedAccounts, Has.Count.EqualTo(2));
            
            Assert.That(loadedAccounts[0], Is.TypeOf<DebitCard>());
            Assert.That(loadedAccounts[0].Name, Is.EqualTo("Test Debit"));
            Assert.That(loadedAccounts[0].CurrentBalance, Is.EqualTo(3500m));
            Assert.That(loadedAccounts[0].AccountCurrency.Code, Is.EqualTo("USD"));
            Assert.That(loadedAccounts[0].Transactions, Has.Count.EqualTo(1));
            Assert.That(loadedAccounts[0].Transactions[0].Name, Is.EqualTo("Salary"));
            Assert.That(loadedAccounts[0].Transactions[0].TransactionCategory.Name, Is.EqualTo("Зарплата"));

            Assert.That(loadedAccounts[1], Is.TypeOf<CreditCard>());
            Assert.That(loadedAccounts[1].Name, Is.EqualTo("Test Credit"));
            Assert.That(loadedAccounts[1].CurrentBalance, Is.EqualTo(500m));
            Assert.That(loadedAccounts[1].AccountCurrency.Code, Is.EqualTo("EUR"));
            Assert.That(((CreditCard)loadedAccounts[1]).CreditLimit, Is.EqualTo(1000m));
        }

        [Test]
        public async Task JsonStorage_LoadAccountsAsync_FileDoesNotExist_ReturnsEmptyList()
        {
            JsonStorage storage = new JsonStorage();

            List<AccountBase> loadedAccounts = await storage.LoadAccountsAsync("non_existent_file.json");

            Assert.That(loadedAccounts, Is.Not.Null);
            Assert.That(loadedAccounts, Is.Empty);
        }
    }
}
