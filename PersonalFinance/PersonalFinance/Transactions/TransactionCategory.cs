using System;
using System.Collections.Generic;
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
        //Groceries,
        //Entertainment,
        //Sports,
        //Travels,
        //Charity,
        //Debts
        public TransactionCategory(string name)
        {
            Name = name;
        }

        public TransactionCategory(string name, TransactionCategory parent) : this(name)
        {
            ParentCategory = parent;
            parent._subCategories.Add(this);
        }
    }
}
