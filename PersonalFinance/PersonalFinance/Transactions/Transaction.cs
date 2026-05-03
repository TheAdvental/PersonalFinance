namespace PersonalFinance.Transactions
{
    public class Transaction
    {
        public string Name { get; set; }
        public Guid Id { get; } = Guid.NewGuid();
        public decimal MoneyAmount { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public DateTime Date { get; set; }

        public Transaction(string name, decimal money, TransactionType trnType, TransactionCategory trnCategory, DateTime date)
        {
            Name = name;
            MoneyAmount = money;
            TransactionType = trnType;
            TransactionCategory = trnCategory;
            Date = date;
        }
        public Transaction(string name, decimal money, TransactionType trnType, TransactionCategory trnCategory) : this(name, money, trnType, trnCategory, DateTime.Now)
        {
        }
    }
}
