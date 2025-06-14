﻿using System.Text;
using Efferent.HL7.V2;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.Hl7.Exceptions;
using LabTestBrowser.UseCases.Hl7.Messaging.v231;
using LabTestBrowser.UseCases.Hl7.Messaging.v231.Segment;

namespace LabTestBrowser.Infrastructure.Hl7;

public class V231OruR01Converter : IV231OruR01Converter
{
	public OruR01 Convert(byte[] hl7Message)
	{
		var encodedHl7Message = Encoding.UTF8.GetString(hl7Message);
		var message = new Message(encodedHl7Message);
		ParseMessage(message);
		var oruR01 = ConvertToOruR01(message);

		return oruR01;
	}

	private static OruR01 ConvertToOruR01(Message message)
	{
		try
		{
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
		catch (Exception ex)
		{
			throw new UnsupportedHl7MessageException(message.MessageControlID, message.MessageStructure,
				$"HL7 message '{message.MessageStructure}' is not supported. Expected message type: ORU_R01", ex);
		}
	}

	private static void ParseMessage(Message message)
	{
		try
		{
			message.ParseMessage();
		}
		catch (Exception ex)
		{
			throw new Hl7ParsingException("Parsing error", ex);
		}
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
			MessageControlId = segment.Fields(10).Value,
			VersionId = segment.Fields(12).Value,
		};

		return messageHeader;
	}
}