using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Services;

namespace MooGameV3.Presentation.Console.Scoring;

public sealed class ScorePresenter : IScorePresenter
{
	private readonly IGameIO _io;
	private readonly ScoreService _scores;

	public ScorePresenter(IGameIO io, ScoreService scores)
	{
		_io = io;
		_scores = scores;
	}

	public void ShowLeaderboard()
	{
		_io.WriteLine("\nPlayer      Games   Average");
		foreach (var p in _scores.GetLeaderboard())
			_io.WriteLine($"{p.Name,-12}{p.Games,5:D}{p.Average,10:F2}");
		_io.WriteLine("");
	}
}