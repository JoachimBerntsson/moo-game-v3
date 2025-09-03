namespace MooGameV3.Infrastructure.FileSystem;

public sealed class FileScoreRepositoryOptions
{
	public string Path { get; init; } = "scores.txt";

	public string Separator { get; init; } = "#&#";
}