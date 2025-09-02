using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Services;
using MooGameV3.Domain.Game;
using MooGameV3.Infrastructure.FileSystem;
using MooGameV3.Infrastructure.Random;
using Microsoft.Extensions.DependencyInjection;

namespace MooGameV3.Presentation.Console;

internal static class Program
{
	private static void Main()
	{
		var services = new ServiceCollection()
			.AddSingleton<ICodeGenerator, RandomCodeGenerator>()
			.AddSingleton<ICodeEvaluator, CodeEvaluator>()
			.AddSingleton<IScoreRepository>(sp => new FileScoreRepository("result.txt"))
			.AddSingleton<ScoreService>()
			.AddSingleton<IGameIO, SystemConsoleIO>()
			.AddSingleton<IGuessValidator, GuessValidator>()
			.AddSingleton<IGameRules>(sp => new GameSettings(DefaultCodeLength: 4, AllowDuplicates: false, PracticeMode: false))
			.AddSingleton<ConsoleGameRunner>()
			.BuildServiceProvider();

		services.GetRequiredService<ConsoleGameRunner>().Run();
	}
}