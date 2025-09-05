namespace MooGameV3.Presentation.Console.Commands;

public interface ICommand
{
	string Name { get; }
	IEnumerable<string> Aliases { get; }
	bool Execute(CommandContext ctx);
}