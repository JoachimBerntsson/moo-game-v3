using Microsoft.Extensions.DependencyInjection;
using MooGameV3.Domain.Game;
using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Services;
using MooGameV3.Presentation.Console;
using MooGameV3.Presentation.Console.Game;
using MooGameV3.Presentation.Console.IO;
using MooGameV3.Presentation.Console.Intro;
using MooGameV3.Presentation.Console.Scoring;

namespace MooGameV3.UnitTests;

[TestClass]
public class EndToEnd_ConsoleGameTests
{
	private sealed class FixedCodeGenerator : ICodeGenerator
	{
		public SecretCode Generate() => SecretCode.Create("1234", 4);
	}

	private sealed class InMemoryScoreRepository : IScoreRepository
	{
		private readonly List<(string Name, int Guesses)> _items = [];
		public void Append(string playerName, int guesses) => _items.Add((playerName, guesses));
		public IEnumerable<(string Name, int Guesses)> ReadAll() => [.. _items];
	}

	private sealed class StringGameIO(TextReader input, TextWriter output) : IGameIO
	{
		public string? ReadLine() => input.ReadLine();
		public void Write(string text) => output.Write(text);
		public void WriteLine(string text) => output.WriteLine(text);
		public void WriteLineColored(string text, ConsoleColor color) => output.WriteLine(text);
	}

	[TestMethod]
	[TestCategory("EndToEnd")]
	[Timeout(10_000)]
	public void WholeApp_WinOnFirstGuess_PrintsVictoryAndExits()
	{
		var inputScript = string.Join(Environment.NewLine, new[]
		{
			"Alice",
			"1234",
			"n"
		}) + Environment.NewLine;

		using var input = new StringReader(inputScript);
		using var output = new StringWriter();

		var services = new ServiceCollection()

			.AddSingleton<ICodeGenerator, FixedCodeGenerator>()
			.AddSingleton<ICodeEvaluator, CodeEvaluator>()
			.AddSingleton<IScoreRepository, InMemoryScoreRepository>()
			.AddSingleton<ScoreService>()
			.AddSingleton<IGuessValidator, GuessValidator>()
			.AddSingleton<IGameRules>(_ => new GameSettings(
				DefaultCodeLength: 4,
				AllowDuplicates: false,
				PracticeMode: false))

			.AddSingleton<IGameIO>(_ => new StringGameIO(input, output))

			.AddSingleton<IWelcomePrinter, WelcomePrinter>()
			.AddSingleton<IPromptService, PromptService>()
			.AddSingleton<IOutputFormatter, OutputFormatter>()
			.AddSingleton<IScorePresenter, ScorePresenter>()
			.AddSingleton<IRoundRunner, RoundRunner>()
			.AddSingleton<ConsoleGameRunner>();

		var sp = services.BuildServiceProvider();

		var runner = sp.GetRequiredService<ConsoleGameRunner>();
		runner.Run();

		var text = output.ToString();

		StringAssert.Contains(text, "Bulls & Cows", "Bör visa en välkomsttext.");
		StringAssert.Contains(text, "New game", "Bör annonsera nytt spel.");
		StringAssert.Contains(text, "Bulls", "Bör skriva ut bulls/cows-resultat.");
		StringAssert.Contains(text, "Correct", "Bör indikera vinst.");
	}
}