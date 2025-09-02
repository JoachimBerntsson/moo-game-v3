using System.Globalization;
using MooGameV3.Application.Abstractions;

namespace MooGameV3.Infrastructure.FileSystem;

public sealed class FileScoreRepository(string path) : IScoreRepository
{
	private static readonly Lock _lock = new();
	private readonly string _path = Path.GetFullPath(path);
	private const string Separator = "#&#";

	public void Append(string playerName, int guesses)
	{
		var name = (playerName ?? string.Empty).Trim();
		if (name.Length == 0) return;
		if (guesses <= 0) return;

		name = name.Replace('\r', ' ')
				   .Replace('\n', ' ')
				   .Replace(Separator, "-");

		EnsureDirectory();

		var line = $"{name}{Separator}{guesses.ToString(CultureInfo.InvariantCulture)}{Environment.NewLine}";
		lock (_lock)
		{
			File.AppendAllText(_path, line);
		}
	}

	public IEnumerable<(string Name, int Guesses)> ReadAll()
	{
		if (!File.Exists(_path)) yield break;

		string[] lines;
		lock (_lock)
		{
			lines = File.ReadAllLines(_path);
		}

		foreach (var raw in lines)
		{
			if (TryParseLine(raw, out var name, out var guesses))
				yield return (name, guesses);
		}
	}

	private static bool TryParseLine(string? line, out string name, out int guesses)
	{
		name = string.Empty;
		guesses = 0;
		if (string.IsNullOrWhiteSpace(line)) return false;

		var s = line.Trim();

		var parts = s.Split(Separator, StringSplitOptions.None);
		if (parts.Length == 2 && Try(parts[0], parts[1], out name, out guesses))
			return true;

		var idx = s.LastIndexOf(';');
		if (idx < 0) idx = s.LastIndexOf(',');
		if (idx < 0) idx = s.LastIndexOf(' ');

		if (idx > 0)
		{
			var left = s[..idx].Trim();
			var right = s[(idx + 1)..].Trim();
			if (Try(left, right, out name, out guesses)) return true;
		}

		return false;

		static bool Try(string left, string right, out string n, out int g)
		{
			n = left.Trim();
			if (n.Length == 0) { g = 0; return false; }
			return int.TryParse(right, NumberStyles.Integer, CultureInfo.InvariantCulture, out g);
		}
	}

	private void EnsureDirectory()
	{
		var dir = Path.GetDirectoryName(_path);
		if (!string.IsNullOrEmpty(dir))
			Directory.CreateDirectory(dir);
	}
}