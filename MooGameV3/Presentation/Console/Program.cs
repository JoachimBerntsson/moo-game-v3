using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Services;
using MooGameV3.Domain.Game;
using MooGameV3.Infrastructure.FileSystem;
using MooGameV3.Infrastructure.Random;
using Microsoft.Extensions.DependencyInjection;

namespace MooGameV3.Presentation.Console;

internal static class Program
{
	private static void Main(string[] args)
	{
		var services = new ServiceCollection()
			.AddSingleton<ICodeGenerator, RandomCodeGenerator>()
			.AddSingleton<ICodeEvaluator, CodeEvaluator>()
			.AddSingleton<IScoreRepository>(sp => new FileScoreRepository("result.txt"))
			.AddSingleton<ScoreService>()
			.BuildServiceProvider();

		var generator = services.GetRequiredService<ICodeGenerator>();
		var evaluator = services.GetRequiredService<ICodeEvaluator>();
		var scores = services.GetRequiredService<ScoreService>();

		bool playOn = true;

		System.Console.WriteLine("Enter your user name:");
		var name = System.Console.ReadLine() ?? "Player";

		while (playOn)
		{
			var goal = generator.Generate();
			System.Console.WriteLine("New game:");
			// Keep practice reveal (comment out to play real games)
			System.Console.WriteLine($"For practice, number is: {goal}");

			int nGuess = 0;
			BullsCows result;

			do
			{
				var guessInput = System.Console.ReadLine() ?? string.Empty;
				nGuess++;
				System.Console.WriteLine(guessInput);
				result = evaluator.Evaluate(goal, new Guess(guessInput));
				System.Console.WriteLine(result.ToMarkers());
			}
			while (!result.IsWin);

			scores.AddScore(name, nGuess);

			System.Console.WriteLine("Player   games average");
			foreach (var p in scores.GetLeaderboard())
			{
				System.Console.WriteLine($"{p.Name,-9}{p.Games,5:D}{p.Average,9:F2}");
			}

			System.Console.WriteLine($"Correct, it took {nGuess} guesses\nContinue?");
			var answer = System.Console.ReadLine();
			if (!string.IsNullOrEmpty(answer) && answer.StartsWith('n'))
				playOn = false;
		}
	}
}
