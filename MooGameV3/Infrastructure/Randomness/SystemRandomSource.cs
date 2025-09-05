using MooGameV3.Application.Abstractions.Randomness;

namespace MooGameV3.Infrastructure.Randomness;

public sealed class SystemRandomSource(Random? rng = null) : IRandomSource
{
	private readonly Random _rng = rng ?? new Random();
	public int Next(int minInclusive, int maxExclusive) => _rng.Next(minInclusive, maxExclusive);
}