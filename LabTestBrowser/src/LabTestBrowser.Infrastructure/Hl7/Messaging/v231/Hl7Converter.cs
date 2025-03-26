using Efferent.HL7.V2;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.Hl7.Messaging.ORU_R01;

namespace LabTestBrowser.Infrastructure.Hl7.Messaging.v231;

public class Hl7Converter(ILogger<Hl7Converter> _logger) : IHl7Converter
{
	private readonly ILogger<Hl7Converter> _logger = _logger;

	public Task<ObservationMessage> ConvertToObservationMessageAsync(string strMessage)
	{
		var message = new Message(strMessage);
		message.ParseMessage();

		var messageHeader = GetMessageHeader(message);
		var observationRequest = GetObservationRequest(message);
		var observationResults = GetObservationResults(message);

		var observationMessage = new ObservationMessage
		{
			MessageHeader = messageHeader,
			ObservationRequest = observationRequest,
			ObservationResults = observationResults
		};

		return Task.FromResult(observationMessage);
	}

	private static List<ObservationResult> GetObservationResults(Message message)
	{
		var segments = message.Segments("OBX");

		var observationResults = segments.Select(obxSegment => new ObservationResult
			{
				Id = Convert.ToInt32(obxSegment.Fields(1).Value),
				ObservationIdentifier = obxSegment.Fields(3).Value,
				ObservationValue = obxSegment.Fields(5).Value,
				Units = obxSegment.Fields(6).Value,
			})
			.ToList();

		return observationResults;
	}

	private static ObservationRequest GetObservationRequest(Message message)
	{
		var segment = message.DefaultSegment("OBR");
		var hl7ObservationDatetime = segment.Fields(7).Value;
		var observationDatetime = MessageHelper.ParseDateTime(hl7ObservationDatetime, throwExeption: false);

		var observationRequest = new ObservationRequest
		{
			FillerOrderNumber = segment.Fields(3).Value,
			UniversalServiceId = segment.Fields(4).Value,
			ObservationDateTime = observationDatetime,
		};

		return observationRequest;
	}

	private static MessageHeader GetMessageHeader(Message message)
	{
		var segment = message.DefaultSegment("MSH");

		var messageHeader = new MessageHeader
		{
			SendingApplication = segment.Fields(3).Value,
			SendingFacility = segment.Fields(4).Value,
			DateTimeOfMessage = segment.Fields(7).Value,
			MessageType = segment.Fields(9).Value,
			VersionId = segment.Fields(12).Value,
		};

		return messageHeader;
	}
}