using LabTestBrowser.Infrastructure.Export.PathSanitizer;

namespace LabTestBrowser.UnitTests.Infrastructure.Export.PathSanitizer;

public class WindowsDirectoryNameSanitizerTests
{
	private readonly IDirectoryNameSanitizer _sanitizer = new WindowsPathSanitizer();

	[Theory]
	[InlineData("my:folder", "myfolder")]
	[InlineData("dir|name*test?", "dirname test")]
	[InlineData("   leading and trailing   ", "leading and trailing")]
	[InlineData("trailingDot.", "trailingDot")]
	[InlineData("multiple...dots..", "multiple...dots")]
	[InlineData("   .dir.   ", ".dir")]
	public void Sanitize_WithInvalidName_ReturnsSanitized(string input, string expected)
	{
		var result = _sanitizer.Sanitize(input);
		result.Should().Be(expected);
	}

	[Theory]
	[InlineData("CON", "_CON_")]
	[InlineData("AUX", "_AUX_")]
	[InlineData("PRN", "_PRN_")]
	[InlineData("NUL", "_NUL_")]
	[InlineData("COM3", "_COM3_")]
	[InlineData("LPT2", "_LPT2_")]
	public void Sanitize_WithReservedNames_ReturnsEscaped(string input, string expected)
	{
		var result = _sanitizer.Sanitize(input);
		result.Should().Be(expected);
	}

	[Theory]
	[InlineData("ValidFolder", "ValidFolder")]
	[InlineData("Reports_2025", "Reports_2025")]
	[InlineData("Nested-Folder123", "Nested-Folder123")]
	public void Sanitize_WithValidName_ReturnsUnchanged(string input, string expected)
	{
		var result = _sanitizer.Sanitize(input);
		result.Should().Be(expected);
	}

	[Fact]
	public void Sanitize_WhenInputIsNullOrWhitespace_ReturnEmpty()
	{
		_sanitizer.Sanitize(null!).Should().BeEmpty();
		_sanitizer.Sanitize("").Should().BeEmpty();
		_sanitizer.Sanitize("     ").Should().BeEmpty();
	}
}