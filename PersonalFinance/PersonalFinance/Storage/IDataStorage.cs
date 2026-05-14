using PersonalFinance.Accounts;

namespace PersonalFinance.Storage
{
    public interface IDataStorage
    {
        Task SaveAccountsAsync(List<AccountBase> accounts, string filePath);
        Task<List<AccountBase>> LoadAccountsAsync(string filePath);
    }
}
