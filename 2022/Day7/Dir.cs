namespace Day7;

public class Dir
{
    public Dir(Dir? parent)
    {
        Parent = parent;
    }

    public Dir? Parent { get; set; }
    public List<int> Files { get; set; } = new();
    public List<Dir> Dirs { get; set; } = new();
    public int Size { get; set; }
}
