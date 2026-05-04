using PersonalFinance.Accounts;
using PersonalFinance.Transactions;

namespace PersonalFinance.Budget
{
    public class Budget
    {
        public AccountBase TargetAccount { get; set; }
        public TransactionCategory TargetCategory { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal CurrentSpend { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Budget(decimal limit, DateTime start, DateTime end, AccountBase targetAcc, TransactionCategory targetCat)
        {
            LimitAmount = limit;
            StartDate = start;
            EndDate = end;
            TargetAccount = targetAcc;
            TargetCategory = targetCat;
        }

        public Budget(decimal limit, DateTime start, DateTime end, TransactionCategory targetCategory) : this(limit, start, end, null, targetCategory)
        {
        }

        public Budget(decimal limit, DateTime start, DateTime end, AccountBase targetAccount) : this(limit, start, end, targetAccount, null)
        {
        }
    }
}
