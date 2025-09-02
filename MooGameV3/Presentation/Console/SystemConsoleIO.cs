using MooGameV3.Application.Abstractions;

namespace MooGameV3.Presentation.Console;

public sealed class SystemConsoleIO : IGameIO
{
	public string? ReadLine() => System.Console.ReadLine();
	public void Write(string text) => System.Console.Write(text);
	public void WriteLine(string text) => System.Console.WriteLine(text);


	public void WriteLineColored(string text, ConsoleColor color)
	{
		var old = System.Console.ForegroundColor;
		System.Console.ForegroundColor = color;
		System.Console.WriteLine(text);
		System.Console.ForegroundColor = old;
	}
}