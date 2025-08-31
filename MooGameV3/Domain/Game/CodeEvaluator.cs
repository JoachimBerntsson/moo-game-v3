namespace MooGameV3.Domain.Game;
public sealed class CodeEvaluator : ICodeEvaluator
{
	public BullsCows Evaluate(SecretCode goal, Guess guess)
	{
		var goalSpan = goal.Value.AsSpan();
		var guessRaw = (guess.Value ?? string.Empty).PadRight(SecretCode.Length, ' ');
		var guessSpan = guessRaw.AsSpan();

		int bulls = 0, cows = 0;

		for (int i = 0; i < SecretCode.Length; i++)
		{
			var g = goalSpan[i];
			for (int j = 0; j < SecretCode.Length; j++)
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