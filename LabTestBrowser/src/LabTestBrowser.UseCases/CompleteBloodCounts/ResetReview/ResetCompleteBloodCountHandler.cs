using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.ResetReview;

public class ResetCompleteBloodCountHandler(IRepository<CompleteBloodCount> _repository)
	: ICommandHandler<ResetCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(ResetCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		if (!request.CompleteBloodCountId.HasValue)
			return Result.Error();

		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId.Value, cancellationToken);

		if (cbc == null)
			return Result.NotFound();

		cbc.Review();
		await _repository.UpdateAsync(cbc, cancellationToken);

		return Result.Success();
	}
}