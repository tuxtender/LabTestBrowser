using LabTestBrowser.UseCases.Hl7.Exceptions;
using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment;
using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment.Urit5160.SaveUrit5160LabTestResult;
using LabTestBrowser.UseCases.Hl7.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.Hl7.ProcessHl7Request;

public class ProcessHl7RequestHandler : ICommandHandler<ProcessHl7RequestCommand, byte[]>
{
	private readonly IV231OruR01Converter _converter;
	private readonly IHl7AcknowledgmentService _acknowledgmentService;
	private readonly IMediator _mediator;
	private readonly ILogger<ProcessHl7RequestHandler> _logger;

	public ProcessHl7RequestHandler(IV231OruR01Converter converter, IHl7AcknowledgmentService acknowledgmentService, IMediator mediator,
		ILogger<ProcessHl7RequestHandler> logger)
	{
		_converter = converter;
		_acknowledgmentService = acknowledgmentService;
		_mediator = mediator;
		_logger = logger;
	}

	public async Task<byte[]> Handle(ProcessHl7RequestCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var response = await ProcessRequestAsync(request, cancellationToken);
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

	private async Task<byte[]> ProcessRequestAsync(ProcessHl7RequestCommand request, CancellationToken cancellationToken)
	{
		const string universalServiceId = "URIT^URIT-5160";

		var oruR01 = _converter.Convert(request.Hl7Message);
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