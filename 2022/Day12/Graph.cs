using System.Diagnostics;
using System.Xml.Linq;

namespace Day12;

public class Graph
{
    public Graph(char[,] grid)
    {
        var width = grid.GetLength(0);
        var height = grid.GetLength(1);
        Nodes = new Node[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Nodes[x, y] = new Node(x, y, grid[x, y]);
                if (x > 0)
                    Nodes[x, y].AddEdge(Nodes[x - 1, y]);
                if (y > 0)
                    Nodes[x, y].AddEdge(Nodes[x, y - 1]);
            }
        }
    }

    public Node[,] Nodes { get; set; }

    public List<Node> NodesList => Nodes.Cast<Node>().ToList();

    public int BFS(Node startNode)
    {
        NodesList.ForEach(n =>
        {
            n.PathLength = 0;
            n.Visited = false;
        });
        startNode.Visited = true;
        var queue = new Queue<Node>();
        queue.Enqueue(startNode);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            if (node.Character == 'E')
                return node.PathLength;

            foreach (var edge in node.Edges)
            {
                // Already visited
                if (edge.Visited)
                    continue;

                edge.Visited = true;
                queue.Enqueue(edge);
                edge.PathLength = node.PathLength + 1;
            }
        }

        return -1;
    }

    public void LogVisited()
    {
        var width = Nodes.GetLength(0);
        var height = Nodes.GetLength(1);
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Nodes[x, y].Visited)
                    Console.BackgroundColor = ConsoleColor.Green;
                else
                    Console.ResetColor();

                Console.Write(Nodes[x, y].Character);
            }
            Console.WriteLine();
        }

        Console.ResetColor();
    }
}

public class Node
{
    public Node(int x, int y, char character)
    {
        X = x;
        Y = y;
        Character = character;
        HeightCharacter = character switch
        {
            'S' => 'a',
            'E' => 'z',
            _ => character
        };
    }

    public int X { get; }
    public int Y { get; }
    public char Character { get; }
    public int HeightCharacter { get; }
    public List<Node> Edges { get; } = new();
    public int PathLength { get; set; }
    public bool Visited { get; set; }

    public void AddEdge(Node other)
    {
        if (HeightCharacter - other.HeightCharacter <= 1)
            other.Edges.Add(this);

        if (other.HeightCharacter - HeightCharacter <= 1)
            Edges.Add(other);
    }
}
