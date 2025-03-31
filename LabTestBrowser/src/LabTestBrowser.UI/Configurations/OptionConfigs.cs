using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Core.Services;
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
		// builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));
		services.Configure<MailserverConfiguration>(configuration.GetSection("Mailserver"));

		if (builder.Environment.IsDevelopment())
		{
		
		}
		
		services.AddSingleton<IV231OruR01Converter, V231OruR01Converter>();
		services.AddSingleton<IUrit5160Hl7Converter, Urit5160Hl7Converter>();
		services.AddSingleton<IHl7MessageHandler, Hl7MessageHandler>();

		logger.LogInformation("{Project} were configured", "Options");
		return services;
	}
}