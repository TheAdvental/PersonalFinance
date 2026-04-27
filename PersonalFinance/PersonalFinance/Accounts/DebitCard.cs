using PersonalFinance.Currencies;
using PersonalFinance.CustomExceptions;
using PersonalFinance.Transactions;

namespace PersonalFinance.Accounts
{
    public class DebitCard : AccountBase
    {
        public DebitCard(string name, decimal currBal, CurrencyType accCurr) : base(name, currBal, accCurr)
        {

        }

        public override void ProcessTransaction(Transaction tr)
        {
            if (tr.TransactionType == TransactionType.Expense && CurrentBalance < tr.MoneyAmount)
            {
                throw new InsufficientFundsException($"Недостатньо коштів для здійснення операції. Ваш баланс: {CurrentBalance}");
            }

            base.ProcessTransaction(tr);
        }
    }
}
