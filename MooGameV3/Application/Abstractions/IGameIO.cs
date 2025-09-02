namespace MooGameV3.Application.Abstractions;


public interface IGameIO
{
	string? ReadLine();
	void Write(string text);
	void WriteLine(string text);
	void WriteLineColored(string text, ConsoleColor color);
}