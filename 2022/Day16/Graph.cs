using System.Diagnostics;
using System.Linq;

namespace Day16;

public class Graph
{
    public List<Node> Nodes { get; set; } = new();

    public void AddNode(Node node)
    {
        Nodes.Add(node);
    }

    public void AddEdge(Node node, string otherId)
    {
        var otherNode = GetNode(otherId);
        if (otherNode is null)
        {
            otherNode = new Node { Id = otherId };
            Nodes.Add(otherNode);
        }
        node.Edges.Add(otherNode);
    }

    public Node? GetNode(string id)
        => Nodes.FirstOrDefault(n => n.Id == id);

    public Node GetRequiredNode(string id)
        => Nodes.First(n => n.Id == id);

    public int GetDistance(Node startNode, Node endNode)
    {
        Nodes.ForEach(n =>
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

            if (node == endNode)
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

        throw new UnreachableException("All nodes should be accessible from one another");
    }
}

public class Node
{
    public required string Id { get; set; }
    public bool Visited { get; set; }
    public int PathLength { get; set; }
    public int Pressure { get; set; }
    public HashSet<Node> Edges { get; set; } = new();
}