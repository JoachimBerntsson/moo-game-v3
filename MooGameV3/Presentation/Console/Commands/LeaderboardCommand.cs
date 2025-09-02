namespace MooGameV3.Presentation.Console.Commands;

public sealed class LeaderboardCommand : ICommand
{
	public string Name => "scores";
	public IEnumerable<string> Aliases => new[] { "lb", "leaderboard" };

	public bool Execute(CommandContext ctx)
	{
		ctx.ScorePresenter.ShowLeaderboard();
		return false;
	}
}