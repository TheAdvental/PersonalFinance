using System.Text;
using PersonalFinance;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

FinanceApp app = new FinanceApp();
await app.RunAsync();
