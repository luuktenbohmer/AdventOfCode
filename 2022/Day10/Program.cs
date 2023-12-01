// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("Input/input.txt");

var currentValue = 1;
var values = new List<int>() { currentValue };
foreach (var line in lines)
{
    var split = line.Split(' ');
    switch (split)
    {
        case ["noop"]:
            values.Add(currentValue);
            break;
        case ["addx", string addX]:
            values.Add(currentValue);

            var addXValue = int.Parse(addX);
            currentValue += addXValue;
            values.Add(currentValue);
            break;
    }
}

// P1
var cycles = new int[] { 20, 60, 100, 140, 180, 220 };
var strengthP1 = cycles.Select(GetSignalStrength).Sum();

Console.WriteLine("P1: " + strengthP1);

// P2
// Rows
for (int y = 0; y < 6; y++)
{
    // Cells
    for (int x = 0; x < 40; x++)
    {
        var character = '.';
        if (Math.Abs(values[y * 40 + x] - x) <= 1)
            character = '#';

        Console.Write(character);
    }

    Console.WriteLine();
}

Console.ReadKey();

int GetSignalStrength(int cycle)
{
    return values[cycle - 1] * cycle;
}