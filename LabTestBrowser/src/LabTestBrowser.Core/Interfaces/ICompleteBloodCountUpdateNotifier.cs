namespace LabTestBrowser.Core.Interfaces;

public interface ICompleteBloodCountUpdateNotifier
{
	Task NotifyAsync(int completeBloodCountId);
}