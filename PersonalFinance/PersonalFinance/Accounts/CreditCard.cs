using PersonalFinance.Currencies;
using PersonalFinance.CustomExceptions;
using PersonalFinance.Transactions;

namespace PersonalFinance.Accounts
{
    public class CreditCard : AccountBase
    {
        public decimal CreditLimit { get; set; }
        public decimal InterestRate { get; set; }
        public decimal AvailableFunds => CurrentBalance + CreditLimit;
        public CreditCard(string name, decimal currBal, CurrencyType accCurr, decimal creditLimit, decimal interestRate) : base(name, currBal, accCurr)
        {
            CreditLimit = creditLimit;
            InterestRate = interestRate / 100;
        }

        public override void ProcessTransaction(Transaction tr)
        {
            if (tr.TransactionType == TransactionType.Expense && AvailableFunds < tr.MoneyAmount)
            {
                throw new InsufficientFundsException($"Недостатньо коштів для здійснення операції. Ваш баланс: {CurrentBalance}");
            }

            base.ProcessTransaction(tr);
        }

        public void Interests()
        {
            if (CurrentBalance >= 0)
            {
                return;
            }

            decimal interestsSum = Math.Abs(CurrentBalance) * InterestRate;

            Transaction fee = new Transaction("Комісія по кредиту", interestsSum, TransactionType.Expense, TransactionCategory.Debts);

            ProcessTransaction(fee);
        }
    }
}
