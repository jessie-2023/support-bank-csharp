using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Logging;

namespace SupportBank.SupportBank;

public class Entrance(ILogger<Entrance> logger) // alternative way: primary constructor
{
    private readonly ILogger<Entrance> _logger = logger;

    public void Run()
    {
        _logger.LogInformation("App starting.");
        
        var fileName = "DodgyTransactions2015.csv"; //"Transactions2014.csv"; // 
        using var streamReader = new StreamReader(fileName);
        using var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture);
        var records = csvReader.GetRecords<RawFile>().ToList();

        var bank = new Bank(_logger);
        foreach (var record in records)
        {
            bank.NewTransaction(
                record.Date,
                record.From,
                record.To,
                record.Narrative,
                record.Amount   
            );
        }
        _logger.LogInformation("Bank established from file: {}", fileName);

        Console.WriteLine($"====Support bank initiated from file: {fileName}====");
        bool quit = true;
        do
        {
            Console.WriteLine("*****");
            Console.WriteLine("[1] List all transactions");
            Console.WriteLine("[2] List account transactions");
            Console.WriteLine("[3] Check account balance");
            Console.Write($"Enter the number of the action would like to take: ");
            var action = Console.ReadLine() ?? "";
            switch (action)
            {
                case "1":
                    bank.ListAllBalance();
                    break;

                case "2":
                    Console.Write($"To list account details, enter the name of the account: ");
                    var accountName = Console.ReadLine() ?? "";
                    bank.ListDetailByName(accountName);
                    break;

                case "3":
                    Console.Write($"To check account balance, enter the name of the account: ");
                    var balanceName = Console.ReadLine() ?? "";
                    bank.CheckBalance(balanceName);
                    break;

                default:
                    Console.WriteLine("Sorry, that wasn't one of the options.");
                    break;
            }   

            Console.Write($"Hit q to quit the program: ");
            quit = Console.ReadLine() =="q";
        } while (!quit);
    }
}