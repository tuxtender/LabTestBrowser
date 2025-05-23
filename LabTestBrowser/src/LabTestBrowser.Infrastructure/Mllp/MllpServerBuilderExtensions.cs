using Serilog;
using Serilog.Events;

namespace LabTestBrowser.Infrastructure.Mllp;

public static class MllpServerBuilderExtensions
{
	public static void ConfigureMllpLogging(this ILoggingBuilder builder)
	{
		builder.ClearProviders();

		builder.AddSerilog(new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.File("logs/mllp-server-errors-.log",
				rollingInterval: RollingInterval.Day,
				restrictedToMinimumLevel: LogEventLevel.Warning)
			.CreateLogger(), dispose: true);
	}
}