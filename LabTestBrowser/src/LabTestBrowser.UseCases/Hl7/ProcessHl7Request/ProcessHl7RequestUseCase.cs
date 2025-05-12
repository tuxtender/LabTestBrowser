using LabTestBrowser.UseCases.Hl7.Exceptions;
using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment;
using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment.Urit5160.SaveUrit5160LabTestResult;
using LabTestBrowser.UseCases.Hl7.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.Hl7.ProcessHl7Request;

public class ProcessHl7RequestUseCase : IProcessHl7RequestUseCase
{
	private readonly IV231OruR01Converter _converter;
	private readonly IHl7AcknowledgmentService _acknowledgmentService;
	private readonly IMediator _mediator;
	private readonly ILogger<ProcessHl7RequestUseCase> _logger;

	public ProcessHl7RequestUseCase(IV231OruR01Converter converter, IHl7AcknowledgmentService acknowledgmentService, IMediator mediator,
		ILogger<ProcessHl7RequestUseCase> logger)
	{
		_converter = converter;
		_acknowledgmentService = acknowledgmentService;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task<byte[]> ExecuteAsync(byte[] hl7Message, CancellationToken cancellationToken = default)
	{
		try
		{
			var response = await ProcessRequestAsync(hl7Message, cancellationToken);
			return response;
		}

		catch (UnsupportedHl7MessageException ex)
		{
			_logger.LogWarning(ex, "Unsupported HL7 message: {MessageType}", ex.MessageType);
			return _acknowledgmentService.GetAckMessage(AckStatus.AR, ex.MessageControlId);
		}
		catch (Hl7ParsingException ex)
		{
			_logger.LogWarning(ex, "Invalid HL7 format or unrecognizable message");
			return [];
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception");
			return [];
		}
	}

	private async Task<byte[]> ProcessRequestAsync(byte[] hl7Message, CancellationToken cancellationToken)
	{
		const string universalServiceId = "URIT^URIT-5160";

		var oruR01 = _converter.Convert(hl7Message);
		if (universalServiceId != oruR01.Obr.UniversalServiceId)
		{
			_logger.LogWarning("Unsupported lab equipment. Sending service: {UniversalServiceId} ", oruR01.Obr.UniversalServiceId);
			return _acknowledgmentService.GetAckMessage(AckStatus.AR, oruR01.Msh.MessageControlId);
		}

		ISaveLabTestResultCommand command = new SaveUrit5160LabTestResultCommand(oruR01);
		var result = await _mediator.Send(command, cancellationToken);
		var ackStatus = result.IsSuccess ? AckStatus.AA : AckStatus.AE;

		return _acknowledgmentService.GetAckMessage(ackStatus, oruR01.Msh.MessageControlId);
	}
}