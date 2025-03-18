using Serilog;
using Serilog.Extensions.Logging;
using LabTestBrowser.UI;
using LabTestBrowser.UI.Configurations;

// Create a builder by specifying the application and main window.
var builder = WpfApplication<App, MainWindow>.CreateBuilder(args);

var logger = Log.Logger = new LoggerConfiguration()
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

logger.Information("Starting application");

builder.AddLoggerConfigs();

var appLogger = new SerilogLoggerFactory(logger)
	.CreateLogger<Program>();

builder.Services.AddOptionConfigs(builder.Configuration, appLogger, builder);
builder.Services.AddServiceConfigs(appLogger, builder);

// Build and run the application.
var app = builder.Build();
await app.RunAsync();

public partial class Program { }