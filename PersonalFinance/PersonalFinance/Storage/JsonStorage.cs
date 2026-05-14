using System.Text.Json;
using PersonalFinance.Accounts;
using PersonalFinance.UI_Elements;

namespace PersonalFinance.Storage
{
    public class JsonStorage : IDataStorage
    {
        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        };

        public async Task SaveAccountsAsync(List<AccountBase> accounts, string filePath)
        {
            try
            {
                using FileStream createStream = File.Create(filePath);
                await JsonSerializer.SerializeAsync(createStream, accounts, _options);

                ConsoleUI.ShowSuccessMessage("Збережено успішно");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка збереження JSON: {ex.Message}", ex);
            }
        }

        public async Task<List<AccountBase>> LoadAccountsAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<AccountBase>();
            }

            try
            {
                using FileStream openStream = File.OpenRead(filePath);
                if (openStream.Length == 0)
                {
                    return new List<AccountBase>();
                }

                return await JsonSerializer.DeserializeAsync<List<AccountBase>>(openStream, _options) ?? new List<AccountBase>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка завантаження JSON: {ex.Message}", ex);
            }
        }
    }
}