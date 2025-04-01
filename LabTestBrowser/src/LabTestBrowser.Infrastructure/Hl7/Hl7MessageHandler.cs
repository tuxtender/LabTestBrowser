using System.Text;
using Efferent.HL7.V2;
using LabTestBrowser.UseCases.CompleteBloodCounts.Create;
using LabTestBrowser.UseCases.LaboratoryEquipment;
using MediatR;

namespace LabTestBrowser.Infrastructure.Hl7;

public class Hl7MessageHandler : IHl7MessageHandler
{
	private readonly IUrit5160Hl7Converter _urit5160Hl7Converter;
	private readonly IMediator _mediator;

	public Hl7MessageHandler(IUrit5160Hl7Converter urit5160Hl7Converter, IMediator mediator)
	{
		_urit5160Hl7Converter = urit5160Hl7Converter;
		_mediator = mediator;
	}

	public async Task<string> HandleMessageAsync(string message)
	{
		var urit5160CompleteBloodCount = await _urit5160Hl7Converter.ConvertAsync(message);

		var command = new CreateCompleteBloodCountCommand
		{
			ExternalId = urit5160CompleteBloodCount.ExternalId,
			ObservationDateTime = urit5160CompleteBloodCount.ObservationDateTime,
			WhiteBloodCell = urit5160CompleteBloodCount.WhiteBloodCell,
			LymphocytePercent = urit5160CompleteBloodCount.LymphocytePercent,
			MonocytePercent = urit5160CompleteBloodCount.MonocytePercent,
			NeutrophilPercent = urit5160CompleteBloodCount.NeutrophilPercent,
			EosinophilPercent = urit5160CompleteBloodCount.EosinophilPercent,
			BasophilPercent = urit5160CompleteBloodCount.BasophilPercent,
			RedBloodCell = urit5160CompleteBloodCount.RedBloodCell,
			Hemoglobin = urit5160CompleteBloodCount.Hemoglobin,
			Hematocrit = urit5160CompleteBloodCount.Hematocrit,
			MeanCorpuscularVolume = urit5160CompleteBloodCount.MeanCorpuscularVolume,
			MeanCorpuscularHemoglobin = urit5160CompleteBloodCount.MeanCorpuscularHemoglobin,
			MeanCorpuscularHemoglobinConcentration = urit5160CompleteBloodCount.MeanCorpuscularHemoglobinConcentration,
			RedBloodCellDistributionWidth = urit5160CompleteBloodCount.RedBloodCellDistributionWidth,
			Platelet = urit5160CompleteBloodCount.Platelet,
			MeanPlateletVolume = urit5160CompleteBloodCount.MeanPlateletVolume,
		};
		
		//TODO: Refactoring
		var hl7msg = new Message(message);
		hl7msg.ParseMessage();
		var controlId = hl7msg.MessageControlID;

		char END_OF_BLOCK = '\u001c';
		char START_OF_BLOCK = '\u000b';
		char CARRIAGE_RETURN = (char)13;
		
		var ackMessage = new StringBuilder();
		ackMessage = ackMessage.Append(START_OF_BLOCK)
			.Append("MSH|^~\\&|||||||ACK||P|2.3.1")
			.Append(CARRIAGE_RETURN)
			.Append("MSA|AA|")
			.Append(controlId)
			.Append(CARRIAGE_RETURN)
			.Append(END_OF_BLOCK)
			.Append(CARRIAGE_RETURN);

		
		var ackStr = ackMessage.ToString();

		var result = await _mediator.Send(command);

		if (result.IsSuccess)
			return ackStr; //TODO: ACK

		return string.Empty; //TODO: NACK
	}
}