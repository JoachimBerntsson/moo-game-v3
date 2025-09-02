namespace MooGameV3.Application.Abstractions.Randomness
{
	public interface IRandomSource
	{
		int Next(int minInclusive, int maxExclusive);
	}
}
