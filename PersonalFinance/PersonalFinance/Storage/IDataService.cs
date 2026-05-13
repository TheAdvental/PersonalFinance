using PersonalFinance.Accounts;

namespace PersonalFinance.Storage
{
    public interface IDataService
    {
        public void SaveAccounts(List<AccountBase> accounts, string filePath);
        List<AccountBase> LoadAccounts(string filePath);
    }
}
