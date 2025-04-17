namespace LabTestBrowser.Core.Interfaces;

public interface ICompleteBloodCountUpdateChannel
{
	Task<int> ReadAsync();
	Task WriteAsync(int completeBloodCountId);
}