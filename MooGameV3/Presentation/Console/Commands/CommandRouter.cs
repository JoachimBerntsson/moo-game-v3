namespace MooGameV3.Presentation.Console.Commands;

public sealed class CommandRouter
{
	private readonly Dictionary<string, ICommand> _byTrigger;

	public CommandRouter(IEnumerable<ICommand> commands)
	{
		_byTrigger = new(StringComparer.OrdinalIgnoreCase);
		foreach (var cmd in commands)
		{
			_byTrigger["/" + cmd.Name] = cmd;
			foreach (var a in cmd.Aliases)
				_byTrigger["/" + a] = cmd;
		}
	}

	public (bool handled, bool endRound) TryExecute(string input, CommandContext ctx)
	{
		if (_byTrigger.TryGetValue(input, out var cmd))
			return (true, cmd.Execute(ctx));
		return (false, false);
	}
}