using Microsoft.VisualStudio.TestTools.UnitTesting;
using MooGameV3.Application.Abstractions;
using MooGameV3.Domain.Game;

namespace MooGameV3.UnitTests;

[TestClass]
public class CodeEvaluatorTests
{
	private readonly ICodeEvaluator _sut = new CodeEvaluator(new TestGameRules());

	private static SecretCode Code(string digits) => SecretCode.Create(digits, 4);
	private static Guess CreateGuess(string input) => new Guess(input);

	private sealed class TestGameRules : IGameRules
	{
		public int CodeLength => 4;
		public bool AllowDuplicates => false;
		public bool PracticeMode => false;
		public int? MaxAttempts => null;
	}
	[TestMethod]
	public void Evaluate_AllCorrect_ShouldBe4Bulls0Cows()
	{
		var result = _sut.Evaluate(Code("1234"), CreateGuess("1234"));
		Assert.AreEqual(4, result.Bulls);
		Assert.AreEqual(0, result.Cows);
	}

	[TestMethod]
	public void Evaluate_Mixed_ShouldBe2Bulls2Cows()
	{
		var result = _sut.Evaluate(Code("1234"), CreateGuess("1243"));
		Assert.AreEqual(2, result.Bulls);
		Assert.AreEqual(2, result.Cows);
	}

	[TestMethod]
	public void Evaluate_None_ShouldBe0Bulls0Cows()
	{
		var result = _sut.Evaluate(Code("1234"), CreateGuess("5678"));
		Assert.AreEqual(0, result.Bulls);
		Assert.AreEqual(0, result.Cows);
	}

	[TestMethod]
	public void Evaluate_AllPresentWrongPlace_ShouldBe0Bulls4Cows()
	{
		var result = _sut.Evaluate(Code("1234"), CreateGuess("4321"));
		Assert.AreEqual(0, result.Bulls);
		Assert.AreEqual(4, result.Cows);
	}
}