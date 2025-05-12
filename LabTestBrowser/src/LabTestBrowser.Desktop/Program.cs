using LabTestBrowser.Desktop;
using LabTestBrowser.Desktop.Configurations;
using LabTestBrowser.Desktop.Navigation;
using LabTestBrowser.Infrastructure;
using Serilog;
using Serilog.Extensions.Logging;

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
builder.Services.AddInfrastructureServices(builder.Configuration, appLogger);
builder.Services.AddMllpServerConfigs();
builder.Services.AddUseCaseConfigs();
builder.Services.AddMediatrConfigs();
builder.Services.AddPresentationConfigs();
builder.Services.AddLocalizationConfigs();

var app = builder.Build();
app.EnsureDatabaseCreated();

await app.RunAsync();

public partial class Program { }