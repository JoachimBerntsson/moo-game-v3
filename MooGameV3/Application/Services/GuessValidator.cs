using MooGameV3.Application.Abstractions;


namespace MooGameV3.Application.Services;


public sealed class GuessValidator : IGuessValidator
{
	public bool TryValidate(string guess, int expectedLength, bool allowDuplicates, out string error)
	{
		error = string.Empty;
		if (string.IsNullOrWhiteSpace(guess))
		{
			error = "Empty input. Type digits only.";
			return false;
		}
		if (guess.Length != expectedLength)
		{
			error = $"Guess must be {expectedLength} digits.";
			return false;
		}
		if (!guess.All(char.IsDigit))
		{
			error = "Digits only (0–9).";
			return false;
		}
		if (!allowDuplicates && guess.Distinct().Count() != expectedLength)
		{
			error = "No duplicate digits allowed.";
			return false;
		}
		return true;
	}
}