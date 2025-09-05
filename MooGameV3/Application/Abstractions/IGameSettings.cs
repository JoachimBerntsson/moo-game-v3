namespace MooGameV3.Application.Abstractions;

public interface IGameSettings
{
	int DefaultCodeLength { get; }
	bool AllowDuplicates { get; }
	bool PracticeMode { get; }
	int? MaxAttempts { get; }
}