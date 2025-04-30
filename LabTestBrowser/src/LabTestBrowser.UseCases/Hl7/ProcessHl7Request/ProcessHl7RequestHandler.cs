using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment.Urit5160.SaveUrit5160LabTestResult;
using LabTestBrowser.UseCases.Hl7.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.Hl7.ProcessHl7Request;

public class ProcessHl7RequestHandler : ICommandHandler<ProcessHl7RequestCommand, string>
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

	public async Task<string> Handle(ProcessHl7RequestCommand request, CancellationToken cancellationToken)
	{
		//TODO: convert exception handling
		const string universalServiceId = "URIT^URIT-5160";

		var oruR01 = _converter.Convert(request.Message);
		if (universalServiceId != oruR01.Obr.UniversalServiceId)
		{
			_logger.LogWarning("Unsupported lab equipment. Sending service: {universalServiceId} ", oruR01.Obr.UniversalServiceId);
			return _acknowledgmentService.GetAckMessage(AckStatus.AR, oruR01.Msh.MessageControlId);
		}

		ICommand<Result> command = new SaveUrit5160LabTestResultCommand(oruR01);
		var result = await _mediator.Send(command, cancellationToken);

		var ackMessage = _acknowledgmentService.GetAckMessage(AckStatus.AA, oruR01.Msh.MessageControlId);

		return ackMessage;
	}
}