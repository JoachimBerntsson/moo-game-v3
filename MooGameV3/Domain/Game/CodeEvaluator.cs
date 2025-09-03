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

		int bulls = 0, cows = 0;

		for (int i = 0; i < _codeLength; i++)
		{
			var g = goalSpan[i];
			for (int j = 0; j < _codeLength; j++)
			{
				if (g == guessSpan[j])
				{
					if (i == j) bulls++;
					else cows++;
				}
			}
		}
		return new BullsCows(bulls, cows);
	}
}