using Microsoft.Extensions.DependencyInjection;
using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Abstractions.Randomness;
using MooGameV3.Application.Services;
using MooGameV3.Domain.Game;
using MooGameV3.Infrastructure.CodeGeneration;
using MooGameV3.Infrastructure.FileSystem;
using MooGameV3.Infrastructure.Randomness;
using MooGameV3.Presentation.Console.Game;
using MooGameV3.Presentation.Console.Intro;
using MooGameV3.Presentation.Console.IO;
using MooGameV3.Presentation.Console.Scoring;

namespace MooGameV3.Presentation.Console.Configuration;

public static class GameServiceCollectionExtensions
{
	public static IServiceCollection AddGameServices(this IServiceCollection services, string scoreFilePath)
	{
		return services
			.AddSingleton<IRandomSource, SystemRandomSource>()
			.AddSingleton<ICodeGenerator, RandomCodeGenerator>()
			.AddSingleton<ICodeEvaluator, CodeEvaluator>()
			.AddSingleton<IScoreRepository>(_ => new FileScoreRepository(
				new FileScoreRepositoryOptions { Path = scoreFilePath }))
			.AddSingleton<ScoreService>();
	}

	public static IServiceCollection AddGameSettings(this IServiceCollection services)
	{
		var gameSettings = new GameSettings(
			DefaultCodeLength: 4,
			AllowDuplicates: false,
			PracticeMode: false);

		return services
			.AddSingleton<IGameSettings>(gameSettings)
			.AddSingleton<IGameRules>(gameSettings)
			.AddSingleton<IGuessValidator, GuessValidator>();
	}

	public static IServiceCollection AddConsoleServices(this IServiceCollection services)
	{
		return services
			.AddSingleton<IGameIO, SystemConsoleIO>()
			.AddSingleton<IWelcomePrinter, WelcomePrinter>()
			.AddSingleton<IPromptService, PromptService>()
			.AddSingleton<IOutputFormatter, OutputFormatter>()
			.AddSingleton<IScorePresenter, ScorePresenter>()
			.AddSingleton<IRoundRunner, RoundRunner>()
			.AddSingleton<ConsoleGameRunner>();
	}
}