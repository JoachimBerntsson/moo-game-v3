namespace MooGameV3.Domain.Game;
public sealed class SecretCode
{
	public string Value { get; }

	private SecretCode(string value) => Value = value;

	public static SecretCode Create(string digits, int length)
	{
		ArgumentNullException.ThrowIfNull(digits);
		if (digits.Length != length) throw new ArgumentException($"Code must be {length} digits.", nameof(digits));
		if (!digits.All(char.IsDigit)) throw new ArgumentException("Code must be digits only.", nameof(digits));
		if (digits.Distinct().Count() != length) throw new ArgumentException("Digits must be unique.", nameof(digits));
		return new SecretCode(digits);
	}

	public override string ToString() => Value;
}
