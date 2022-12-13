using Day13;

var linesP1 = await File.ReadAllLinesAsync("Input/input.txt");

var packetsP1 = new List<(Packet, Packet)>();
for (int i = 0; i < linesP1.Length; i += 3)
{
    packetsP1.Add((new(linesP1[i]), new(linesP1[i + 1])));
}

// P1
var resultP1 = 0;
Console.Write("Right order: ");
for (int i = 0; i < packetsP1.Count; i++)
{
    if (packetsP1[i].Item1.GreaterThan(packetsP1[i].Item2))
        continue;

    Console.Write($"{i + 1},");
    resultP1 += i + 1;
}

Console.WriteLine();
Console.WriteLine("P1: " + resultP1);

// P2
var linesP2 = linesP1
    .Where(l => !string.IsNullOrEmpty(l))
    .ToList();
var divider1 = "[[2]]";
var divider2 = "[[6]]";
linesP2.Add(divider1);
linesP2.Add(divider2);

var packetsP2 = linesP2
    .Select(l => new Packet(l))
    .ToList();

var ordered = packetsP2.Order(new PacketComparer()).ToList();
var i1 = ordered.FindIndex(p => p.RawPacket == divider1) + 1;
var i2 = ordered.FindIndex(p => p.RawPacket == divider2) + 1;

Console.WriteLine("P2: " + i1 * i2);
Console.ReadKey();

public class PacketComparer : IComparer<Packet>
{
    public int Compare(Packet? x, Packet? y)
    {
        if (x.GreaterThan(y))
        {
            return 1;
        }

        return -1;
    }
}