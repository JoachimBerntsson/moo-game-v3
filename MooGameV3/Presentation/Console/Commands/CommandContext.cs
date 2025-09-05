using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Services;
using MooGameV3.Presentation.Console.Game;
using MooGameV3.Presentation.Console.Intro;
using MooGameV3.Presentation.Console.IO;
using MooGameV3.Presentation.Console.Scoring;

namespace MooGameV3.Presentation.Console.Commands;

public sealed class CommandContext
{
	public required IGameIO IO { get; init; }
	public required IOutputFormatter Out { get; init; }
	public required IWelcomePrinter Welcome { get; init; }
	public required IGameRules Rules { get; init; }
	public required ScoreService Scores { get; init; }
	public required IScorePresenter ScorePresenter { get; init; }
	public required GameSession Session { get; init; }

	public required Func<int> RevealIndex { get; init; }

	public required Action<int> AddPenalty { get; init; }
}