using MooGameV3.Domain.Players;

namespace MooGameV3.Application.Abstractions;

public interface IScoreRepository
{
	void Append(string playerName, int guesses);

	IEnumerable<(string Name, int Guesses)> ReadAll();
}