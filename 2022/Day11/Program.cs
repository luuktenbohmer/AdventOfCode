using Newtonsoft.Json;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

var lines = await File.ReadAllLinesAsync("Input/example.txt");
var monkeys = lines
    .Chunk(7)
    .Select(m => new Monkey(m))
    .ToList();
var monkeysP2 = JsonConvert.DeserializeObject<List<Monkey>>(JsonConvert.SerializeObject(monkeys))!;

// P1
var resultP1 = GetAnswer(monkeys, 20, false);
Console.WriteLine("P1: " + resultP1);

// P2
var resultP2 = GetAnswer(monkeysP2, 10000, true);
Console.WriteLine("P2: " + resultP2);

Console.ReadKey();

long GetAnswer(List<Monkey> monkeys, int numberOfRounds, bool part2)
{
    var totalDivisible = monkeys.Select(m => m.TestDivisibleBy).Aggregate((a, b) => a * b);
    for (int i = 0; i < numberOfRounds; i++)
    {
        foreach (var monkey in monkeys)
        {
            foreach (var item in monkey.Items)
            {
                monkey.ItemsInspected++;
                var newItem = monkey.Operation.GetResult(item);
                if (!part2)
                    newItem /= 3;
                var nextMonkeyNumber = newItem % monkey.TestDivisibleBy == 0 ? monkey.TrueMonkeyNumber : monkey.FalseMonkeyNumber;
                var nextMonkey = monkeys.Single(m => m.Number == nextMonkeyNumber);
                newItem %= totalDivisible;
                nextMonkey.Items.Add(newItem);
            }

            monkey.Items.Clear();
        }
    }

    var orderedMonkeys = monkeys.OrderByDescending(m => m.ItemsInspected).ToList();
    return (long)orderedMonkeys[0].ItemsInspected * orderedMonkeys[1].ItemsInspected;
}

class Monkey
{
    public Monkey() { }
    public Monkey(string[] inputLines)
    {
        var monkeyNumber = MatchOrThrow(inputLines[0], @"Monkey (\d+):");
        var startingItems = MatchOrThrow(inputLines[1], "Starting items: (.*)");
        var operation = MatchOrThrow(inputLines[2], "Operation: new = (.*)");
        var test = MatchOrThrow(inputLines[3], @"Test: divisible by (\d+)");
        var ifTrue = MatchOrThrow(inputLines[4], @"If true: throw to monkey (\d+)");
        var ifFalse = MatchOrThrow(inputLines[5], @"If false: throw to monkey (\d+)");

        Number = int.Parse(monkeyNumber);
        Items = startingItems
            .Split(", ")
            .Select(long.Parse)
            .ToList();
        Operation = new Operation(operation);
        TestDivisibleBy = int.Parse(test);
        TrueMonkeyNumber = int.Parse(ifTrue);
        FalseMonkeyNumber = int.Parse(ifFalse);
    }

    public int Number { get; set; }
    public List<long> Items { get; set; } = new();
    public Operation Operation { get; set; }
    public int TestDivisibleBy { get; set; }
    public int TrueMonkeyNumber { get; set; }
    public int FalseMonkeyNumber { get; set; }

    public int ItemsInspected { get; set; }

    string MatchOrThrow(string input, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
    {
        var result = Regex.Match(input, pattern);
        if (!result.Success)
            throw new Exception($"Could not match '{input}' to '{pattern}'");

        return result.Groups[1].Value;
    }
}

class Operation
{
    public Operation() { }
    public Operation(string rawOperation)
    {
        var splitOperation = rawOperation.Split(' ');

        Op = splitOperation[1].Single();

        if (int.TryParse(splitOperation[0], out var left))
            Left = left;

        if (int.TryParse(splitOperation[2], out var right))
            Right = right;
    }

    public long? Left { get; set; }
    public long? Right { get; set; }
    public char Op { get; set; }

    public long GetResult(long old)
    {
        var left = Left ?? old;
        var right = Right ?? old;

        return Op switch
        {
            '+' => left + right,
            '-' => left - right,
            '/' => left / right,
            '*' => left * right,
            _ => throw new UnreachableException()
        };
    }
}
