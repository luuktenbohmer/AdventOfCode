var inputLines = await File.ReadAllLinesAsync("Input/input.txt");

var emptyChar = '.';
var sandChar = 'o';
var rockChar = '#';

var rockLines = new List<Line>();
foreach (var inputLine in inputLines)
{
    var inputPoints = inputLine.Split(" -> ");
    var rockLine = new Line();
    foreach (var inputPoint in inputPoints)
    {
        var splitPoint = inputPoint.Split(',');
        rockLine.Points.Add(new Point
        {
            X = int.Parse(splitPoint[0]),
            Y = int.Parse(splitPoint[1]),
        });
    }
    rockLines.Add(rockLine);
}

var minX = rockLines.Min(l => l.Points.Min(p => p.X));
var minY = rockLines.Min(l => l.Points.Min(p => p.Y));
var maxX = rockLines.Max(l => l.Points.Max(p => p.X));
var maxY = rockLines.Max(l => l.Points.Max(p => p.Y));
rockLines.SelectMany(l => l.Points).ToList().ForEach(p =>
{
    p.X -= minX;
});
var sandX = 500 - minX;

// Create grid
var width = maxX - minX + 1;
var height = maxY + 1;
var grid = new char[width, height];
for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        grid[x, y] = emptyChar;
    }
}
foreach (var rockLine in rockLines)
{
    for (int i = 0; i < rockLine.Points.Count - 1; i++)
    {
        // Horizontal
        if (rockLine.Points[i].Y == rockLine.Points[i + 1].Y)
        {
            var currentX = rockLine.Points[i].X;
            var sign = Math.Sign(rockLine.Points[i + 1].X - rockLine.Points[i].X);
            do
            {
                grid[currentX, rockLine.Points[i].Y] = rockChar;
                currentX += sign;
            }
            while (currentX != rockLine.Points[i + 1].X + sign);
        }

        // Vertical
        if (rockLine.Points[i].X == rockLine.Points[i + 1].X)
        {
            var currentY = rockLine.Points[i].Y;
            var sign = Math.Sign(rockLine.Points[i + 1].Y - rockLine.Points[i].Y);
            do
            {
                grid[rockLine.Points[i].X, currentY] = rockChar;
                currentY += sign;
            }
            while (currentY != rockLine.Points[i + 1].Y + sign);
        }
    }
}
DrawGrid(grid);

var counter = 0;
while (grid[sandX, 0] == emptyChar)
{
    var sandPoint = new Point { X = sandX, Y = 0 };
    var done = false;
    while (true)
    {
        if (sandPoint.Y >= height - 1 || sandPoint.X < 0 || sandPoint.X >= width - 1)
        {
            // Outside
            done = true;
            break;
        }

        if (grid[sandPoint.X, sandPoint.Y + 1] == emptyChar)
        {
            sandPoint.Y++;
        }
        else
        {
            if (sandPoint.X == 0 || grid[sandPoint.X - 1, sandPoint.Y + 1] == emptyChar)
            {
                sandPoint.X--;
                sandPoint.Y++;
            }
            else if (sandPoint.X == width - 1 || grid[sandPoint.X + 1, sandPoint.Y + 1] == emptyChar)
            {
                sandPoint.X++;
                sandPoint.Y++;
            }
            else
            {
                // Rest
                grid[sandPoint.X, sandPoint.Y] = sandChar;
                counter++;
                break;
            }
        }
    }
    if (false)
        DrawGrid(grid);
    if (done)
        break;
}

Console.WriteLine("P1: " + counter);

void DrawGrid(char[,] grid)
{
    Console.Clear();
    for (int y = 0; y < height; y++)
    {
        Console.Write($"{y} ");
        for (int x = 0; x < width; x++)
        {
            Console.Write(grid[x, y]);
        }
        Console.WriteLine();
    }
}

class Line
{
    public List<Point> Points { get; set; } = new();
}

class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}