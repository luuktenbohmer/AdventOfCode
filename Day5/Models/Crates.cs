namespace Day5.Models;

public class Crates
{
    private Dictionary<string, Stack<char>> Stacks { get; set; } = new();

    public void AddStack(string key) => Stacks.Add(key, new());
    public List<string> ListStacks() => Stacks.Keys.ToList();

    public void PushCrate(string stack, char crate) => Stacks[stack].Push(crate);
    public char PopCrate(string stack) => Stacks[stack].Pop();
    public char PeekCrate(string stack) => Stacks[stack].Peek();
}
