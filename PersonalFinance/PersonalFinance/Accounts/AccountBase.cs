using PersonalFinance.Transactions;
using PersonalFinance.Currencies;

namespace PersonalFinance.Accounts
{
    public abstract class AccountBase
    {
        public string Name { get; set; }
        public decimal CurrentBalance { get; set; }
        public CurrencyType AccountCurrency { get; set; }
        private List<Transaction> LinkedTransactions { get; set; } = new List<Transaction>();
        public List<Transaction> Transactions => LinkedTransactions;

        protected AccountBase(string name, decimal currBal, CurrencyType accCurr)
        {
            Name = name;
            CurrentBalance = currBal;
            AccountCurrency = accCurr;
        }

        public virtual void ProcessTransaction(Transaction tr)
        {
            if (tr.TransactionType == TransactionType.Income)
            {
                CurrentBalance += tr.MoneyAmount;
            }
            else if (tr.TransactionType == TransactionType.Expense)
            {
                CurrentBalance -= tr.MoneyAmount;
            }
            LinkedTransactions.Add(tr);
        }


    }
}
