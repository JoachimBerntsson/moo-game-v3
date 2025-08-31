using MooGameV3.Domain.Game;

namespace MooGameV3.Infrastructure.Random;

public sealed class RandomCodeGenerator : ICodeGenerator
{
	private readonly System.Random rng = new();

	public SecretCode Generate()
	{
		Span<char> digits = stackalloc char[SecretCode.Length];
		int idx = 0;
		while (idx < SecretCode.Length)
		{
			char d = (char)('0' + rng.Next(0, 10));
			bool exists = false;
			for (int i = 0; i < idx; i++)
				if (digits[i] == d) { exists = true; break; }
			if (!exists) digits[idx++] = d;
		}
		return SecretCode.Create(new string(digits));
	}
}