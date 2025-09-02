using MooGameV3.Presentation.Console.IO;

namespace MooGameV3.Presentation.Console.Commands;

public sealed class HelpCommand : ICommand
{
	public string Name => "help";
	public IEnumerable<string> Aliases => new[] { "h", "?" };

	public bool Execute(CommandContext ctx)
	{
		ctx.Welcome.Print();
		return false;
	}
}