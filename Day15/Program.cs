using Day15;
using System.Text.RegularExpressions;

var exampleY = 10;
var exampleMaxCoordinate = 20;
var inputY = 2_000_000;
var inputMaxCoordinate = 4_000_000;

var lines = await File.ReadAllLinesAsync("Input/input.txt");
var readings = new List<Reading>();
foreach (var line in lines)
{
    var regex = new Regex("Sensor at x=(-?\\d+), y=(-?\\d+): closest beacon is at x=(-?\\d+), y=(-?\\d+)");
    var result = regex.Match(line);
    readings.Add(new Reading
    {
        Sensor = new Point(int.Parse(result.Groups[1].Value), int.Parse(result.Groups[2].Value)),
        ClosestBeacon = new Point(int.Parse(result.Groups[3].Value), int.Parse(result.Groups[4].Value))
    });
}

Console.WriteLine($"P1: {GetNumberOfPositionsWhichCannotContainBeacon(inputY, false).Count}");
var pointP2 = GetPossibleBeaconPosition(inputMaxCoordinate);
Console.WriteLine($"{pointP2.X}, {pointP2.Y}");

long resultP2 = 4_000_000L * pointP2.X + pointP2.Y;

Console.WriteLine($"P2: {resultP2}");

List<int> GetNumberOfPositionsWhichCannotContainBeacon(int y, bool includeBeacons)
{
    return readings
        .Select(r => r.GetNoBeaconPositions(y, includeBeacons))
        .SelectMany(x => x)
        .Distinct()
        .ToList();
}

Point GetPossibleBeaconPosition(int maxCoordinate)
{
    for (int i = 0; i <= maxCoordinate; i++)
    {
        var noBeaconRanges = readings
            .Select(r => r.GetNoBeaconRange(i))
            .OrderBy(r => r.min)
            .ToList();
        var currentMax = 0;
        foreach (var range in noBeaconRanges)
        {
            if (range.min > currentMax + 1)
            {
                return new Point(currentMax + 1, i);
            }
            currentMax = Math.Max(range.max, currentMax);
        }
        //for (int j = 0; j < maxCoordinate; j++)
        //{
        //    if (noBeaconRanges.All(r => r.min > j && r.max < j))
        //        return new Point(j, i);
        //}
        //var positions = GetNumberOfPositionsWhichCannotContainBeacon(i, true)
        //    .Where(p => p >= 0 && p <= maxCoordinate)
        //    .ToList();
        //if (positions.Count != maxCoordinate + 1)
        //{
        //    var test = Enumerable.Range(0, maxCoordinate + 1);
        //    var x = test.Except(positions).Single();
        //    return new Point(x, i);
        //}
    }

    return null;
}

Console.ReadKey();