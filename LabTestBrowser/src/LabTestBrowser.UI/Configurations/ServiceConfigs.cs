using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Infrastructure;
using LabTestBrowser.Infrastructure.Email;
using LabTestBrowser.Infrastructure.Hl7;
using LabTestBrowser.Infrastructure.LaboratoryEquipment;
using LabTestBrowser.UI.Localization;
using LabTestBrowser.UseCases;
using LabTestBrowser.UseCases.LaboratoryEquipment;

namespace LabTestBrowser.UI.Configurations;

public static class ServiceConfigs
{
	public static IServiceCollection AddServiceConfigs(this IServiceCollection services, 
		ILogger logger,
		WpfApplicationBuilder<App, MainWindow> builder)
	{
		services.AddInfrastructureServices(builder.Configuration, logger)
			.AddMediatrConfigs();

		if (builder.Environment.IsDevelopment())
		{
			// Use a local test email server
			// See: https://ardalis.com/configuring-a-local-test-email-server/
			services.AddScoped<IEmailSender, MimeKitEmailSender>();

			// Otherwise use this:
			//builder.Services.AddScoped<IEmailSender, FakeEmailSender>();
		}
		else
		{
			services.AddScoped<IEmailSender, MimeKitEmailSender>();
		}
		
		services.AddSingleton<IV231OruR01Converter, V231OruR01Converter>();
		services.AddSingleton<IUrit5160Hl7Converter, Urit5160Hl7Converter>();
		services.AddSingleton<IHl7MessageHandler, Hl7MessageHandler>();
		services.AddSingleton<ICompleteBloodCountUpdateChannel, CompleteBloodCountUpdateChannel>();
		services.AddSingleton<ILocalizationService, LocalizationService>();

		logger.LogInformation("{Project} services registered", "Mediatr and Email Sender");

		return services;
	}
}