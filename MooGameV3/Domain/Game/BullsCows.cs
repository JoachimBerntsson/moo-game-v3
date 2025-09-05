namespace MooGameV3.Domain.Game;

public readonly record struct BullsCows(int Bulls, int Cows)
{
	public bool IsWin(int codeLength) => Bulls == codeLength;
}