using PersonalFinance.Currencies;
using PersonalFinance.Transactions;

namespace PersonalFinance.Accounts
{
    public class CreditCard : AccountBase
    {
        public decimal CreditLimit { get; set; }
        public decimal InterestRate { get; set; }
        public CreditCard(string name, decimal currBal, CurrencyType accCurr) : base(name, currBal, accCurr)
        {
            Name = name;
            CurrentBalance = currBal;
            AccountCurrency = accCurr;
        }

        public override void ProcessTransaction(Transaction tr)
        {
            base.ProcessTransaction(tr);
        }

        public void Interests()
        {

        }
    }
}
