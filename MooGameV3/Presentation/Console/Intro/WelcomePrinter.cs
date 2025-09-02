using MooGameV3.Application.Abstractions;

namespace MooGameV3.Presentation.Console.Intro;

public interface IWelcomePrinter
{
	void Print();
}

public sealed class WelcomePrinter : IWelcomePrinter
{
	private readonly IGameIO _io;
	private readonly IGameRules _rules;

	public WelcomePrinter(IGameIO io, IGameRules rules)
	{
		_io = io;
		_rules = rules;
	}

	public void Print()
	{
		_io.WriteLineColored("=== Bulls & Cows (MooGame) ===", System.ConsoleColor.Cyan);
		_io.WriteLine("Goal: guess the secret code. After each guess you get markers:");
		_io.WriteLine("B = right digit in the right position, C = right digit in the wrong position.");
		_io.WriteLine("");

		_io.WriteLine("How it works:");
		_io.WriteLine($"- The code is {_rules.CodeLength} digits long.");
		_io.WriteLine(_rules.AllowDuplicates ? "- Duplicates are allowed." : "- Duplicates are NOT allowed.");
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
}