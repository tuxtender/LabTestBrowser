using Efferent.HL7.V2;
using LabTestBrowser.Infrastructure.Hl7.Messaging.v231;
using LabTestBrowser.Infrastructure.Hl7.Messaging.v231.Segment;

namespace LabTestBrowser.Infrastructure.Hl7;

public class V231OruR01Converter : IV231OruR01Converter
{
	public OruR01 Convert(string hl7Message)
	{
		var message = new Message(hl7Message);
		message.ParseMessage();

		var msh = GetMessageHeader(message);
		var obr = GetObservationRequest(message);
		var obxList = GetObservationResults(message);

		return new OruR01
		{
			Msh = msh,
			Obr = obr,
			ObxList = obxList
		};
	}

	private static List<Obx> GetObservationResults(Message message)
	{
		var segments = message.Segments("OBX");

		var observationResults = segments.Select(obxSegment => new Obx
			{
				Id = int.Parse(obxSegment.Fields(1).Value),
				ObservationIdentifier = obxSegment.Fields(3).Value,
				ObservationValue = obxSegment.Fields(5).Value,
				Units = obxSegment.Fields(6).Value,
			})
			.ToList();

		return observationResults;
	}

	private static Obr GetObservationRequest(Message message)
	{
		var segment = message.DefaultSegment("OBR");
		var hl7ObservationDatetime = segment.Fields(7).Value;
		var observationDatetime = MessageHelper.ParseDateTime(hl7ObservationDatetime, throwExeption: false);

		var observationRequest = new Obr
		{
			FillerOrderNumber = segment.Fields(3).Value,
			UniversalServiceId = segment.Fields(4).Value,
			ObservationDateTime = observationDatetime,
		};

		return observationRequest;
	}

	private static Msh GetMessageHeader(Message message)
	{
		var segment = message.DefaultSegment("MSH");

		var messageHeader = new Msh
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