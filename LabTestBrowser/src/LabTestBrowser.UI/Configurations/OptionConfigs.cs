using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Core.Services;
using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.Infrastructure.Email;
using LabTestBrowser.Infrastructure.Hl7;
using LabTestBrowser.Infrastructure.LaboratoryEquipment;
using LabTestBrowser.UseCases.LaboratoryEquipment;

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

		if (builder.Environment.IsDevelopment())
		{
		
		}
		
		logger.LogInformation("{Project} were configured", "Options");
		return services;
	}
}