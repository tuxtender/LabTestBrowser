namespace LabTestBrowser.UseCases.Export.Format;

public class DateToken(string name, DateOnly value, string format = "dd.MM.yyyy") : IToken
{
	public string GetName() => name;
	public string GetValue() => value.ToString(format);
}