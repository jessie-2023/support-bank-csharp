using SupportBank.SupportBank;

var fileName = "Transactions2014.csv";
// read file & add all the transactions to a list
var fileList = new List<string>(File.ReadAllLines(fileName));

// Console.WriteLine(String.Join("-", fileList[0].Split(",")));

var bank = new Bank();
for (int i = 1; i < fileList.Count; i++)
{
    string[] transaction = fileList[i].Split(",");
    bank.NewTransaction(
        transaction[0],
        transaction[1],
        transaction[2],
        transaction[3],
        decimal.Parse(transaction[4])
    );
}

bank.ListDetailByName("Ben B");
// bank.CheckBalance("Ben B");
// bank.ListAllBalance();

// creates an account for each person & keeps track of how much each person owes / is owed.



// List All - prints out the names of each person, along with the total amount they owe or are owed, as before

// List [Account] - prints out every transaction (with date, narrative, to and amount) for the specific account the user asks for.

