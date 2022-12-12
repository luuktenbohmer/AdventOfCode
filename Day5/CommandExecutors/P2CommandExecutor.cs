using Day5.Models;

namespace Day5.CommandExecutors;

public class P2CommandExecutor : ICommandExecutor
{
    public string Name => "P2";

    public void Execute(Crates crates, Command command)
    {
        var cratesToMove = new List<char>();
        for (int i = 0; i < command.Amount; i++)
        {
            cratesToMove.Add(crates.PopCrate(command.SourceStack));
        }

        for (int i = cratesToMove.Count - 1; i >= 0; i--)
        {
            crates.PushCrate(command.DestinationStack, cratesToMove[i]);
        }
    }
}
