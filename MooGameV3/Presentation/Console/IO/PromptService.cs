using System.Text.RegularExpressions;
using MooGameV3.Application.Abstractions;

namespace MooGameV3.Presentation.Console.IO;

public sealed class PromptService : IPromptService
{
	private readonly IGameIO _io;
	public PromptService(IGameIO io) => _io = io;

	public string AskNonEmpty(string prompt)
	{
		_io.Write(prompt);
		var s = _io.ReadLine();
		while (string.IsNullOrWhiteSpace(s))
		{
			_io.Write("Please enter a value: ");
			s = _io.ReadLine();
		}
		return s!;
	}

	public bool AskYesNo(string prompt)
	{
		_io.Write(prompt);
		var s = (_io.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
		while (!(s.StartsWith('y') || s.StartsWith('n')))
		{
			_io.Write("Please answer y/n: ");
			s = (_io.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
		}
		return s.StartsWith('y');
	}

	public string AskPlayerName(
		string? initialPrompt = null,
		string? retryPrompt = null,
		int minLen = 2,
		int maxLen = 32,
		string? defaultName = "Player",
		int maxEmptyTries = 3)
	{
		initialPrompt ??= "Enter your player name and press ENTER to start the game: ";
		retryPrompt ??= "Please enter a name (2–32 characters): ";

		int emptyCount = 0;

		while (true)
		{
			_io.Write(emptyCount == 0 ? initialPrompt : retryPrompt);
			var input = _io.ReadLine();

			var normalized = input is null ? string.Empty
				: Regex.Replace(input.Trim(), @"\s+", " ");

			var isValid =
				!string.IsNullOrWhiteSpace(normalized) &&
				normalized.Length >= minLen &&
				normalized.Length <= maxLen &&
				normalized.Any(char.IsLetterOrDigit);

			if (isValid)
				return normalized;

			if (string.IsNullOrWhiteSpace(normalized))
			{
				emptyCount++;
				if (defaultName is not null && emptyCount >= maxEmptyTries)
				{
					_io.WriteLine($"Using default name \"{defaultName}\".");
					return defaultName;
				}
			}
		}
	}
}
