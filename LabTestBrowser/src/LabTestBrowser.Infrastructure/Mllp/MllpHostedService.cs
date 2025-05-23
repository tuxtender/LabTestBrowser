using Microsoft.Extensions.Hosting;

namespace LabTestBrowser.Infrastructure.Mllp;

public class MllpHostedService(IMllpHost mllpHost) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		await mllpHost.RunAsync(stoppingToken);
	}
}