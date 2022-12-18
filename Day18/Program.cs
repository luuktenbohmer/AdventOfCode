using Day18;

var lines = await File.ReadAllLinesAsync("Input/input.txt");
var points = lines.Select(l =>
{
    var split = l.Split(',');
    return new Point
    (
        int.Parse(split[0]),
        int.Parse(split[1]),
        int.Parse(split[2])
    );
}).ToList();

var xMax = points.Max(p => p.X);
var yMax = points.Max(p => p.Y);
var zMax = points.Max(p => p.Z);
var pointsArray = new bool[xMax + 1, yMax + 1, zMax + 1];
points.ForEach(p => pointsArray[p.X, p.Y, p.Z] = true);

Console.WriteLine("P1: " + GetP1());
Console.WriteLine("P2: " + GetP2());

int GetP1()
{
    var total = 0;
    foreach (var point in points)
    {
        // X
        if (point.X == 0 || !pointsArray[point.X - 1, point.Y, point.Z])
            total++;
        if (point.X == xMax || !pointsArray[point.X + 1, point.Y, point.Z])
            total++;

        // Y
        if (point.Y == 0 || !pointsArray[point.X, point.Y - 1, point.Z])
            total++;
        if (point.Y == yMax || !pointsArray[point.X, point.Y + 1, point.Z])
            total++;

        // Z
        if (point.Z == 0 || !pointsArray[point.X, point.Y, point.Z - 1])
            total++;
        if (point.Z == zMax || !pointsArray[point.X, point.Y, point.Z + 1])
            total++;
    }

    return total;
}

int GetP2()
{
    var total = 0;
    foreach (var point in points)
    {
        // X
        if (point.X == 0 || (!pointsArray[point.X - 1, point.Y, point.Z] && IsReachable(new Point(point.X - 1, point.Y, point.Z))))
            total++;
        if (point.X == xMax || (!pointsArray[point.X + 1, point.Y, point.Z] && IsReachable(new Point(point.X + 1, point.Y, point.Z))))
            total++;

        // Y
        if (point.Y == 0 || (!pointsArray[point.X, point.Y - 1, point.Z] && IsReachable(new Point(point.X, point.Y - 1, point.Z))))
            total++;
        if (point.Y == yMax || (!pointsArray[point.X, point.Y + 1, point.Z] && IsReachable(new Point(point.X, point.Y + 1, point.Z))))
            total++;

        // Z
        if (point.Z == 0 || (!pointsArray[point.X, point.Y, point.Z - 1] && IsReachable(new Point(point.X, point.Y, point.Z - 1))))
            total++;
        if (point.Z == zMax || (!pointsArray[point.X, point.Y, point.Z + 1] && IsReachable(new Point(point.X, point.Y, point.Z + 1))))
            total++;
    }

    return total;
}

bool IsReachable(Point point)
{
    var queue = new Queue<Point>();
    queue.Enqueue(point);
    var visited = new HashSet<Point>();

    while (queue.TryDequeue(out point))
    {
        if (visited.Contains(point))
            continue;
        visited.Add(point);

        if (IsOutside(point))
            return true;

        if (pointsArray[point.X, point.Y, point.Z])
            continue;

        queue.Enqueue(new Point(point.X + 1, point.Y, point.Z));
        queue.Enqueue(new Point(point.X - 1, point.Y, point.Z));
        queue.Enqueue(new Point(point.X, point.Y + 1, point.Z));
        queue.Enqueue(new Point(point.X, point.Y - 1, point.Z));
        queue.Enqueue(new Point(point.X, point.Y, point.Z + 1));
        queue.Enqueue(new Point(point.X, point.Y, point.Z - 1));
    }

    return false;
}

bool IsOutside(Point point) =>
    point.X < 0 || point.X > xMax ||
    point.Y < 0 || point.Y > yMax ||
    point.Z < 0 || point.Z > zMax;

Console.ReadKey();