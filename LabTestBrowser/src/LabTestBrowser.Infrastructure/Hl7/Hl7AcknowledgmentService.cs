using Efferent.HL7.V2;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.Hl7.Messaging;

namespace LabTestBrowser.Infrastructure.Hl7;

public class Hl7AcknowledgmentService : IHl7AcknowledgmentService
{
	public byte[] GetAckMessage(AckStatus status, string messageControlId)
	{
		var message = new Message();
		message.AddSegmentMSH(null, null, null, null, null, "ACK", messageControlId, "P", "2.3.1");
		var msa = new Segment("MSA", new HL7Encoding());
		msa.AddNewField(status.ToString());
		msa.AddNewField(messageControlId);
		message.AddNewSegment(msa);
		var mllpMessage = message.GetMLLP();

		return mllpMessage;
	}
}