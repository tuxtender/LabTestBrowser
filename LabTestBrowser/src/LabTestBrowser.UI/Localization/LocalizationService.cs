using System.Reflection;
using Ardalis.GuardClauses;
using LabTestBrowser.UseCases;
using Microsoft.Extensions.Localization;

namespace LabTestBrowser.UI.Localization;

public class LocalizationService : ILocalizationService
{
	private readonly IStringLocalizer _localizer;

	public LocalizationService(IStringLocalizerFactory factory)
	{
		//TODO: Refactor
		var assemblyFullName = typeof(Program).GetTypeInfo().Assembly.FullName;
		var assemblyName = new AssemblyName(assemblyFullName!);
		Guard.Against.Null(assemblyName);
		_localizer = factory.Create("Messages", assemblyName.Name!);
	}

	public string GetString(string name, params object[] arguments)
	{
		return _localizer[name, arguments];
	}
}