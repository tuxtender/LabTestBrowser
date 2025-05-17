using LabTestBrowser.Infrastructure.Templating.Engines;

namespace LabTestBrowser.UnitTests.Infrastructure.Templating.Engines;

public class TextTemplateEngineTests
{
	private readonly ITextTemplateEngine _engine;

	public TextTemplateEngineTests()
	{
		var logger = new LoggerFactory().CreateLogger<TextTemplateEngine>();
		_engine = new TextTemplateEngine(logger);
	}

	[Theory]
	[InlineData("Hello, {{name}}!", "Hello, John!", "name", "John")]
	[InlineData("Order ID: {{ order.id }}", "Order ID: 12345", "order.id", "12345")]
	[InlineData("Welcome {{ user-name }}", "Welcome Alice", "user-name", "Alice")]
	[InlineData("Underscore and period: {{a.B_c}}", "Underscore and period: dummy", "a.B_c", "dummy")]
	[InlineData("Repeated: {{x}}, {{x}}", "Repeated: 42, 42", "x", "42")]
	[InlineData("Missing {{token}}", "Missing ", "otherToken", "otherTokenValue")]
	[InlineData("Partial {{match", "Partial {{match", "", "")]
	public void Render_WithTemplate_MatchesExpected(string template, string expected, string key, string value)
	{
		var tokens = new Dictionary<string, string>
		{
			[key] = value
		};

		var result = _engine.Render(template, tokens);

		result.Should().Be(expected);
	}

	[Theory]
	[MemberData(nameof(TemplateTestCases.RenderCases), MemberType = typeof(TemplateTestCases))]
	public void Render_WhenTemplateContainsMultipleToken_MatchesExpected(TemplateTestCase testCase)
	{
		var tokens = testCase.Keys
			.Zip(testCase.Values, (key, value) => new
			{
				Key = key,
				Value = value
			})
			.ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);

		var result = _engine.Render(testCase.Template, tokens);

		result.Should().Be(testCase.Expected);
	}
}