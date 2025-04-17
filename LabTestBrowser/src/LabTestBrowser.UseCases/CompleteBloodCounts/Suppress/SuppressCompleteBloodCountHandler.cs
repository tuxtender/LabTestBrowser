using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Suppress;

public class SuppressCompleteBloodCountHandler(IRepository<CompleteBloodCount> _repository)
	: ICommandHandler<SuppressCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(SuppressCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		if (!request.CompleteBloodCountId.HasValue)
			return Result.Error();

		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId.Value, cancellationToken);

		if (cbc == null)
			return Result.NotFound();

		cbc.Suppress(request.SuppressionDate);
		await _repository.UpdateAsync(cbc, cancellationToken);

		return Result.Success();
	}
}