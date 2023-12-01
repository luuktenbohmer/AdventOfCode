namespace Day5.Models;

public class Command
{
    public string SourceStack { get; set; } = string.Empty;
    public string DestinationStack { get; set; } = string.Empty;
    public int Amount { get; set; }
}
