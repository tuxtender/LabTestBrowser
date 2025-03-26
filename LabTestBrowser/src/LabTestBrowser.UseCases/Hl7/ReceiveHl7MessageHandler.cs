namespace LabTestBrowser.UseCases.Hl7;

public class ReceiveHl7MessageHandler(IHl7Converter _hl7Converter) : ICommandHandler<ReceiveHl7MessageCommand, Result>
{
	public async Task<Result> Handle(ReceiveHl7MessageCommand request, CancellationToken cancellationToken)
	{
		var oru = await _hl7Converter.ConvertToObservationMessageAsync(request.Message);
		return Result.Success();
	}
}