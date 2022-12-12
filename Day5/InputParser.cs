using Day5.Models;

namespace Day5;

public class InputParser
{
    private const string CRATES_FILE = "Input/Crates.txt";
    private const string COMMANDS_FILE = "Input/Commands.txt";

    public async Task<Crates> ParseCrates()
    {
        var result = new Crates();

        var lines = await File.ReadAllLinesAsync(CRATES_FILE);
        if (!lines.Any())
            return result;

        var length = lines.Last().Length;
        var columns = Math.Ceiling(length / 4D);

        for (var i = 0; i < columns; i++)
        {
            var stack = lines.Last().Substring(i * 4, 3).Trim();
            result.AddStack(stack);

            for (var j = lines.Length - 2; j >= 0; j--)
            {
                var crate = lines[j][i * 4 + 1];
                if (crate != ' ')
                    result.PushCrate(stack, crate);
            }
        }

        return result;
    }

    public async Task<List<Command>> ParseCommands()
    {
        var result = new List<Command>();

        var lines = await File.ReadAllLinesAsync(COMMANDS_FILE);
        foreach (var line in lines)
        {
            var splitLine = line.Split(' ');
            result.Add(new()
            {
                Amount = int.Parse(splitLine[1]),
                SourceStack = splitLine[3],
                DestinationStack = splitLine[5]
            });
        }

        return result;
    }
}
