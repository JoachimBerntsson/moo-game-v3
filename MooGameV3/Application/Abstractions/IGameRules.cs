namespace MooGameV3.Application.Abstractions;

public interface IGameRules
{
	int CodeLength { get; }
	bool AllowDuplicates { get; }
	bool PracticeMode { get; }
	int? MaxAttempts { get; }
}

public sealed class StandardGameRules : IGameRules
{
	public int CodeLength { get; init; } = 4;
	public bool AllowDuplicates { get; init; } = false;
	public bool PracticeMode { get; init; } = false;
	public int? MaxAttempts { get; init; } = null;
}