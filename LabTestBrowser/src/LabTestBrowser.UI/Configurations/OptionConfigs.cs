using LabTestBrowser.Infrastructure.Email;

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

		logger.LogInformation("{Project} were configured", "Options");

		return services;
	}
}