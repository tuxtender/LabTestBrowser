using System.Text;
using LabTestBrowser.Infrastructure.Hl7;
using LabTestBrowser.UseCases.Hl7;
using LabTestBrowser.UseCases.Hl7.LaboratoryEquipment;
using LabTestBrowser.UseCases.Hl7.ProcessHl7Request;
using Microsoft.Extensions.Logging.Abstractions;

namespace LabTestBrowser.UnitTests.UseCases.ProcessHl7Request;

public class ProcessHl7RequestUseCaseTests
{
	private readonly IMediator _mediator = Substitute.For<IMediator>();
	private readonly IHl7AcknowledgmentService _acknowledgmentService = Substitute.For<IHl7AcknowledgmentService>();
	private readonly ILogger<ProcessHl7RequestUseCase> _logger = NullLogger<ProcessHl7RequestUseCase>.Instance; 
	private readonly V231OruR01Converter _converter = new();
	private readonly ProcessHl7RequestUseCase _processHl7RequestUseCase;

	public ProcessHl7RequestUseCaseTests()
	{
		_processHl7RequestUseCase = new ProcessHl7RequestUseCase(_converter, _acknowledgmentService, _mediator, _logger);
	}

	[Fact]
	public async Task ExecuteAsync_WithUrit5160Message_SendsSaveLabTestResultCommand()
	{
		var urit5160Message =
			"MSH|^~\\&|URIT|URIT-5160|LIS|PC|20241017224800||ORU^R01|4|P|2.3.1||||||UNICODE\r"
			+ "PV1|0||\r"
			+ "PID|0||3||||0|\r"
			+ "OBR|1||000000016856|URIT^URIT-5160|||20241017224700||||||||1|||||1|\r";

		var urit5160MllpMessage = Encoding.UTF8.GetBytes(urit5160Message);

		await _processHl7RequestUseCase.ExecuteAsync(urit5160MllpMessage);

		await _mediator.Received(1)
			.Send(Arg.Any<ISaveLabTestResultCommand>());
	}

	[Fact]
	public async Task ExecuteAsync_WhenOruR01IsFromUnsupportedDevice_DoesNotSendSaveLabTestResultCommand()
	{
		var unsupportedDeviceMessage =
			"MSH|^~\\&|||LIS|PC|20241017224800||ORU^R01|4|P|2.3.1||||||UNICODE\r"
			+ "OBR|1||000000016856|SOME^SERVICE^ID|||20241017224700||||||||1|||||1|\r";

		var urit5160MllpMessage = Encoding.UTF8.GetBytes(unsupportedDeviceMessage);

		await _processHl7RequestUseCase.ExecuteAsync(urit5160MllpMessage);

		_mediator.ReceivedCalls()
			.Should()
			.NotContain(call => call.GetMethodInfo().Name == nameof(IMediator.Send));
	}
}