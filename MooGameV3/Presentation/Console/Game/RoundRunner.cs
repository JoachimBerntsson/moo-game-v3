using MooGameV3.Application.Abstractions;
using MooGameV3.Domain.Game;
using MooGameV3.Presentation.Console.Commands;
using MooGameV3.Presentation.Console.Intro;
using MooGameV3.Presentation.Console.IO;
using MooGameV3.Presentation.Console.Scoring;

namespace MooGameV3.Presentation.Console.Game;

public sealed class RoundRunner(
	IGameIO io,
	IGameRules rules,
	IGuessValidator validator,
	ICodeEvaluator evaluator,
	IOutputFormatter output,
	IWelcomePrinter welcome,
	IScorePresenter scoresPresenter) : IRoundRunner
{
	private readonly IGameIO _io = io;
	private readonly IGameRules _rules = rules;
	private readonly IGuessValidator _validator = validator;
	private readonly ICodeEvaluator _evaluator = evaluator;
	private readonly IOutputFormatter _out = output;
	private readonly IWelcomePrinter _welcome = welcome;
	private readonly IScorePresenter _scoresPresenter = scoresPresenter;

	public RoundResult Run(GameSession session)
	{
		var commands = new ICommand[]
		{
			new HelpCommand(),
			new LeaderboardCommand(),
			new GiveUpCommand(),
			new SecretCommand(),
			new HintCommand()
		};
		var router = new CommandRouter(commands);

		var ctx = new CommandContext
		{
			IO = _io,
			Out = _out,
			Welcome = _welcome,
			Rules = _rules,
			Scores = null!,
			ScorePresenter = _scoresPresenter,
			Session = session,
			RevealIndex = () => Random.Shared.Next(session.SecretString.Length),
			AddPenalty = p => session.AddPenalty(p)
		};

		_out.Info("New game started! Type /help anytime for commands.");
		if (_rules.PracticeMode)
			_out.Hint($"[Practice] Secret: {session.SecretString}");

		while (true)
		{
			_io.Write("> ");
			var input = _io.ReadLine() ?? string.Empty;

			if (input.Length > 0 && input[0] == '/')
			{
				var (handled, endRound) = router.TryExecute(input, ctx);
				if (!handled)
					_out.Error("Unknown command. Type /help.");
				if (endRound)
					return new RoundResult(EndRound: true, IsWin: false, Attempts: session.Attempts, Penalty: session.Penalty);
				continue;
			}

			if (!_validator.TryValidate(input, session.ExpectedLength, _rules.AllowDuplicates, out var err))
			{
				_out.Error(err);
				continue;
			}

			session.IncrementAttempts();

			BullsCows result = _evaluator.Evaluate(session.Secret, new Guess(input));
			_out.Markers(result, session.ExpectedLength);

			if (result.IsWin(session.ExpectedLength))
			{
				_out.Success($"Correct! It took {session.TotalScore} guesses.");
				return new RoundResult(EndRound: true, IsWin: true, Attempts: session.Attempts, Penalty: session.Penalty);
			}
		}
	}
}
