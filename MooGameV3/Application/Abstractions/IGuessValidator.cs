using System.Diagnostics.CodeAnalysis;

namespace MooGameV3.Application.Abstractions;

public interface IGuessValidator
{
	bool TryValidate(string guess, int requiredLength, bool allowDuplicates, [NotNullWhen(false)] out string? error);
}