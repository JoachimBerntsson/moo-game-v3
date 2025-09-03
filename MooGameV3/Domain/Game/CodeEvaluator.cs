using MooGameV3.Application.Abstractions;

namespace MooGameV3.Domain.Game;
public sealed class CodeEvaluator : ICodeEvaluator
{
	private readonly int _codeLength;

	public CodeEvaluator(IGameRules rules)
	{
		_codeLength = rules.CodeLength;
	}

	public BullsCows Evaluate(SecretCode goal, Guess guess)
	{
		var goalSpan = goal.Value.AsSpan();
		var guessRaw = (guess.Value ?? string.Empty).PadRight(_codeLength, ' ');
		var guessSpan = guessRaw.AsSpan();

		int bulls = 0;

		Span<int> goalDigitCount = stackalloc int[10];
		Span<int> guessDigitCount = stackalloc int[10];

		for (int i = 0; i < _codeLength; i++)
		{
			char goalDigit = goalSpan[i];
			char guessDigit = guessSpan[i];

			if (goalDigit == guessDigit)
			{
				bulls++;
			}
			else
			{
				if (goalDigit >= '0' && goalDigit <= '9')
					goalDigitCount[goalDigit - '0']++;

				if (guessDigit >= '0' && guessDigit <= '9')
					guessDigitCount[guessDigit - '0']++;
			}
		}

		int cows = 0;
		for (int i = 0; i < 10; i++)
		{
			cows += Math.Min(goalDigitCount[i], guessDigitCount[i]);
		}

		return new BullsCows(bulls, cows);
	}
}