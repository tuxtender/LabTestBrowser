using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Infrastructure;
using LabTestBrowser.Infrastructure.Email;
using LabTestBrowser.Infrastructure.Export;
using LabTestBrowser.Infrastructure.Hl7;
using LabTestBrowser.UI.Localization;
using LabTestBrowser.UseCases;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment.Urit5160;

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
		services.AddSingleton<IHl7AcknowledgmentService, Hl7AcknowledgmentService>();
		services.AddSingleton<ICompleteBloodCountUpdateChannel, CompleteBloodCountUpdateChannel>();
		services.AddSingleton<ILocalizationService, LocalizationService>();
		services.AddSingleton<IErrorLocalizationService, ErrorLocalizationService>();
		services.AddSingleton<IValidationLocalizationService, ValidationLocalizationService>();
		services.AddSingleton<IDefaultPathProvider, ResxDefaultPathProvider>();

		logger.LogInformation("{Project} services registered", "Mediatr and Email Sender");

		return services;
	}
}