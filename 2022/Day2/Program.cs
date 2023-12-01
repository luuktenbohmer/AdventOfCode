// See https://aka.ms/new-console-template for more information
using Day2;

Console.WriteLine("Hello, World!");

var wins = new Dictionary<RPC, RPC>
{
    { RPC.Rock, RPC.Scissors },
    { RPC.Paper, RPC.Rock },
    { RPC.Scissors, RPC.Paper }
};

var pointsMapping = new Dictionary<RPC, int>
{
    { RPC.Rock, 1 },
    { RPC.Paper, 2 },
    { RPC.Scissors, 3 },
};

var lines = await File.ReadAllLinesAsync("Input/Rounds.txt");

// P1
var pointsP1 = 0;
foreach (var line in lines)
{
    // Parsing
    var split = line.Split(' ');
    var other = RPCFactory.GetRPC(split[0]);
    var self = RPCFactory.GetRPC(split[1]);

    // Points
    pointsP1 += GetRoundPoints(self, other);
}
Console.WriteLine(pointsP1);

// P2
var pointsP2 = 0;
foreach (var line in lines)
{
    // Parsing
    var split = line.Split(' ');
    var other = RPCFactory.GetRPC(split[0]);
    var result = RPCFactory.GetResult(split[1]);
    var self = result switch
    {
        Result.Lose => wins[other],
        Result.Draw => other,
        Result.Win => wins[wins[other]],
        _ => throw new Exception($"Invalid result type: {result}")
    };

    pointsP2 += GetRoundPoints(self, other);
}
Console.WriteLine(pointsP2);

Console.ReadKey();

int GetRoundPoints(RPC self, RPC other)
{
    var result = pointsMapping[self];
    if (self == other)
    {
        result += 3;
    }
    else if (wins[self] == other)
    {
        result += 6;
    }

    return result;
}