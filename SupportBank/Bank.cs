using Microsoft.Extensions.Logging;
using NLog;

namespace SupportBank.SupportBank;

public class Bank
{
    private readonly ILogger<Entrance> _logger;
    private readonly HashSet<Account> _accounts = [];
    private readonly Dictionary<string, Account> _accountRegister = [];
    private readonly HashSet<Transaction> _transactions = [];

    public Bank(ILogger<Entrance> logger)
    {
        _logger = logger;
    }
    public Account OpenAccount(string name)
    {
        var newAccount = new Account
        { 
            Name = name,
            Bank = this
        };
        _accounts.Add(newAccount);
        _accountRegister.Add(name, newAccount);
        _logger.LogInformation($"{name} is opening a new account.");
        return newAccount;
    }
    public Account GetAccountByName(string name)
    {
        return _accountRegister.ContainsKey(name) ? _accountRegister[name] : OpenAccount(name);
    }

    public void NewTransaction(string date, string from, string to, string narrative, string amount)
    {
        var newTransaction = new Transaction
        {
            Date = date,
            From = GetAccountByName(from),
            To = GetAccountByName(to),
            Narrative = narrative
        };

        try
        {
            newTransaction.Amount = decimal.Parse(amount); // error handling, _logger
        }
        catch (FormatException exception)
        {
            
            _logger.LogError(
                "Transaction failed to register: Invalid format for amount: {0}, from {1} to {2} in {3} ", 
                amount, from, to, date
            );
            _logger.LogDebug(exception.StackTrace);
        }

        _transactions.Add(newTransaction);
    }

    private decimal GetCreditByName(string name)
    {
        return _transactions
                        .Where(transaction => transaction.From.Name == name)
                        .Sum(transaction => transaction.Amount);
    }
    private decimal GetDebtByName(string name)
    {
        return _transactions
                    .Where(transaction => transaction.To.Name == name)
                    .Sum(transaction => transaction.Amount);
    }
    public void CheckBalance(string name)
    {
        var credit = GetCreditByName(name);
        var debt = GetDebtByName(name);
        Console.WriteLine($"Account Balance for {name}: own {debt}, be owned {credit}");
    }

    public void ListAllBalance()
    {
        foreach (var name in _accountRegister.Keys)
        {
            CheckBalance(name);
        }
    }

    public void ListBorrowByName(string name)
    {
        var borrowList = _transactions
                        .Where(transaction => transaction.To.Name == name);
        foreach (var borrow in borrowList)
        {
            Console.WriteLine($"Borrow {borrow.Amount} from {borrow.From.Name} in {borrow.Date}: {borrow.Narrative}.");
        }
    }   

    public void ListLendByName(string name)
    {
        var lendList = _transactions
                        .Where(transaction => transaction.From.Name == name);
        foreach (var lend in lendList)
        {
            Console.WriteLine($"Lend {lend.Amount} from {lend.From.Name} in {lend.Date}: {lend.Narrative}.");
        }
    }   

    public void ListDetailByName(string name)
    {
        Console.WriteLine($"\n===== The details of {name}\'s debt is as follows: =====");
        ListBorrowByName(name);
        Console.WriteLine($"\n===== The details of {name}\'s credit is as follows: =====");
        ListLendByName(name);

    }
}