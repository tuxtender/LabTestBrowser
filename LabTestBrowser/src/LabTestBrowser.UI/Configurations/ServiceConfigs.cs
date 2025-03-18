﻿using LabTestBrowser.Core.Interfaces;
using LabTestBrowser.Infrastructure;
using LabTestBrowser.Infrastructure.Email;

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

		logger.LogInformation("{Project} services registered", "Mediatr and Email Sender");

		return services;
	}
}