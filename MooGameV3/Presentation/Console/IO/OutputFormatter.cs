using MooGameV3.Application.Abstractions;
using MooGameV3.Domain.Game;

namespace MooGameV3.Presentation.Console.IO;

public sealed class OutputFormatter : IOutputFormatter
{
	private readonly IGameIO _io;

	public OutputFormatter(IGameIO io) => _io = io;

	public void Section(string title)
	{
		_io.WriteLine("");
		_io.WriteLineColored(title, System.ConsoleColor.DarkCyan);
	}

	public void Info(string text) => _io.WriteLineColored(text, System.ConsoleColor.Gray);
	public void Success(string text) => _io.WriteLineColored(text, System.ConsoleColor.Green);
	public void Error(string text) => _io.WriteLineColored(text, System.ConsoleColor.Red);
	public void Hint(string text) => _io.WriteLineColored(text, System.ConsoleColor.DarkYellow);

	public void Markers(BullsCows result, int codeLength)
	{
		_io.WriteLineColored(result.ToMarkers(codeLength), System.ConsoleColor.Yellow);
	}
}