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

// Top
for (int i = 0; i < width; i++)
{
    var currentHighest = trees[i, 0].Height;
    trees[i, 0].Visible = true;
    for (int j = 1; j < height; j++)
    {
        if (trees[i, j].Height > currentHighest)
        {
            trees[i, j].Visible = true;
            currentHighest = trees[i, j].Height;
        }
    }
}

// Bottom
for (int i = 0; i < width; i++)
{
    var currentHighest = trees[i, height - 1].Height;
    trees[i, height - 1].Visible = true;
    for (int j = height - 2; j >= 0; j--)
    {
        if (trees[i, j].Height > currentHighest)
        {
            trees[i, j].Visible = true;
            currentHighest = trees[i, j].Height;
        }
    }
}

// Left
for (int i = 0; i < height; i++)
{
    var currentHighest = trees[0, i].Height;
    trees[0, i].Visible = true;
    for (int j = 1; j < width; j++)
    {
        if (trees[j, i].Height > currentHighest)
        {
            trees[j, i].Visible = true;
            currentHighest = trees[j, i].Height;
        }
    }
}

// Right
for (int i = 0; i < height; i++)
{
    var currentHighest = trees[width - 1, i].Height;
    trees[width - 1, i].Visible = true;
    for (int j = width - 2; j >= 0; j--)
    {
        if (trees[j, i].Height > currentHighest)
        {
            trees[j, i].Visible = true;
            currentHighest = trees[j, i].Height;
        }
    }
}

int totalVisible = 0;
for (int i = 0; i < width; i++)
{
    for (int j = 0; j < height; j++)
    {
        if (trees[i, j].Visible)
        {
            totalVisible++;
        }
    }
}

Console.WriteLine("P1: " + totalVisible);
Console.ReadKey();

public class Tree
{
    public Tree(int height)
    {
        Height = height;
    }

    public int Height { get; set; }
    public bool Visible { get; set; }
}