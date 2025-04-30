using LabTestBrowser.UseCases.Hl7.Messaging;

namespace LabTestBrowser.UseCases.Hl7;

public interface IHl7AcknowledgmentService
{
	byte[] GetAckMessage(AckStatus ackStatus, string messageControlId);
}