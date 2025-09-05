using MooGameV3.Application.Abstractions;
using MooGameV3.Domain.Players;

namespace MooGameV3.Application.Services;

public sealed class ScoreService(IScoreRepository repository)
{
	private readonly IScoreRepository _repository = repository ?? throw new ArgumentNullException(nameof(repository));

	public void AddScore(string playerName, int guesses)
	{
		if (string.IsNullOrWhiteSpace(playerName))
			throw new ArgumentException("Player name is required.", nameof(playerName));

		if (guesses <= 0)
			throw new ArgumentOutOfRangeException(nameof(guesses), "Number of guesses must be positive.");

		var name = playerName.Trim();
		_repository.Append(name, guesses);
	}

	public IReadOnlyList<PlayerStats> GetLeaderboard()
	{
		var map = new Dictionary<string, PlayerStats>(StringComparer.OrdinalIgnoreCase);

		foreach (var (name, guesses) in _repository.ReadAll())
		{
			if (string.IsNullOrWhiteSpace(name)) continue;

			var key = name.Trim();
			if (key.Length == 0) continue;

			if (!map.TryGetValue(key, out var stats))
				map[key] = stats = new PlayerStats(key, guesses);
			else
				stats.Update(guesses);
		}

		return [.. map.Values
			.OrderBy(p => p.Average)
			.ThenBy(p => p.Name, StringComparer.OrdinalIgnoreCase)];
	}
}