using Microsoft.Extensions.DependencyInjection;
using MooGameV3.Application.Abstractions.Randomness;
using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Services;
using MooGameV3.Domain.Game;
using MooGameV3.Infrastructure.CodeGeneration;
using MooGameV3.Infrastructure.Randomness;
using MooGameV3.Infrastructure.FileSystem;
using MooGameV3.Presentation.Console.Game;
using MooGameV3.Presentation.Console.Intro;
using MooGameV3.Presentation.Console.IO;
using MooGameV3.Presentation.Console.Scoring;

namespace MooGameV3.Presentation.Console;

internal static class Program
{
	private static void Main()
	{
		System.Console.OutputEncoding = System.Text.Encoding.UTF8;

		var scoreFile = Path.Combine(AppContext.BaseDirectory, "result.txt");

		var services = new ServiceCollection()

			.AddSingleton<IRandomSource, SystemRandomSource>()
			.AddSingleton<ICodeGenerator, RandomCodeGenerator>()
			.AddSingleton<ICodeEvaluator, CodeEvaluator>()
			.AddSingleton<IScoreRepository>(_ => new FileScoreRepository(scoreFile))
			.AddSingleton<ScoreService>()
			.AddSingleton<IGameIO, SystemConsoleIO>()
			.AddSingleton<IGuessValidator, GuessValidator>()
			.AddSingleton<IGameRules>(_ => new GameSettings(
				DefaultCodeLength: 4,
				AllowDuplicates: false,
				PracticeMode: false))

			.AddSingleton<IWelcomePrinter, WelcomePrinter>()
			.AddSingleton<IPromptService, PromptService>()
			.AddSingleton<IOutputFormatter, OutputFormatter>()
			.AddSingleton<IScorePresenter, ScorePresenter>()
			.AddSingleton<IRoundRunner, RoundRunner>()

			.AddSingleton<ConsoleGameRunner>()
			.BuildServiceProvider();

		services.GetRequiredService<ConsoleGameRunner>().Run();
	}
}
