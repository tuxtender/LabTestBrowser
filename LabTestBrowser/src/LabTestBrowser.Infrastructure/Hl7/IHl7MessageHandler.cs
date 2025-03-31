namespace LabTestBrowser.Infrastructure.Hl7;

public interface IHl7MessageHandler
{
	Task<string> HandleMessageAsync(string message);
}