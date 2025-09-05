namespace MooGameV3.Presentation.Console.Commands;

public sealed class SecretCommand : ICommand
{
	public string Name => "secret";
	public IEnumerable<string> Aliases => Array.Empty<string>();

	public bool Execute(CommandContext ctx)
	{
		if (ctx.Rules.PracticeMode)
			ctx.Out.Hint($"Secret: {ctx.Session.SecretString}");
		else
			ctx.Out.Error("Not available (enable practice mode).");

		return false;
	}
}