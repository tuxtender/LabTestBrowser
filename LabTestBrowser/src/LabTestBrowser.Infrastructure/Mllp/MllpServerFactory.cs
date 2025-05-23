using Microsoft.Extensions.Hosting;
using SuperSocket.Server.Abstractions;
using SuperSocket.Server.Host;

namespace LabTestBrowser.Infrastructure.Mllp;

public class MllpServerFactory : IMllpServerFactory
{
	private readonly ILogger<MllpHostedService> _logger;
	private readonly MllpOptions _settings;

	public MllpServerFactory(IOptions<MllpOptions> settings, ILogger<MllpHostedService> logger)
	{
		_logger = logger;
		_settings = settings.Value;
	}

	public IMllpHost Create(Func<byte[], Task<byte[]>> messageHandler)
	{
		var builder = SuperSocketHostBuilder.Create<MllpPackage, MllpPipelineFilter>()
			.UsePackageHandler(async (session, package) =>
			{
				var response = await messageHandler(package.Content);
				await session.SendAsync(response);
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
			.ConfigureLogging(builder => builder.ConfigureMllpLogging());

		var mllpHost = builder.Build();
		return new MllpHost(mllpHost);
	}
}