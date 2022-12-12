// See https://aka.ms/new-console-template for more information
using Day12;

Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("Input/input.txt");
var width = lines.First().Length;
var height = lines.Length;
var grid = new char[width, height];
for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        grid[x, y] = lines[y][x];
    }
}

var graph = new Graph(grid);

// Part 1
var startNodeP1 = graph.NodesList.Single(n => n.Character == 'S');
var shortestPathP1 = graph.BFS(startNodeP1);

Console.WriteLine("P1: " + shortestPathP1);

// Part 2
var shortestPathP2 = graph.NodesList
    .Where(n => n.HeightCharacter == 'a')
    .Select(graph.BFS)
    .Where(p => p != -1)
    .Min();

Console.WriteLine("P2: " + shortestPathP2);

Console.ReadKey();