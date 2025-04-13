using System.Text.RegularExpressions;

namespace LabTestBrowser.Infrastructure.Export;

public class TextTemplateEngine(ILogger<TextTemplateEngine> _logger) : ITextTemplateEngine
{
	private const string TokenPattern = @"{{\s*([a-zA-Z][a-zA-Z0-9_.\-%]*)\s*}}";

	public string Render(string template, Dictionary<string, string> tokens)
	{
		return Regex.Replace(template, TokenPattern, match =>
		{
			var token = match.Value[2..^2];

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