namespace MooGameV3.Application.Abstractions;

public interface IGuessValidator
{
	bool TryValidate(string guess, int expectedLength, bool allowDuplicates, out string error);
}