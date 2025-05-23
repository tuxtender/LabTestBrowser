using Ardalis.GuardClauses;
using LabTestBrowser.Desktop.Navigation;
using LabTestBrowser.Infrastructure.Mllp;

namespace LabTestBrowser.Desktop.Configurations;

public static class OptionConfigs
{
	public static IServiceCollection AddOptionConfigs(this IServiceCollection services,
		IConfiguration configuration,
		ILogger logger,
		WpfApplicationBuilder<App, MainWindow> builder)
	{
		var mllpSettingsSection = configuration.GetSection(MllpOptions.SectionName);
		var mllpSettings = mllpSettingsSection.Get<MllpOptions>();
		Guard.Against.Null(mllpSettings);
		services.Configure<MllpOptions>(mllpSettingsSection);

		logger.LogInformation("{Project} were configured", "Options");
		return services;
	}
}