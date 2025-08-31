using MooGameV3.Application.Abstractions;
using MooGameV3.Domain.Players;

namespace MooGameV3.Application.Services;

public sealed class ScoreService(IScoreRepository repository)
{
	private readonly IScoreRepository repository = repository;

	public void AddScore(string playerName, int guesses) => repository.Append(playerName, guesses);

	public IReadOnlyList<PlayerStats> GetLeaderboard()
	{
		var map = new Dictionary<string, PlayerStats>(StringComparer.Ordinal);
		foreach (var (name, guesses) in repository.ReadAll())
		{
			if (string.IsNullOrWhiteSpace(name)) continue;
			if (!map.TryGetValue(name, out var stats))
			{
				stats = new PlayerStats(name, guesses);
				map[name] = stats;
			}
			else
			{
				stats.Update(guesses);
			}
		}

		return map.Values
			.OrderBy(p => p.Average)
			.ThenBy(p => p.Name, StringComparer.Ordinal)
			.ToList();
	}
}