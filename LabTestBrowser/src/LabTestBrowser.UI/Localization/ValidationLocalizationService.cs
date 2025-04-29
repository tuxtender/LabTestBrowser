using System.Reflection;
using LabTestBrowser.UseCases;
using Microsoft.Extensions.Localization;

namespace LabTestBrowser.UI.Localization;

public class ValidationLocalizationService : IValidationLocalizationService
{
	private readonly IStringLocalizer _localizer;

	public ValidationLocalizationService(IStringLocalizerFactory factory)
	{
		//TODO: Refactor
		var assemblyFullName = typeof(ValidationLocalizationService).GetTypeInfo().Assembly.FullName;
		var assemblyName = new AssemblyName(assemblyFullName!);
		_localizer = factory.Create("ValidationErrors", assemblyName.Name!);
	}

	public string GetString(string name, params object[] arguments)
	{
		return _localizer[name, arguments];
	}
}