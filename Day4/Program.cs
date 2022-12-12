// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("Input/Assignments.txt");

var numberOfFullyContains = 0;
var numberOfOverlaps = 0;
foreach (var line in lines)
{
    var ranges = line.Split(',');
    var range1 = ParseRange(ranges[0]);
    var range2 = ParseRange(ranges[1]);
    if (Contains(range1, range2) || Contains(range2, range1))
    {
        numberOfFullyContains++;
    }
    if (Overlaps(range1, range2))
    {
        numberOfOverlaps++;
    }
}

Console.WriteLine("P1: " + numberOfFullyContains);
Console.WriteLine("P2: " + numberOfOverlaps);


Console.ReadKey();

(int start, int end) ParseRange(string range)
{
    var split = range.Split('-');
    return (int.Parse(split[0]), int.Parse(split[1]));
}

bool Contains((int start, int end) outer, (int start, int end) inner)
{
    return outer.start <= inner.start && outer.end >= inner.end;
}

bool Overlaps((int start, int end) range1, (int start, int end) range2)
{
    return range1.start <= range2.end && range1.end >= range2.start;
}