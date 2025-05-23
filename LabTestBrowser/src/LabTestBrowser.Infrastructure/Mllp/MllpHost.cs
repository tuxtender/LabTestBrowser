using Microsoft.Extensions.Hosting;

namespace LabTestBrowser.Infrastructure.Mllp;

public class MllpHost(IHost host) : IMllpHost
{
	public async Task RunAsync(CancellationToken token = default) => await host.RunAsync(token);
}