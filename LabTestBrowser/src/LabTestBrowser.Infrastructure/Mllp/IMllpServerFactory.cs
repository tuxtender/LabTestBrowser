namespace LabTestBrowser.Infrastructure.Mllp;

public interface IMllpServerFactory
{
	IMllpHost Create(Func<byte[], Task<byte[]>> messageHandler);
}