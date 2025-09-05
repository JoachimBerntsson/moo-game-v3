using Microsoft.Extensions.DependencyInjection;
using MooGameV3.Presentation.Console.Configuration;
using MooGameV3.Presentation.Console.Game;

namespace MooGameV3.Presentation.Console;

internal static class Program
{
	private static void Main()
	{
		System.Console.OutputEncoding = System.Text.Encoding.UTF8;

		var scoreFile = Path.Combine(AppContext.BaseDirectory, "result.txt");

		var services = new ServiceCollection()
			.AddGameServices(scoreFile)
			.AddGameSettings()
			.AddConsoleServices()
			.BuildServiceProvider();

		services.GetRequiredService<ConsoleGameRunner>().Run();
	}
}