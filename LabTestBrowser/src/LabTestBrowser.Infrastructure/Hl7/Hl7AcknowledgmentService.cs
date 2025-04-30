using System.Text;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.Hl7.Messaging;

namespace LabTestBrowser.Infrastructure.Hl7;

public class Hl7AcknowledgmentService : IHl7AcknowledgmentService
{
	public string GetAckMessage(AckStatus status, string messageControlId)
	{
		const char endOfBlock = '\u001c';
		const char startOfBlock = '\u000b';
		const char carriageReturn = (char)13;

		var ackMessage = new StringBuilder().Append(startOfBlock)
			.Append("MSH|^~\\&|||||||ACK||P|2.3.1")
			.Append(carriageReturn)
			.Append($"MSA|{status}|")
			.Append(messageControlId)
			.Append(carriageReturn)
			.Append(endOfBlock)
			.Append(carriageReturn)
			.ToString();

		return ackMessage;
	}
}