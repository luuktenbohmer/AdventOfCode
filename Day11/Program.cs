// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

var lines = await File.ReadAllLinesAsync("Input/example.txt");
Monkey currentMonkey;
foreach (var line in lines)
{
    var split = line.Split(' ');
    switch (split)
    {
        case ["Monkey", string number]:
            currentMonkey = new Monkey
            {

            }
            break;
    };
}


Console.ReadKey();

public class Monkey
{
    public required int Index { get; set; }
    public List<Item> Items { get; } = new();
    public required Action<Item> Operation { get; set; }
    public required Func<Item, bool> Test { get; set; }
    public required Action<Item> TrueAction { get; set; }
    public required Action<Item> FalseAction { get; set; }
}

public class Item
{
    public int WorryLevel { get; set; }
}