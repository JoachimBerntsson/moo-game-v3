namespace MooGameV3.Presentation.Console.IO;

public interface IPromptService
{
	string AskNonEmpty(string prompt);
	bool AskYesNo(string prompt);

	string AskPlayerName(
		string? initialPrompt = null,
		string? retryPrompt = null,
		int minLen = 2,
		int maxLen = 32,
		string? defaultName = "Player",
		int maxEmptyTries = 3);
}