using LabTestBrowser.UseCases.Hl7.Messaging.ORU_R01;

namespace LabTestBrowser.UseCases.Hl7;

public interface IHl7Converter
{
	Task<ObservationMessage> ConvertToObservationMessageAsync(string message);
}