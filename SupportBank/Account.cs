using Microsoft.Extensions.Logging;

namespace SupportBank.SupportBank;

public class Account()
{
    public required string Name { get; init; }
    public required Bank Bank { get; init; }
    
    private decimal GetCredit()
    {
        return Bank.transactions
                        .Where(transaction => transaction.From.Name == Name)
                        .Sum(transaction => transaction.Amount);
    }
    private decimal GetDebt()
    {
        return Bank.transactions
                    .Where(transaction => transaction.To.Name == Name)
                    .Sum(transaction => transaction.Amount);
    }
    public void ListSummary()
    {
        var credit = GetCredit();
        var debt = GetDebt();
        Console.WriteLine($"Account Balance for {Name}: own {debt}, be owned {credit}");
    }

    public void ListDetail()
    {
        var borrowList = Bank.transactions
                        .Where(transaction => transaction.To.Name == Name);
        var lendList = Bank.transactions
                        .Where(transaction => transaction.From.Name == Name);

        Console.WriteLine($"\n===== The details of {Name}\'s debt is as follows: =====");
        foreach (var borrow in borrowList)
        {
            Console.WriteLine($"Borrow {borrow.Amount} from {borrow.From.Name} in {borrow.Date}: {borrow.Narrative}.");
        }
    
        Console.WriteLine($"\n===== The details of {Name}\'s credit is as follows: =====");
        foreach (var lend in lendList)
        {
            Console.WriteLine($"Lend {lend.Amount} from {lend.From.Name} in {lend.Date}: {lend.Narrative}.");
        }
    }   
}

