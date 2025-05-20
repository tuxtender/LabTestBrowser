namespace LabTestBrowser.Infrastructure.Export.PathSanitizer;

public class WindowsPathSanitizer : IPathSanitizer
{
	private readonly char[] _invalidChars = Path.GetInvalidFileNameChars();

	public string Sanitize(string pathComponent)
	{
		if (string.IsNullOrWhiteSpace(pathComponent))
			return string.Empty;

		var trimmed = pathComponent
			.TrimStart(' ')
			.TrimEnd(' ', '.');

		var sanitizedChars = trimmed.Split(_invalidChars, StringSplitOptions.RemoveEmptyEntries);
		var sanitized = string.Concat(sanitizedChars);

		if (IsReservedName(sanitized))
			sanitized = $"_{sanitized}_";

		return sanitized;
	}

	private static bool IsReservedName(string name)
	{
		var reserved = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"CON",
			"PRN",
			"AUX",
			"NUL",
			"COM1",
			"COM2",
			"COM3",
			"COM4",
			"COM5",
			"COM6",
			"COM7",
			"COM8",
			"COM9",
			"LPT1",
			"LPT2",
			"LPT3",
			"LPT4",
			"LPT5",
			"LPT6",
			"LPT7",
			"LPT8",
			"LPT9"
		};

		return reserved.Contains(name);
	}
}