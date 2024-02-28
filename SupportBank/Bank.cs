using Microsoft.Extensions.Logging;

namespace SupportBank.SupportBank;

public class Bank
{
    private readonly ILogger<Entrance> _logger;
    private readonly HashSet<Account> _accounts = [];
    private readonly Dictionary<string, Account> _accountRegister = [];
    public required HashSet<Transaction> transactions = [];


    public Bank(ILogger<Entrance> logger)
    {
        _logger = logger;
    }
    
    public Account OpenAccount(string name)
    {
        var newAccount = new Account
        { 
            Name = name,
            Bank = this,
        };
        _accounts.Add(newAccount);
        _accountRegister[name] = newAccount;
        _logger.LogInformation($"{name} is opening a new account.");
        return newAccount;
    }

    public Account? GetAccountByName(string name, bool open = true)
    {
        if(_accountRegister.TryGetValue(name, out Account account)) 
        {
            return account;
        } 
        else if (open)
        {
            return OpenAccount(name);
        }
        else
        {
            return null;
        }
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

        transactions.Add(newTransaction);
    }

    public void ListAll()
    {
        foreach (var name in _accountRegister.Keys)
        {
            GetAccountByName(name).ListSummary();
        }
    }

}