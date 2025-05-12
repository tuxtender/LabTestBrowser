namespace LabTestBrowser.Infrastructure.Mllp;

public interface IMllpHost
{
	Task RunAsync(CancellationToken token = default);
}