using LabTestBrowser.Desktop.Localization;
using LabTestBrowser.Infrastructure.Export;
using LabTestBrowser.Infrastructure.Templating.Tokens.Formatters;
using LabTestBrowser.UseCases;
using Microsoft.Extensions.Localization;

namespace LabTestBrowser.Desktop.Configurations;

public static class LocalizationConfigs
{
	public static IServiceCollection AddLocalizationConfigs(this IServiceCollection services)
	{
		services.AddLocalization(options => options.ResourcesPath = "Resources");
		
		services.AddSingleton<IDefaultPathProvider, ResxDefaultPathProvider>();
		services.AddSingleton<IErrorLocalizationService, ErrorLocalizationService>();
		services.AddSingleton<IValidationLocalizationService>(provider =>
		{
			var factory = provider.GetRequiredService<IStringLocalizerFactory>();
			var stringLocalizer = factory.Create("ValidationErrors", typeof(Program).Assembly.GetName().Name!);

			return new ValidationLocalizationService(stringLocalizer);
		});
		services.AddSingleton<IAgeLocalizationService, AgeLocalizationService>();

		return services;
	}
}