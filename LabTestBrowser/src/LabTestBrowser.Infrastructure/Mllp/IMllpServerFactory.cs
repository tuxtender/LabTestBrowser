using Microsoft.Extensions.Hosting;

namespace LabTestBrowser.Infrastructure.Mllp;

public interface IMllpServerFactory
{
	IHost Create();
}