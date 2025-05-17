using System.Text.RegularExpressions;

namespace LabTestBrowser.Infrastructure.Templating.Engines;

public class TextTemplateEngine(ILogger<TextTemplateEngine> _logger) : ITextTemplateEngine
{
	private const string TokenPattern = @"{{\s*([a-zA-Z][a-zA-Z0-9_.\-%]*)\s*}}";

	public string Render(string template, IReadOnlyDictionary<string, string> tokens)
	{
		return Regex.Replace(template, TokenPattern, match =>
		{
			var token = match.Groups[1].Value;

			if (tokens.TryGetValue(token, out var value))
			{
				_logger.LogDebug("Replaced token: {token}, value : {value}", token, value);
				return value;
			}

			_logger.LogDebug("Skipped unregistered token: {token}", token);

			return string.Empty;
		});
	}
}