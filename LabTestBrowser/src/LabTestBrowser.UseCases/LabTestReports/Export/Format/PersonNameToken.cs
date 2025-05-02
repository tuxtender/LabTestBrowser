namespace LabTestBrowser.UseCases.LabTestReports.Export.Format;

public class PersonNameToken(string name, string? personName) : IToken
{
	public string GetName() => name;
	public string GetValue() => personName?.Split().First() ?? string.Empty;
}