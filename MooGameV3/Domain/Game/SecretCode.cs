namespace MooGameV3.Domain.Game;
public sealed class SecretCode
{
	public const int Length = 4;
	public string Value { get; }

	private SecretCode(string value) => Value = value;

	public static SecretCode Create(string digits)
	{
		ArgumentNullException.ThrowIfNull(digits);
		if (digits.Length != Length) throw new ArgumentException($"Code must be {Length} digits.", nameof(digits));
		if (!digits.All(char.IsDigit)) throw new ArgumentException("Code must be digits only.", nameof(digits));
		if (digits.Distinct().Count() != Length) throw new ArgumentException("Digits must be unique.", nameof(digits));
		return new SecretCode(digits);
	}

	public override string ToString() => Value;
}
