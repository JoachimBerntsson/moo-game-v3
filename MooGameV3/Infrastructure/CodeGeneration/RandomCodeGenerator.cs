using MooGameV3.Application.Abstractions;
using MooGameV3.Application.Abstractions.Randomness;
using MooGameV3.Domain.Game;

namespace MooGameV3.Infrastructure.CodeGeneration;

public sealed class RandomCodeGenerator(IGameRules rules, IRandomSource rng) : ICodeGenerator
{
	private readonly IGameRules _rules = rules ?? throw new ArgumentNullException(nameof(rules));
	private readonly IRandomSource _rng = rng ?? throw new ArgumentNullException(nameof(rng));

	public SecretCode Generate()
	{
		int len = _rules.CodeLength;
		bool allowDup = _rules.AllowDuplicates;

		if (!allowDup && len > 10)
			throw new InvalidOperationException($"Cannot generate {len} unique digits (max 10).");

		Span<char> buf = len <= 64 ? stackalloc char[len] : new char[len];

		if (allowDup)
		{
			for (int i = 0; i < len; i++)
				buf[i] = (char)('0' + _rng.Next(0, 10));
		}
		else
		{
			Span<bool> used = stackalloc bool[10]; // index 0–9
			int i = 0;
			while (i < len)
			{
				int d = _rng.Next(0, 10);
				if (used[d]) continue;
				used[d] = true;
				buf[i++] = (char)('0' + d);
			}
		}

		return SecretCode.Create(new string(buf));
	}
}