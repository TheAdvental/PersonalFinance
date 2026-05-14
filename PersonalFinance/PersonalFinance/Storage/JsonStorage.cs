using System.Text.Json;
using PersonalFinance.Accounts;
using PersonalFinance.UI_Elements;

namespace PersonalFinance.Storage
{
    public class JsonStorage : IDataStorage
    {
        private JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        public void SaveAccounts(List<AccountBase> accounts, string filePath)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(accounts, _options);

                File.WriteAllText(filePath, jsonString);

                ConsoleUI.ShowSuccessMessage("Збережено успішно");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка збереження JSON: {ex.Message}", ex);
            }
        }

        public List<AccountBase> LoadAccounts(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<AccountBase>();
            }

            try
            {
                string jsonString = File.ReadAllText(filePath);

                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    return new List<AccountBase>();
                }

                return JsonSerializer.Deserialize<List<AccountBase>>(jsonString, _options) ?? new List<AccountBase>();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Помилка завантаження JSON: {ex.Message}", ex);
            }
        }
    }
}