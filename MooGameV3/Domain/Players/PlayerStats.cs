namespace MooGameV3.Domain.Players;
public sealed class PlayerStats(string name, int firstGameGuesses)
{
	public string Name { get; } = name ?? throw new ArgumentNullException(nameof(name));
	public int Games { get; private set; } = 1;
	public int TotalGuesses { get; private set; } = firstGameGuesses;

	public void Update(int guesses)
	{
		TotalGuesses += guesses;
		Games++;
	}

	public double Average => (double)TotalGuesses / Games;
}