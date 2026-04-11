using PersonalFinance.FinTransactions;

namespace PersonalFinance.Account
{
    public abstract class AccountBase
    {
        public string Name { get; set; }
        public decimal CurrentBalance { get; set; }
        public string AccountCurrency { get; set; }
        private List<Transaction> LinkedTransactions { get; set; } = new List<Transaction>();
        public List<Transaction> Transactions => LinkedTransactions;

        protected AccountBase(string name, decimal currBal, string accCurr)
        {
            Name = name;
            CurrentBalance = currBal;
            AccountCurrency = accCurr;
        }

        public virtual void Deposit (Transaction transaction)
        {
            //CurrentBalance += depSum;
        }

        public abstract void Withdraw(decimal withSum);
    }
}
