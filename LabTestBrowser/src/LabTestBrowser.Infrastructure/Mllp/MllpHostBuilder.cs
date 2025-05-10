using LabTestBrowser.UseCases.Hl7.ProcessHl7Request;
using MediatR;
using Microsoft.Extensions.Hosting;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;

namespace LabTestBrowser.Infrastructure.Mllp;

public class MllpHostBuilder : IMllpHostBuilder
{
	private readonly IMediator _mediator;
	private readonly ILogger<MllpHostedService> _logger;
	private readonly MllpOptions _settings;

	public MllpHostBuilder(IMediator mediator, IOptions<MllpOptions> settings, ILogger<MllpHostedService> logger)
	{
		_mediator = mediator;
		_logger = logger;
		_settings = settings.Value;
	}

	public IHost Build()
	{
		return SuperSocketHostBuilder.Create<MllpPackage, MllpPipelineFilter>()
			.UsePackageHandler(async (s, p) =>
			{
				var response = await _mediator.Send(new ProcessHl7RequestCommand(p.Content));
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
						Port = _settings.Port
					}
				];
			})
			.ConfigureErrorHandler((app, error) =>
			{
				_logger.LogError(error, "MLLP service failed");
				return new ValueTask<bool>();
			})
			.UseSessionHandler((session) =>
				{
					_logger.LogInformation("Session {ArgSessionId} opened from {ArgRemoteEndPoint}", session.SessionID,
						session.RemoteEndPoint);
					return ValueTask.CompletedTask;
				},
				(session, args) =>
				{
					_logger.LogInformation("Session {SessionSessionId} closed because of {Reason} ", session.SessionID, args.Reason);
					return ValueTask.CompletedTask;
				})
			.ConfigureLogging((hostCtx, loggingBuilder) => { loggingBuilder.AddConsole(); })
			.Build();
	}
}