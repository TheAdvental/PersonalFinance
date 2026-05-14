using PersonalFinance.Currencies;
using PersonalFinance.Transactions;
using System.Text.Json.Serialization;

namespace PersonalFinance.Accounts
{
    [JsonDerivedType(typeof(DebitCard), typeDiscriminator: "debit")]
    [JsonDerivedType(typeof(CreditCard), typeDiscriminator: "credit")]      
    public abstract class AccountBase
    {
        public delegate void TransactionAddedHandler(Transaction tr, AccountBase senderAccount);
        public event TransactionAddedHandler OnTransactionAdded;

        public string Name { get; set; }
        public decimal CurrentBalance { get; set; }
        public CurrencyType AccountCurrency { get; set; }
        [JsonInclude]
        private List<Transaction> LinkedTransactions { get; set; } = new List<Transaction>();
        
        [JsonIgnore]
        public IReadOnlyList<Transaction> Transactions => LinkedTransactions;

        public AccountBase() {}

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

            OnTransactionAdded?.Invoke(tr, this);
        }

        public virtual void RemoveTransaction(Transaction tr)
        {
            if (LinkedTransactions.Remove(tr))
            {
                if (tr.TransactionType == TransactionType.Income)
                {
                    CurrentBalance -= tr.MoneyAmount;
                }
                else if (tr.TransactionType == TransactionType.Expense)
                {
                    CurrentBalance += tr.MoneyAmount;
                }
            }
        }
    }
}
