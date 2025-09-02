using System.Diagnostics.CodeAnalysis;
using MooGameV3.Application.Abstractions;

namespace MooGameV3.Application.Services;

public sealed class GuessValidator : IGuessValidator
{
	public bool TryValidate(
		string guess,
		int requiredLength,
		bool allowDuplicateDigits,
		[NotNullWhen(false)] out string? error)
	{
		if (requiredLength <= 0)
			throw new ArgumentOutOfRangeException(nameof(requiredLength), "Required length must be positive.");

		if (string.IsNullOrWhiteSpace(guess))
		{
			error = "Empty input. Type digits only.";
			return false;
		}

		if (guess.Length != requiredLength)
		{
			error = $"Guess must be {requiredLength} digits.";
			return false;
		}

		bool onlyDigits = guess.All(c => c >= '0' && c <= '9');
		if (!onlyDigits)
		{
			error = "Digits only (0–9).";
			return false;
		}

		if (!allowDuplicateDigits)
		{
			var seen = new HashSet<char>(requiredLength);
			foreach (char c in guess)
			{
				if (!seen.Add(c))
				{
					error = "No duplicate digits allowed.";
					return false;
				}
			}
		}

		error = null;
		return true;
	}
}