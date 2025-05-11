using Microsoft.Extensions.Hosting;

namespace LabTestBrowser.Infrastructure.Mllp;

public class MllpHostedService(IMllpServerFactory serverFactory) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var mllpServer = serverFactory.Create();
		await mllpServer.RunAsync(stoppingToken);
	}
}