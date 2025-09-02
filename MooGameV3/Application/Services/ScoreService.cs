using MooGameV3.Application.Abstractions;
using MooGameV3.Domain.Players;

namespace MooGameV3.Application.Services;

public sealed class ScoreService
{
	private readonly IScoreRepository _repository;

	public ScoreService(IScoreRepository repository)
	{
		_repository = repository;
	}

	public void AddScore(string playerName, int guesses) => _repository.Append(playerName, guesses);

	public IReadOnlyList<PlayerStats> GetLeaderboard()
	{
		var map = new Dictionary<string, PlayerStats>(StringComparer.Ordinal);

		foreach (var (name, guesses) in _repository.ReadAll())
		{
			if (string.IsNullOrWhiteSpace(name)) continue;

			if (!map.TryGetValue(name, out var stats))
				map[name] = stats = new PlayerStats(name, guesses);
			else
				stats.Update(guesses);
		}

		return map.Values
			.OrderBy(p => p.Average)
			.ThenBy(p => p.Name, StringComparer.Ordinal)
			.ToList();
	}
}