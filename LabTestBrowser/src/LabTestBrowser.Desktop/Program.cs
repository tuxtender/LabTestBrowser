using LabTestBrowser.Desktop;
using LabTestBrowser.Desktop.Configurations;
using LabTestBrowser.Desktop.Mllp;
using LabTestBrowser.Desktop.Navigation;
using LabTestBrowser.Infrastructure.Data;
using Serilog;
using Serilog.Extensions.Logging;
using LabTestBrowser.UseCases.Hl7.ProcessHl7Request;
using MediatR;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;

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
builder.Services.AddPresentationConfigs();
builder.Services.AddLocalizationConfigs();

// Build and run the application.
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;
	var context = services.GetRequiredService<AppDbContext>();
	context.Database.EnsureCreated();
}

var mllpHostBuilder = SuperSocketHostBuilder.Create<MllpPackage, MllpPipelineFilter>()
	.UsePackageHandler(async (s, p) =>
	{
		//TODO: Refactor to BackgroundService
		using var serviceScope = app.Services.CreateScope();
		var services = serviceScope.ServiceProvider;
		var mediator = services.GetRequiredService<IMediator>();
		var response = await mediator.Send(new ProcessHl7RequestCommand(p.Content));
		await s.SendAsync(response);
	})
	.ConfigureSuperSocket(options =>
	{
		options.Name = "Echo Server";
		options.Listeners =
		[
			new ListenOptions
			{
				Ip = "Any",
				Port = 4040
			}
		];
	})
	.UseSessionHandler((s) =>
		{
			logger.Information($"Session {s.SessionID} opened from {s.RemoteEndPoint}");
			return ValueTask.CompletedTask;

			// things to do when the session just connects
		},
		(s, e) =>
		{
			// s: the session
			// e: the CloseEventArgs
			// e.Reason: the close reason
			// things to do after the session closes
			logger.Information($"Session {s.SessionID} closed ");
			return ValueTask.CompletedTask;
		})
	.ConfigureLogging((hostCtx, loggingBuilder) => { loggingBuilder.AddConsole(); });

var mllpHost = mllpHostBuilder.Build();

await Task.WhenAny(mllpHost.RunAsync(), app.RunAsync());

// await app.RunAsync();

// await mllpHost.RunAsync();

public partial class Program { }