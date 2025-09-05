namespace MooGameV3.Presentation.Console.Commands;

public sealed class HintCommand : ICommand
{
	public string Name => "hint";
	public IEnumerable<string> Aliases => Array.Empty<string>();

	public bool Execute(CommandContext ctx)
	{
		int idx = ctx.RevealIndex();
		ctx.AddPenalty(2);
		var ch = ctx.Session.SecretString[idx];
		ctx.Out.Hint($"Hint: position {idx + 1} is '{ch}' (+2 guesses).");
		return false;
	}
}