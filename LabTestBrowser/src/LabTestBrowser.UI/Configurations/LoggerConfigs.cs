using Serilog;

namespace LabTestBrowser.UI.Configurations;

public static class LoggerConfigs
{
	public static WpfApplicationBuilder<App, Navigation.ShellWindow> AddLoggerConfigs(this WpfApplicationBuilder<App, Navigation.ShellWindow> builder)
	{
		builder.Host.UseSerilog((_, config) => config.ReadFrom.Configuration(builder.Configuration));

		return builder;
	}
}