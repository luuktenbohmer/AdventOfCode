// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("Input/input.txt");
var height = lines.Length;
var width = lines.First().Length;
var trees = new Tree[width, height];
for (int y = 0; y < height; y++)
{
    for (int x = 0; x < width; x++)
    {
        trees[x, y] = new Tree(int.Parse(lines[y][x].ToString()));
    }
}

for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        var tree = trees[x, y];

        int top = 0, bottom = 0, right = 0, left = 0;

        // Top
        for (int i = y - 1; i >= 0; i--)
        {
            top++;
            if (trees[x, i].Height >= tree.Height)
                break;
        }

        // Bottom
        for (int i = y + 1; i < height; i++)
        {
            bottom++;
            if (trees[x, i].Height >= tree.Height)
                break;
        }

        // Left
        for (int i = x - 1; i >= 0; i--)
        {
            left++;
            if (trees[i, y].Height >= tree.Height)
                break;
        }

        // Right
        for (int i = x + 1; i < width; i++)
        {
            right++;
            if (trees[i, y].Height >= tree.Height)
                break;
        }

        tree.ScenicScore = top * bottom * left * right;
    }
}

var highest = 0;
for (int x = 0; x < width; x++)
{
    for (int y = 0; y < height; y++)
    {
        if (trees[x, y].ScenicScore > highest)
            highest = trees[x, y].ScenicScore;
    }
}
Console.WriteLine("P2: " + highest);
Console.ReadKey();

public class Tree
{
    public Tree(int height)
    {
        Height = height;
    }

    public int Height { get; set; }
    public bool Visible { get; set; }
    public int ScenicScore { get; set; }
}