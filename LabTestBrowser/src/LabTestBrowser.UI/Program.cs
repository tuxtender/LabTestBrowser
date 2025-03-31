using System.Text;
using LabTestBrowser.Infrastructure.Data;
using LabTestBrowser.Infrastructure.Hl7;
using Serilog;
using Serilog.Extensions.Logging;
using LabTestBrowser.UI;
using LabTestBrowser.UI.Configurations;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.LaboratoryEquipment.Hl7;
using MediatR;
using SuperSocket.ProtoBase;
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

// var t = new Hl7Hub(null);
// builder.Services.AddSingleton<IHl7Hub>(sp => t);
builder.Services.AddSingleton<LabReportViewModel>();
builder.Services.AddHostedService<RecoverDatabaseBackgroundService>();



// Build and run the application.
var app = builder.Build();

//TODO: Shared service collection
using var serviceScope = app.Services.CreateScope();
var services = serviceScope.ServiceProvider;
// var mediator = services.GetRequiredService<IMediator>();
var hl7Handler = services.GetRequiredService<IHl7MessageHandler>();

var mllpHostBuilder = SuperSocketHostBuilder.Create<MllpPackage, MllpPipelineFilter>()
	.UsePackageHandler(async (s, p) =>
	{
		var hl7Message = Encoding.UTF8.GetString(p.Content);
		var hl7AckMessage = await hl7Handler.HandleMessageAsync(hl7Message);
		await s.SendAsync(Encoding.UTF8.GetBytes(hl7AckMessage));
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


// await Task.WhenAny(app.RunAsync(), mllpHost.RunAsync());

await Task.WhenAny(mllpHost.RunAsync(), app.RunAsync());


// await app.RunAsync();

// await mllpHost.RunAsync();

public partial class Program { }