namespace LabTestBrowser.UseCases.Export.Format;

public class TextToken(string name, string? value) : IToken
{
	public string GetName() => name;
	public string GetValue() => value ?? string.Empty;
}