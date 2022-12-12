// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("Input/input.txt");

var visited = new HashSet<(int X, int Y)>();
var hPos = (X: 0, Y: 0);
var tPos = (X: 0, Y: 0);
visited.Add((0, 0));
foreach (var line in lines)
{
    var split = line.Split();
    var direction = split[0];
    var steps = int.Parse(split[1]);

    for (int i = 0; i < steps; i++)
    {
        switch (direction)
        {
            case "U":
                hPos.Y++;
                break;
            case "D":
                hPos.Y--;
                break;
            case "L":
                hPos.X--;
                break;
            case "R":
                hPos.X++;
                break;
            default:
                throw new UnreachableException($"Unknown direction: {direction}");
        }

        tPos = CalculateTPos(hPos, tPos);
        visited.Add(tPos);
    }
}

Console.WriteLine("P1: " + visited.Count);

(int X, int Y) CalculateTPos((int X, int Y) h, (int X, int Y) t)
{
    // Move left
    if (t.X > h.X + 1)
    {
        if (t.Y != h.Y)
        {
            t.Y += Math.Sign(h.Y - t.Y);
        }
        t.X--;
    }

    // Move right
    if (t.X < h.X - 1)
    {
        if (t.Y != h.Y)
        {
            t.Y += Math.Sign(h.Y - t.Y);
        }
        t.X++;
    }

    // Move top
    if (t.Y > h.Y + 1)
    {
        if (t.X != h.X)
        {
            t.X += Math.Sign(h.X - t.X);
        }
        t.Y--;
    }

    // Move down
    if (t.Y < h.Y - 1)
    {
        if (t.X != h.X)
        {
            t.X += Math.Sign(h.X - t.X);
        }
        t.Y++;
    }

    return t;
}

Console.ReadKey();