namespace LabTestBrowser.UseCases.Hl7.ProcessHl7Request;

public interface IProcessHl7RequestUseCase
{
	Task<byte[]> ExecuteAsync(byte[] hl7Message, CancellationToken cancellationToken = default);
}