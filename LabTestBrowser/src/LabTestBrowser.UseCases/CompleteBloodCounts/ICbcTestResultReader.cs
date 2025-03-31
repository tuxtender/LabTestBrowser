namespace LabTestBrowser.UseCases.CompleteBloodCounts;

public interface ICbcTestResultReader
{
	Task<CompleteBloodCountDto> ReadAsync();
	Task WriteAsync(CompleteBloodCountDto testResults);
}