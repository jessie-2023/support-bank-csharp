namespace SupportBank.SupportBank;
class RawFile
{

    //public required DateOnly Date { get; set; } 
    //String '14/01/2014' was not recognized as a valid DateOnly
    public required string Date { get; set; } 
    public required string To { get; set; }
    public required string From { get; set; }
    public required string Narrative { get; set; }
    public required string Amount { get; set; }

    public override string ToString()
    {
        return $"{Date} {To} {From} {Narrative} {Amount}";
    }
}