// See https://aka.ms/new-console-template for more information
using Day5;
using Day5.CommandExecutors;

var commandExecutors = new List<ICommandExecutor>
{
    new P1CommandExecutor(),
    new P2CommandExecutor()
};

var parser = new InputParser();
var commands = await parser.ParseCommands();

foreach (var commandExecutor in commandExecutors)
{
    var crates = await parser.ParseCrates();
    commands.ForEach(c => commandExecutor.Execute(crates, c));

    Console.Write($"{commandExecutor.Name}: ");
    crates.ListStacks().ForEach(s => Console.Write(crates.PopCrate(s)));
    Console.WriteLine();
}

Console.ReadKey();