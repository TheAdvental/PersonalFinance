using PersonalFinance.Currencies;
using PersonalFinance.CustomExceptions;
using PersonalFinance.Transactions;

namespace PersonalFinance.Accounts
{
    public class DebitCard : AccountBase
    {
        public DebitCard(string name, decimal currBal, CurrencyType accCurr) : base(name, currBal, accCurr)
        {
            Name = name;
            CurrentBalance = currBal;
            AccountCurrency = accCurr;
        }

        public override void ProcessTransaction(Transaction tr)
        {
            if (tr.TransactionType == TransactionType.Income)
            {
                CurrentBalance += tr.MoneyAmount;
            }
            if (tr.TransactionType == TransactionType.Expense)
            {
                if (CurrentBalance < tr.MoneyAmount)
                {
                    throw new InsufficientFundsException($"Недостатньо коштів на рахунку. Ваш баланс: {CurrentBalance}");
                }
            }
        }
    }
}
