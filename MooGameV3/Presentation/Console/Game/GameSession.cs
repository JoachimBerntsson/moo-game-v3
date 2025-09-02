using System;
using MooGameV3.Domain.Game;

namespace MooGameV3.Presentation.Console.Game;

public sealed class GameSession
{
	public string PlayerName { get; }
	public SecretCode Secret { get; }

	public string SecretString => Secret.ToString();
	public int Attempts { get; private set; }
	public int Penalty { get; private set; }
	public int ExpectedLength { get; }

	public int TotalScore => Attempts + Penalty;

	public GameSession(SecretCode secret, int expectedLength, string playerName)
	{
		if (expectedLength <= 0)
			throw new ArgumentOutOfRangeException(nameof(expectedLength), "Expected length must be > 0.");

		if (string.IsNullOrWhiteSpace(playerName))
			throw new ArgumentException("Player name is required.", nameof(playerName));

		Secret = secret;
		ExpectedLength = expectedLength;
		PlayerName = playerName;
	}

	public void IncrementAttempts() => Attempts++;

	public void AddPenalty(int value)
	{
		if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Penalty cannot be negative.");
		Penalty += value;
	}
}