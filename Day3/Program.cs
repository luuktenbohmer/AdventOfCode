// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

const int OFFSET_UPPER = 38, OFFSET_LOWER = 96;
var lines = await File.ReadAllLinesAsync("Inputs/Rucksacks.txt");

// P1
var totalPriorityP1 = 0;
foreach (var line in lines)
{
    var compartment1 = line.Substring(0, line.Length / 2);
    var compartment2 = line.Substring(line.Length / 2);
    var duplicate = compartment1.Intersect(compartment2).Single();
    totalPriorityP1 += GetPriority(duplicate);
}

Console.WriteLine(totalPriorityP1);

// P2
var totalPriorityP2 = 0;
for (int i = 0; i < lines.Length; i += 3)
{
    var duplicate = lines[i].Intersect(lines[i + 1]).Intersect(lines[i + 2]).Single();
    totalPriorityP2 += GetPriority(duplicate);
}

Console.WriteLine(totalPriorityP2);


Console.ReadKey();

int GetPriority(char input)
{
    return input - (char.IsUpper(input) ? OFFSET_UPPER : OFFSET_LOWER);
}