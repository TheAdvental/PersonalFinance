using PersonalFinance.Accounts;

namespace PersonalFinance.Storage
{
    public interface IDataStorage
    {
        public void SaveAccounts(List<AccountBase> accounts, string filePath);
        List<AccountBase> LoadAccounts(string filePath);
    }
}
