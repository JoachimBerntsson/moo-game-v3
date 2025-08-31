using System.Globalization;
using MooGameV3.Application.Abstractions;

namespace MooGameV3.Infrastructure.FileSystem;

public sealed class FileScoreRepository(string path) : IScoreRepository
{
	private readonly string path = path;
	private const string Separator = "#&#";

	public void Append(string playerName, int guesses)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path)) ?? ".");
		using var output = new StreamWriter(path, append: true);
		output.WriteLine($"{playerName}{Separator}{guesses.ToString(CultureInfo.InvariantCulture)}");
	}

	public IEnumerable<(string Name, int Guesses)> ReadAll()
	{
		if (!File.Exists(path)) yield break;

		using var input = new StreamReader(path);
		string? line;
		while ((line = input.ReadLine()) is not null)
		{
			var parts = line.Split(Separator, StringSplitOptions.None);
			if (parts.Length != 2) continue;
			if (!int.TryParse(parts[1], NumberStyles.Integer, CultureInfo.InvariantCulture, out var guesses)) continue;
			yield return (parts[0], guesses);
		}
	}
}