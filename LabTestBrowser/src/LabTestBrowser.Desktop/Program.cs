using LabTestBrowser.Desktop;
using LabTestBrowser.Desktop.Configurations;
using LabTestBrowser.Desktop.Navigation;
using LabTestBrowser.Infrastructure;
using LabTestBrowser.Infrastructure.Data;
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
builder.Services.AddUseCaseConfigs();
builder.Services.AddMediatrConfigs();
builder.Services.AddPresentationConfigs();
builder.Services.AddLocalizationConfigs();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<AppDbContext>();
	context.Database.EnsureCreated();
}

await app.RunAsync();

public partial class Program { }