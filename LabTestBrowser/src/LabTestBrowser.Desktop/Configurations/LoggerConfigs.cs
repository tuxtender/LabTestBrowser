using LabTestBrowser.Desktop.Navigation;
using Serilog;
using Serilog.Settings.Configuration;

namespace LabTestBrowser.Desktop.Configurations;

public static class LoggerConfigs
{
	public static WpfApplicationBuilder<App, MainWindow> AddLoggerConfigs(this WpfApplicationBuilder<App, MainWindow> builder)
	{
		// builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

		var readerOptions = new ConfigurationReaderOptions(
			typeof(ConsoleLoggerConfigurationExtensions).Assembly,
			typeof(FileLoggerConfigurationExtensions).Assembly
		);

		builder.Host.UseSerilog((context, services, loggerConfiguration) =>
		{
			loggerConfiguration
				.ReadFrom.Configuration(configuration, readerOptions);
		});
		
		// builder.Host.UseSerilog((_, config) =>
		// 	new LoggerConfiguration()
		// 		.ReadFrom.Configuration(configuration, readerOptions)
		// 		.CreateLogger()
		// );

		
		// builder.Host.UseSerilog((hostingContext, services, loggerConfiguration) => loggerConfiguration
		// 	.ReadFrom.Configuration(hostingContext.Configuration)
		// 	.Enrich.FromLogContext()
		// 	.WriteTo.File(
		// 		@"Logs\log.txt", 
		// 		rollingInterval: RollingInterval.Day));
		
		return builder;
	}
}