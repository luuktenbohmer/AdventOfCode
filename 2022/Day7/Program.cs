using Day7;
using System.Diagnostics;

var lines = await File.ReadAllLinesAsync("Input/input.txt");
var allDirs = new List<Dir>();

Dir rootDir = null!;
Dir currentDir = null!;
foreach (var line in lines)
{
    var words = line.Split(' ');
    switch (words)
    {
        case ["$", "cd", "/"]:
            rootDir = new Dir(null);
            currentDir = rootDir;
            allDirs.Add(rootDir);
            break;
        case ["$", "cd", ".."]:
            currentDir = currentDir.Parent!;
            break;
        case ["$", "cd", string dir]:
            var newDir = new Dir(currentDir);
            currentDir.Dirs.Add(newDir);
            currentDir = newDir;
            allDirs.Add(newDir);
            break;
        case ["$", "ls"]: break;
        case ["dir", string dirName]: break;
        case [string size, string fileName]:
            currentDir.Files.Add(int.Parse(size));
            break;
        default:
            throw new UnreachableException("Input line should always match");
    }
}

SetSizes(rootDir);
var totalS = allDirs.Where(d => d.Size < 100_000).Sum(d => d.Size);
Console.WriteLine("P1: " + totalS);

var totalSize = 70_000_000;
var totalNeeded = 30_000_000;
var leftOver = totalSize - rootDir.Size;
var needed = totalNeeded - leftOver;
var directoryToDelete = allDirs.OrderBy(d => d.Size).Where(d => d.Size > needed).First().Size;

Console.WriteLine("P2: " + directoryToDelete);

Console.ReadKey();

void SetSizes(Dir dir)
{
    foreach (var subDir in dir.Dirs)
    {
        SetSizes(subDir);
    }

    dir.Size = dir.Files.Sum() + dir.Dirs.Sum(d => d.Size);
}
