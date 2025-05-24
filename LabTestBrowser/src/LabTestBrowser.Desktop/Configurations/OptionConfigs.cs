using Ardalis.GuardClauses;
using LabTestBrowser.Desktop.Navigation;
using LabTestBrowser.Infrastructure.Export;
using LabTestBrowser.Infrastructure.Mllp;

namespace LabTestBrowser.Desktop.Configurations;

public static class OptionConfigs
{
	public static IServiceCollection AddOptionConfigs(this IServiceCollection services,
		IConfiguration configuration,
		ILogger logger,
		WpfApplicationBuilder<App, MainWindow> builder)
	{
		builder.Configuration.AddJsonFile("labreportsettings.json");
		builder.Configuration.AddJsonFile("animalsettings.json");

		var exportSettingsSection = configuration.GetSection(ExportOptions.SectionName);
		var exportSettings = exportSettingsSection.Get<ExportOptions>();
		Guard.Against.Null(exportSettings);
		services.Configure<ExportOptions>(exportSettingsSection);

		var mllpSettingsSection = configuration.GetSection(MllpOptions.SectionName);
		var mllpSettings = mllpSettingsSection.Get<MllpOptions>();
		Guard.Against.Null(mllpSettings);
		services.Configure<MllpOptions>(mllpSettingsSection);

		logger.LogInformation("{Project} were configured", "Options");
		return services;
	}
}