// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var totals = new List<int>();
var lines = await File.ReadAllLinesAsync("Input/Calories.txt");
var current = 0;
foreach (var line in lines)
{
    if (string.IsNullOrEmpty(line))
    {
        totals.Add(current);
        current = 0;
        continue;
    }
    current += int.Parse(line.Trim());
}

totals.Add(current);
var ordered = totals.OrderByDescending(x => x).ToList();

var top1 = ordered.First();
Console.WriteLine($"P1: {top1}");

var top3 = ordered.Take(3).Sum();
Console.WriteLine($"P2: {top3}");

Console.ReadKey();