namespace LabTestBrowser.UseCases;

public interface IValidationLocalizationService
{
	string GetString(string name, params object[] arguments);
}