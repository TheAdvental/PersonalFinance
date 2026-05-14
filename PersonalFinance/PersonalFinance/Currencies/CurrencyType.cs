using System.Security;

namespace PersonalFinance.Currencies
{
    public struct CurrencyType
    {
        public string Code { get; set; }
        public string Symbol { get; set; }

        public CurrencyType(string code, string symbol)
        {
            Code = code;
            Symbol = symbol;
        }

        public static CurrencyType USD => new CurrencyType("USD", "$");
        public static CurrencyType UAH => new CurrencyType("UAH", "₴");
        public static CurrencyType EUR => new CurrencyType("EUR", "€");
    }
}
