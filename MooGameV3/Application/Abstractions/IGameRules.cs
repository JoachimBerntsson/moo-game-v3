namespace MooGameV3.Application.Abstractions;

public interface IGameRules
{
	int CodeLength { get; }
	bool AllowDuplicates { get; }
	bool PracticeMode { get; }
}