namespace MooGameV3.Domain.Game;
public interface ICodeEvaluator
{
	BullsCows Evaluate(SecretCode goal, Guess guess);
}