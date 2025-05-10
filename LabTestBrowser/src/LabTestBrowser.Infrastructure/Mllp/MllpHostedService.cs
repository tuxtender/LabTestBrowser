using Microsoft.Extensions.Hosting;

namespace LabTestBrowser.Infrastructure.Mllp;

public class MllpHostedService(IMllpHostBuilder builder) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var mllpHost = builder.Build();
		await mllpHost.RunAsync(stoppingToken);
	}
}