using LabTestBrowser.UseCases.Hl7.ProcessHl7Request;
using MediatR;
using Microsoft.Extensions.Hosting;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;

namespace LabTestBrowser.Infrastructure.Mllp;

public class MllpServerFactory : IMllpServerFactory
{
	private readonly IMediator _mediator;
	private readonly ILogger<MllpHostedService> _logger;
	private readonly MllpOptions _settings;

	public MllpServerFactory(IMediator mediator, IOptions<MllpOptions> settings, ILogger<MllpHostedService> logger)
	{
		_mediator = mediator;
		_logger = logger;
		_settings = settings.Value;
	}

	public IHost Create()
	{
		return SuperSocketHostBuilder.Create<MllpPackage, MllpPipelineFilter>()
			.UsePackageHandler(async (s, p) =>
			{
				var response = await _mediator.Send(new ProcessHl7RequestCommand(p.Content));
				await s.SendAsync(response);
			})
			.ConfigureSuperSocket(options =>
			{
				options.Name = "MLLP Server";
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
				_logger.LogError(error, "MLLP server error");
				return new ValueTask<bool>();
			})
			.UseSessionHandler(session =>
				{
					_logger.LogInformation("Medical device connected: {RemoteEndPoint}", session.RemoteEndPoint);
					return ValueTask.CompletedTask;
				},
				(session, args) =>
				{
					_logger.LogInformation("Medical device disconnected: {RemoteEndPoint}, reason: {Reason}", session.RemoteEndPoint,
						args.Reason);
					return ValueTask.CompletedTask;
				})
			.ConfigureLogging(builder => builder.ConfigureMllpLogging())
			.Build();
	}
}