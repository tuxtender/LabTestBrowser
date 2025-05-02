namespace LabTestBrowser.UseCases.Export.Format;

public class NumberToken(string name, int? age) : IToken
{
	public string GetName() => name;
	public string GetValue() => age.HasValue ? age.Value.ToString() : string.Empty;
}