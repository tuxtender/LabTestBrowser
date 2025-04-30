using LabTestBrowser.UseCases.Hl7.Messaging;

namespace LabTestBrowser.UseCases.Hl7;

public interface IHl7AcknowledgmentService
{
	string GetAckMessage(AckStatus ackStatus, string messageControlId);
}