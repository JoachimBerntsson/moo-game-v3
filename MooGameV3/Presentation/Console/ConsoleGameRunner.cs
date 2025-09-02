using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Services;
using MooGameV3.Domain.Game;

namespace MooGameV3.Presentation.Console;

public sealed class ConsoleGameRunner
{
	private readonly IGameIO _io;
	private readonly ICodeGenerator _generator;
	private readonly ICodeEvaluator _evaluator;
	private readonly ScoreService _scores;
	private readonly IGameRules _rules;
	private readonly IGuessValidator _validator;

	public ConsoleGameRunner(
		IGameIO io,
		ICodeGenerator generator,
		ICodeEvaluator evaluator,
		ScoreService scores,
		IGameRules rules,
		IGuessValidator validator)
	{
		_io = io;
		_generator = generator;
		_evaluator = evaluator;
		_scores = scores;
		_rules = rules;
		_validator = validator;
	}

	public void Run()
	{
		PrintWelcome();

		var name = AskNonEmpty("Enter your player name and press ENTER to start the game: ") ?? "Player";
		_io.WriteLine("");

		var playOn = true;
		while (playOn)
		{
			SecretCode goal = _generator.Generate();

			int expectedLen = _rules.CodeLength;
			string secret = goal.ToString();
			if (secret.Length != expectedLen)
				expectedLen = secret.Length;

			_io.WriteLineColored("New game started! Type /help anytime for commands.", ConsoleColor.DarkCyan);
			if (_rules.PracticeMode)
				_io.WriteLine($"[Practice] Secret: {secret}");

			int nGuess = 0;
			int penalty = 0;

			var commands = BuildCommands(secret, () => penalty += 2);

			while (true)
			{
				_io.Write("> ");
				var input = _io.ReadLine() ?? string.Empty;

				if (input.Length > 0 && input[0] == '/')
				{
					if (commands.TryGetValue(input, out var action))
					{
						bool endRound = action();
						if (endRound)
						{
							playOn = AskYesNo("Start a new game? (y/n): ");
							break;
						}
					}
					else
					{
						_io.WriteLineColored("Unknown command. Type /help.", ConsoleColor.Red);
					}
					continue;
				}

				if (!_validator.TryValidate(input, expectedLen, _rules.AllowDuplicates, out var err))
				{
					_io.WriteLineColored(err, ConsoleColor.Red);
					continue;
				}

				nGuess++;
				BullsCows result = _evaluator.Evaluate(goal, new Guess(input));
				_io.WriteLineColored(result.ToMarkers(), ConsoleColor.Yellow);

				if (result.IsWin)
				{
					int total = nGuess + penalty;
					_io.WriteLineColored($"Correct! It took {total} guesses.", ConsoleColor.Green);
					_scores.AddScore(name, total);
					PrintLeaderboard();
					playOn = AskYesNo("Play again? (y/n): ");
					break;
				}
			}
		}
	}

	private Dictionary<string, Func<bool>> BuildCommands(string secret, Action addHintPenalty)
	{
		// Returnerar true = avsluta rundan, false = fortsätt.
		return new Dictionary<string, Func<bool>>(StringComparer.OrdinalIgnoreCase)
		{
			["/help"] = () =>
			{
				PrintHelp();
				return false;
			},
			["/scores"] = () =>
			{
				PrintLeaderboard();
				return false;
			},
			["/giveup"] = () =>
			{
				_io.WriteLineColored($"You gave up. The code was {secret}.", ConsoleColor.Red);
				return true;
			},
			["/secret"] = () =>
			{
				if (_rules.PracticeMode)
					_io.WriteLineColored($"Secret: {secret}", ConsoleColor.DarkYellow);
				else
					_io.WriteLineColored("Not available (enable practice mode).", ConsoleColor.Red);
				return false;
			},
			["/hint"] = () =>
			{
				int idx = RevealOne(secret);
				addHintPenalty();
				_io.WriteLineColored($"Hint: position {idx + 1} is '{secret[idx]}' (+2 guesses).", ConsoleColor.DarkYellow);
				return false;
			}
		};
	}

	private void PrintWelcome()
	{
		_io.WriteLineColored("=== Bulls & Cows (MooGame) ===", ConsoleColor.Cyan);
		_io.WriteLine("Goal: guess the secret code. After each guess you get markers:");
		_io.WriteLine("B = right digit in the right position, C = right digit in the wrong position.");
		_io.WriteLine("");

		_io.WriteLine("How it works:");
		_io.WriteLine($"- The code is {_rules.CodeLength} digits long.");
		_io.WriteLine(_rules.AllowDuplicates
			? "- Duplicates are allowed."
			: "- Duplicates are NOT allowed.");
		_io.WriteLine("- Digits only (0–9), no spaces.");
		_io.WriteLine("- Example:");
		_io.WriteLine("    Secret: 4271");
		_io.WriteLine("    Guess : 1234");
		_io.WriteLine("    Result: B,CC  (2 is a Bull; 1 and 4 are Cows)");
		_io.WriteLine("");
		_io.WriteLine("Scoring & leaderboard:");
		_io.WriteLine("- Your score is the number of guesses; lower is better.");
		_io.WriteLine("- The leaderboard is sorted by average guesses, then by name.");
		_io.WriteLine("");
		_io.WriteLine("Commands (type anytime):");
		_io.WriteLine("  /help    – show these rules & commands");
		_io.WriteLine("  /scores  – show the leaderboard");
		_io.WriteLine("  /giveup  – give up the current round and reveal the code");
		_io.WriteLine("  /secret  – reveal the code (practice mode only)");
		_io.WriteLine("  /hint    – reveal one correct position (+2 guesses penalty)");
		_io.WriteLine("");
	}

	private void PrintHelp()
	{
		_io.WriteLine("Help:");
		_io.WriteLine($"- Enter a {_rules.CodeLength}-digit number (digits only).");
		_io.WriteLine(_rules.AllowDuplicates
			? "- Duplicates are allowed."
			: "- Duplicates are NOT allowed.");
		_io.WriteLine("- Markers example: BBB,CC means 3 bulls and 2 cows.");
		_io.WriteLine("Commands: /help, /scores, /giveup, /secret (practice), /hint (+2 penalty).");
		_io.WriteLine("");
	}

	private void PrintLeaderboard()
	{
		_io.WriteLine("\nPlayer      Games   Average");
		foreach (var p in _scores.GetLeaderboard())
			_io.WriteLine($"{p.Name,-12}{p.Games,5:D}{p.Average,10:F2}");
		_io.WriteLine("");
	}
	private int RevealOne(string secret) => 0;

	private string? AskNonEmpty(string prompt)
	{
		_io.Write(prompt);
		var s = _io.ReadLine();
		while (string.IsNullOrWhiteSpace(s))
		{
			_io.Write("Please enter a value: ");
			s = _io.ReadLine();
		}
		return s;
	}

	private bool AskYesNo(string prompt)
	{
		_io.Write(prompt);
		var s = (_io.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
		while (!(s.StartsWith('y') || s.StartsWith('n')))
		{
			_io.Write("Please answer y/n: ");
			s = (_io.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
		}
		return s.StartsWith('y');
	}
}
