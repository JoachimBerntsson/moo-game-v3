namespace MooGameV3.Presentation.Console.Commands;

public sealed class GiveUpCommand : ICommand
{
	public string Name => "giveup";
	public IEnumerable<string> Aliases => Array.Empty<string>();

	public bool Execute(CommandContext ctx)
	{
		ctx.Out.Error($"You gave up. The code was {ctx.Session.SecretString}.");
		return true;
	}
}