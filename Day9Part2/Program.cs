// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("Input/input.txt");

var visited = new HashSet<Position>();
var knots = Enumerable.Range(1, 10).Select(i => new Position(0, 0)).ToList();
visited.Add(new Position(0, 0));
foreach (var line in lines)
{
    var split = line.Split();
    var direction = split[0];
    var steps = int.Parse(split[1]);

    for (int c = 0; c < steps; c++)
    {
        switch (direction)
        {
            case "U":
                knots.First().Y++;
                break;
            case "D":
                knots.First().Y--;
                break;
            case "L":
                knots.First().X--;
                break;
            case "R":
                knots.First().X++;
                break;
            default:
                throw new UnreachableException($"Unknown direction: {direction}");
        }
        for (int i = 1; i < knots.Count; i++)
        {
            CalculateKnotPos(knots[i - 1], knots[i]);
        }
        visited.Add(knots.Last().GetClone());
    }
}

Console.WriteLine("P2: " + visited.Count);

Position CalculateKnotPos(Position h, Position t)
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

record Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }

    public Position GetClone() => new Position(X, Y);
}
