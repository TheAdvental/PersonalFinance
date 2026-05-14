using PersonalFinance.Transactions;
using PersonalFinance.UI_Elements;

namespace PersonalFinance;

public partial class FinanceApp
{
    private void ManageCategories()
    {
        string[] options = { "Переглянути всі категорії", "Додати підкатегорію" };
        int choice = ConsoleUI.ShowInteractiveMenu("КЕРУВАННЯ КАТЕГОРІЯМИ", options);

        if (choice == 0)
        {
            ShowAllCategories();
        }
        else if (choice == 1)
        {
            AddSubCategory();
        }
    }

    private void ShowAllCategories()
    {
        Console.WriteLine("\n--- Всі категорії ---");
        
        List<TransactionCategory> rootCategories = appCategories.Where(c => c.ParentCategory == null).ToList();
        
        foreach (TransactionCategory root in rootCategories)
        {
            PrintCategoryNode(root, 0);
        }
        
        Console.WriteLine("\nНатисніть будь-яку клавішу для повернення...");
        Console.ReadKey();
    }

    private void PrintCategoryNode(TransactionCategory category, int indent)
    {
        string indentStr = new string(' ', indent * 2);
        Console.WriteLine($"{indentStr}- {category.Name}");
        foreach (TransactionCategory sub in category.SubCategories)
        {
            PrintCategoryNode(sub, indent + 1);
        }
    }

    private void AddSubCategory()
    {
        if (appCategories.Count == 0) 
        {
            return;
        }
        
        int parentIdx = ShowCategoryMenu();
        TransactionCategory parentCat = GetCategoryFromChoice(parentIdx);

        Console.Write("Введіть назву нової підкатегорії: ");
        string subName = Console.ReadLine();
        
        if (!string.IsNullOrWhiteSpace(subName))
        {
            TransactionCategory newCat = new TransactionCategory(subName, parentCat);
            appCategories.Add(newCat);
            ConsoleUI.ShowSuccessMessage($"Підкатегорію '{subName}' успішно додано до '{parentCat.Name}'!");
        }
    }

    private int ShowCategoryMenu()
    {
        string[] names = new string[appCategories.Count];
        for (int i = 0; i < appCategories.Count; i++)
        {
            string parentPrefix = appCategories[i].ParentCategory != null ? $"[{appCategories[i].ParentCategory.Name}] -> " : "";
            names[i] = parentPrefix + appCategories[i].Name;
        }
        return ConsoleUI.ShowInteractiveMenu("ВИБЕРІТЬ КАТЕГОРІЮ", names);
    }

    private TransactionCategory GetCategoryFromChoice(int choice)
    {
        if (choice >= 0 && choice < appCategories.Count)
        {
            return appCategories[choice];
        }
        return TransactionCategory.Other;
    }
}
