using System.Diagnostics;

var lines = await File.ReadAllLinesAsync("Input/input.txt");
var initialHeight = lines.Length;
var initialWidth = lines[0].Length;
// Change for P1
var rounds = 2000;
//var rounds = 10;
var height = initialHeight + rounds * 2;
var width = initialWidth + rounds * 2;
var grid = new Elf[width, height];
var elves = new List<Elf>();
for (int y = 0; y < initialHeight; y++)
{
    for (int x = 0; x < initialWidth; x++)
    {
        if (lines[y][x] == '#')
        {
            var elf = new Elf(x + rounds, y + rounds);
            grid[elf.Current.X, elf.Current.Y] = elf;
            elves.Add(elf);
        }
    }
}

var order = new Queue<char>();
order.Enqueue('N');
order.Enqueue('S');
order.Enqueue('W');
order.Enqueue('E');

var resultP2 = 0;
for (int i = 0; i < rounds; i++)
{
    if (ExecuteRound())
    {
        resultP2 = i + 1;
        break;
    }
}
//DrawGrid();
var left = elves.Min(e => e.Current.X);
var right = elves.Max(e => e.Current.X);
var top = elves.Min(e => e.Current.Y);
var bottom = elves.Max(e => e.Current.Y);
var resultP1 = (right - left + 1) * (bottom - top + 1) - elves.Count;

Console.WriteLine("P1: " + resultP1);
Console.WriteLine("P2: " + resultP2);

Console.ReadKey();

bool ExecuteRound()
{
    // Only check elves which have neighbors
    var elvesToCheck = elves.Where(e =>
    {
        var x = e.Current.X;
        var y = e.Current.Y;
        return grid[x - 1, y - 1] is not null || grid[x, y - 1] is not null || grid[x + 1, y - 1] is not null
         || grid[x + 1, y] is not null || grid[x - 1, y] is not null
         || grid[x - 1, y + 1] is not null || grid[x, y + 1] is not null || grid[x + 1, y + 1] is not null;
    });

    // Set proposed next points
    foreach (var direction in order)
    {
        foreach (var elf in elvesToCheck.Where(e => e.Proposed is null))
        {
            switch (direction)
            {
                case 'N':
                    if (grid[elf.Current.X, elf.Current.Y - 1] is null && grid[elf.Current.X - 1, elf.Current.Y - 1] is null && grid[elf.Current.X + 1, elf.Current.Y - 1] is null)
                        elf.Proposed = new Point(elf.Current.X, elf.Current.Y - 1);
                    break;
                case 'S':
                    if (grid[elf.Current.X, elf.Current.Y + 1] is null && grid[elf.Current.X - 1, elf.Current.Y + 1] is null && grid[elf.Current.X + 1, elf.Current.Y + 1] is null)
                        elf.Proposed = new Point(elf.Current.X, elf.Current.Y + 1);
                    break;
                case 'E':
                    if (grid[elf.Current.X + 1, elf.Current.Y] is null && grid[elf.Current.X + 1, elf.Current.Y - 1] is null && grid[elf.Current.X + 1, elf.Current.Y + 1] is null)
                        elf.Proposed = new Point(elf.Current.X + 1, elf.Current.Y);
                    break;
                case 'W':
                    if (grid[elf.Current.X - 1, elf.Current.Y] is null && grid[elf.Current.X - 1, elf.Current.Y - 1] is null && grid[elf.Current.X - 1, elf.Current.Y + 1] is null)
                        elf.Proposed = new Point(elf.Current.X - 1, elf.Current.Y);
                    break;
                default:
                    throw new UnreachableException();
            }
        }
    }

    // Move first consideration to back of queue
    var firstConsidered = order.Dequeue();
    order.Enqueue(firstConsidered);

    // Filter elves to move based on proposed and whether another elf wants to move to the same spot
    var elvesToMove = elves.Where(e => e.Proposed is not null)
        .GroupBy(e => e.Proposed)
        .Where(e => e.Count() == 1)
        .Select(e => e.Single())
        .ToList();

    // Part 2: Done when elves won't move anymore
    if (!elvesToMove.Any())
    {
        return true;
    }
    elvesToMove.ForEach(e => e.Moved = true);
    //DrawGrid();

    // Move elves
    elvesToMove.ForEach(e => grid[e.Current.X, e.Current.Y] = null);
    foreach (var elf in elvesToMove)
    {
        grid[elf.Proposed.X, elf.Proposed.Y] = elf;
        elf.Current = elf.Proposed;
    }

    // Reset proposed state
    elves.ForEach(e =>
    {
        e.Proposed = null;
        e.Moved = false;
    });

    return false;
}

void DrawGrid()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var e = grid[x, y];
            var c = '.';
            if (e is not null)
            {
                c = '#';
                if (e.Moved)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.Write(c);
        }

        Console.WriteLine();
    }
    Console.WriteLine();
    Console.WriteLine(new string('=', width + 2));
    Console.WriteLine();
}

class Elf
{
    public Elf(int x, int y)
    {
        Current = new Point(x, y);
    }

    public Point Current { get; set; }
    public Point Proposed { get; set; }
    public bool Moved { get; set; }
}

record Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
}