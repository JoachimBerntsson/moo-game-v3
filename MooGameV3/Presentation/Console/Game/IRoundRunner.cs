namespace MooGameV3.Presentation.Console.Game;

public sealed record RoundResult(bool EndRound, bool IsWin, int Attempts, int Penalty)
{
	public int Total => Attempts + Penalty;
}

public interface IRoundRunner
{
	RoundResult Run(GameSession session);
}