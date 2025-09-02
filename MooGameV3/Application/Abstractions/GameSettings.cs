namespace MooGameV3.Application.Abstractions;

public sealed record GameSettings(
	int DefaultCodeLength = 4,
	bool AllowDuplicates = false,
	bool PracticeMode = false,
	int? MaxAttempts = null
) : IGameRules
{
	int IGameRules.CodeLength => DefaultCodeLength;
	bool IGameRules.AllowDuplicates => AllowDuplicates;
	bool IGameRules.PracticeMode => PracticeMode;
	int? IGameRules.MaxAttempts => MaxAttempts;
}