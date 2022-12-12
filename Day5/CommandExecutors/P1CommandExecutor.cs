using Day5.Models;

namespace Day5.CommandExecutors;

public class P1CommandExecutor : ICommandExecutor
{
    public string Name => "P1";

    public void Execute(Crates crates, Command command)
    {
        for (int i = 0; i < command.Amount; i++)
        {
            var crate = crates.PopCrate(command.SourceStack);
            crates.PushCrate(command.DestinationStack, crate);
        }
    }
}
