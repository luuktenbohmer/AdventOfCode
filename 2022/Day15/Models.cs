namespace Day15;

public class Reading
{
    public Point Sensor { get; set; }
    public Point ClosestBeacon { get; set; }
    public int Distance => Math.Abs(Sensor.X - ClosestBeacon.X) + Math.Abs(Sensor.Y - ClosestBeacon.Y);

    public List<int> GetNoBeaconPositions(int y, bool includeBeacons)
    {
        var yDiff = Math.Abs(Sensor.Y - y);
        if (yDiff > Distance)
            return new();

        var result = Enumerable.Range(Sensor.X - (Distance - yDiff), 1 + (Distance - yDiff) * 2).ToList();
        if (y == ClosestBeacon.Y && !includeBeacons)
            result.Remove(ClosestBeacon.X);

        return result;
    }

    public (int min, int max) GetNoBeaconRange(int y)
    {
        var yDiff = Math.Abs(Sensor.Y - y);
        if (yDiff > Distance)
            return (-1, -1);

        return (Sensor.X - (Distance - yDiff), Sensor.X + (Distance - yDiff));
    }
}

public class Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
}