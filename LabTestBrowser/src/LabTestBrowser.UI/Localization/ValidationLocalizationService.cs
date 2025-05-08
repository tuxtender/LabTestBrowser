using LabTestBrowser.UseCases;
using Microsoft.Extensions.Localization;

namespace LabTestBrowser.UI.Localization;

public class ValidationLocalizationService(IStringLocalizer localizer) : IValidationLocalizationService
{
	public string GetString(string name, params object[] arguments) => localizer[name, arguments];
}