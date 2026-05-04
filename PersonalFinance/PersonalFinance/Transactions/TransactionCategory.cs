using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace PersonalFinance.Transactions
{
    public class TransactionCategory
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public TransactionCategory ParentCategory { get; set; }
        private List<TransactionCategory> _subCategories = new List<TransactionCategory>();
        public IReadOnlyList<TransactionCategory> SubCategories => _subCategories;
        
        public TransactionCategory(string name)
        {
            Name = name;
        }

        public TransactionCategory(string name, TransactionCategory parent) : this(name)
        {
            ParentCategory = parent;
            parent._subCategories.Add(this);
        }

        public bool IsSubCategoryOf(TransactionCategory potentialParent)
        {
            TransactionCategory current = this;

            while (current != null)
            {
                if (current == potentialParent)
                {
                    return true;
                }

                current = current.ParentCategory;
            }

            return false;
        }

        public static TransactionCategory Groceries => new TransactionCategory("Продукти");
        public static TransactionCategory Entertainment => new TransactionCategory("Розваги");
        public static TransactionCategory Sports => new TransactionCategory("Спорт");
        public static TransactionCategory Travels => new TransactionCategory("Подорожі");
        public static TransactionCategory Charity => new TransactionCategory("Благодійність");
        public static TransactionCategory Debts => new TransactionCategory("Борги");
    }
}
