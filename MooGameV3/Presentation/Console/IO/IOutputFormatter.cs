using MooGameV3.Domain.Game;

namespace MooGameV3.Presentation.Console.IO;

public interface IOutputFormatter
{
	void Section(string title);
	void Info(string text);
	void Success(string text);
	void Error(string text);
	void Hint(string text);
	void Markers(BullsCows result);
}