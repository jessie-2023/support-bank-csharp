namespace SupportBank.SupportBank;

public class Transaction
{
    public required string Date { get; init; }
    public required Account From {get; init; }
    public required Account To {get; init; }
    public required string Narrative { get; init; }
    public decimal Amount { get; set; } //cannot init for error handling

}