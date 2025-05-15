using LabTestBrowser.Desktop.Navigation;
using Serilog;
using Serilog.Settings.Configuration;

namespace LabTestBrowser.Desktop.Configurations;

public static class LoggerConfigs
{
	public static WpfApplicationBuilder<App, MainWindow> AddLoggerConfigs(this WpfApplicationBuilder<App, MainWindow> builder)
	{
		var configuration = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

		var readerOptions = new ConfigurationReaderOptions(typeof(FileLoggerConfigurationExtensions).Assembly);

		builder.Host.UseSerilog((context, services, loggerConfiguration) =>
			loggerConfiguration.ReadFrom.Configuration(configuration, readerOptions));

		return builder;
	}
}