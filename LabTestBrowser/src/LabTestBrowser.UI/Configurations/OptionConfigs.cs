using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Core.Services;
using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.Infrastructure.Email;
using LabTestBrowser.Infrastructure.Export;
using LabTestBrowser.Infrastructure.Hl7;

namespace LabTestBrowser.UI.Configurations;

public static class OptionConfigs
{
	public static IServiceCollection AddOptionConfigs(this IServiceCollection services,
		IConfiguration configuration,
		ILogger logger,
		WpfApplicationBuilder<App, MainWindow> builder)
	{
		builder.Configuration.AddJsonFile("labreportsettings.json");
		builder.Configuration.AddJsonFile("animalsettings.json");

		// builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
		services.Configure<MailserverConfiguration>(configuration.GetSection("Mailserver"));
		builder.Services.Configure<ExportSettings>(builder.Configuration.GetSection(nameof(ExportSettings)));

		if (builder.Environment.IsDevelopment())
		{
		
		}
		
		logger.LogInformation("{Project} were configured", "Options");
		return services;
	}
}