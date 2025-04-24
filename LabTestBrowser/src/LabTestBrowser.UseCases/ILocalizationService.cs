namespace LabTestBrowser.UseCases;

public interface ILocalizationService
{
	string GetString(string name, params object[] arguments);
}