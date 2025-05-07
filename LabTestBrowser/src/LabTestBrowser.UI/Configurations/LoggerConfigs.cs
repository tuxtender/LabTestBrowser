using LabTestBrowser.UI.Navigation;
using Serilog;

namespace LabTestBrowser.UI.Configurations;

public static class LoggerConfigs
{
	public static WpfApplicationBuilder<App, MainWindow> AddLoggerConfigs(this WpfApplicationBuilder<App, MainWindow> builder)
	{
		builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

		return builder;
	}
}