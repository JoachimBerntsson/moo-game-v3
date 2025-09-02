using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Services;
using MooGameV3.Domain.Game;
using MooGameV3.Presentation.Console.Game;
using MooGameV3.Presentation.Console.Intro;
using MooGameV3.Presentation.Console.IO;
using MooGameV3.Presentation.Console.Scoring;

namespace MooGameV3.Presentation.Console;

public sealed class ConsoleGameRunner
{
	private readonly IGameIO _io;
	private readonly ICodeGenerator _generator;
	private readonly ScoreService _scores;
	private readonly IGameRules _rules;
	private readonly IRoundRunner _roundRunner;
	private readonly IWelcomePrinter _welcome;
	private readonly IPromptService _prompts;
	private readonly IScorePresenter _scorePresenter;

	public ConsoleGameRunner(
		IGameIO io,
		ICodeGenerator generator,
		ScoreService scores,
		IGameRules rules,
		IRoundRunner roundRunner,
		IWelcomePrinter welcome,
		IPromptService prompts,
		IScorePresenter scorePresenter)
	{
		_io = io;
		_generator = generator;
		_scores = scores;
		_rules = rules;
		_roundRunner = roundRunner;
		_welcome = welcome;
		_prompts = prompts;
		_scorePresenter = scorePresenter;
	}

	public void Run()
	{
		_welcome.Print();

		var name = _prompts.AskPlayerName();
		_io.WriteLine("");

		var playOn = true;
		while (playOn)
		{
			SecretCode secret = _generator.Generate();

			int expectedLen = _rules.CodeLength;
			string secretStr = secret.ToString();
			if (secretStr.Length != expectedLen)
				expectedLen = secretStr.Length;

			var session = new GameSession(secret, expectedLen, name);
			var result = _roundRunner.Run(session);

			if (result.IsWin)
			{
				var total = result.Total;
				_scores.AddScore(name, total);
				_scorePresenter.ShowLeaderboard();
				playOn = _prompts.AskYesNo("Play again? (y/n): ");
			}
			else
			{
				playOn = _prompts.AskYesNo("Start a new game? (y/n): ");
			}
		}
	}
}
