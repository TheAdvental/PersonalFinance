namespace PersonalFinance.Transactions
{
    public class Transaction
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public decimal MoneyAmount { get; set; }
        public TransactionType TransactionType { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public DateTime Date { get; set; }

        public Transaction(string name, int id, decimal money, TransactionType trnType, TransactionCategory trnCategory, DateTime date)
        {
            Name = name;
            Id = id;
            MoneyAmount = money;
            TransactionType = trnType;
            TransactionCategory = trnCategory;
            Date = date;
        }
        public Transaction(string name, int id, decimal money, TransactionType trnType, TransactionCategory trnCategory) : this(name, id, money, trnType, trnCategory, DateTime.Now)
        {
        }
    }
}
