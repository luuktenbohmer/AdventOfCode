using Day5.Models;

namespace Day5.CommandExecutors;

public interface ICommandExecutor
{
    string Name { get; }
    void Execute(Crates crates, Command command);
}
