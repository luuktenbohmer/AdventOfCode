using Day16;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.RegularExpressions;

var exampleInput = new Input { FileName = "Input/example.txt" };
var actualInput = new Input { FileName = "Input/input.txt" };
var input = actualInput;

// Parsing
var lines = await File.ReadAllLinesAsync(input.FileName);
var graph = new Graph();
foreach (var line in lines)
{
    var regex = new Regex(@"Valve ([A-Z]{2}) has flow rate=(\d+); tunnels? leads? to valves? (.+)");
    var match = regex.Match(line);
    if (!match.Success)
        throw new Exception($"Could not match line to regex: {line}");

    var node = graph.GetNode(match.Groups[1].Value);
    if (node == null)
    {
        node = new Node
        {
            Id = match.Groups[1].Value
        };

        graph.AddNode(node);
    }

    node.Pressure = int.Parse(match.Groups[2].Value);

    match.Groups[3].Value
        .Split(", ")
        .ToList()
        .ForEach(c => graph.AddEdge(node, c));
}

var startingNode = graph.GetRequiredNode("AA");

// Flatten graph
var fromValves = graph.Nodes.Where(n => n.Pressure > 0).ToHashSet();
var toValves = new List<Node>(fromValves);
fromValves.Add(startingNode);

var distances = new Dictionary<(Node, Node), int>();
foreach (var fromValve in fromValves)
{
    foreach (var toValve in toValves)
    {
        distances[(fromValve, toValve)] = graph.GetDistance(fromValve, toValve);
    }
}

// Setup orders
var watch = Stopwatch.StartNew();
var resultP1 = GetP1(startingNode, toValves, 30, 0);
watch.Stop();
var p1Duration = watch.ElapsedMilliseconds;
Console.WriteLine("P1: " + resultP1);

watch = Stopwatch.StartNew();
var resultP2 = GetP2();
watch.Stop();
var p2Duration = watch.ElapsedMilliseconds;
Console.WriteLine("P2: " + resultP2);

int GetP1(Node currentNode, List<Node> nodesLeft, int timeLeft, int currentScore)
{
    if (!nodesLeft.Any())
        return currentScore;

    return nodesLeft.Select(toNode =>
    {
        var toTimeLeft = timeLeft - distances[(currentNode, toNode)] - 1;
        if (toTimeLeft < 0)
        {
            return currentScore;
        }

        var toNodesLeft = new List<Node>(nodesLeft);
        toNodesLeft.Remove(toNode);
        var toScore = currentScore + toTimeLeft * toNode.Pressure;
        return GetP1(toNode, toNodesLeft, toTimeLeft, toScore);
    }).Max();
}

int GetP2()
{
    var possibleSolutions = GetPossibleSolutions(toValves, new(), new());
    var results = new ConcurrentBag<int>();
    Parallel.ForEach(possibleSolutions, ps =>
    {
        var r = GetP1(startingNode, ps.Item1, 26, 0) + GetP1(startingNode, ps.Item2, 26, 0);
        results.Add(r);
    });

    return results.Max();
}

List<(List<Node>, List<Node>)> GetPossibleSolutions(List<Node> nodesLeft, List<Node> personNodes, List<Node> elephantNodes)
{
    if (!nodesLeft.Any())
        return new List<(List<Node>, List<Node>)> { (personNodes, elephantNodes) };

    var newNodesLeft = new List<Node>(nodesLeft);
    var newNode = nodesLeft.First();
    newNodesLeft.Remove(newNode);
    var r1 = GetPossibleSolutions(newNodesLeft, new List<Node>(personNodes) { newNode }, elephantNodes);
    var r2 = GetPossibleSolutions(newNodesLeft, personNodes, new List<Node>(elephantNodes) { newNode });
    return r1.Concat(r2).ToList();
}

Console.ReadKey();
