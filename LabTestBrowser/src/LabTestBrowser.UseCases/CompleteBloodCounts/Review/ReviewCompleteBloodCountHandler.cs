using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Specifications;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Review;

public class ReviewCompleteBloodCountHandler(IRepository<CompleteBloodCount> _repository)
	: ICommandHandler<ReviewCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(ReviewCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		if (!request.CompleteBloodCountId.HasValue)
			return Result.Error();

		var accessionNumber = AccessionNumber.Create(request.SequenceNumber, request.Date);

		if (!accessionNumber.IsSuccess)
			return Result.Error();

		var spec = new CompleteBloodCountByAccessionNumberSpec(accessionNumber);
		var assignedCbc = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

		if (assignedCbc != null)
		{
			assignedCbc.Suppress(request.Date);
			await _repository.UpdateAsync(assignedCbc, cancellationToken);
		}

		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId.Value, cancellationToken);

		if (cbc == null)
			return Result.NotFound();

		cbc.Review(accessionNumber);
		await _repository.UpdateAsync(cbc, cancellationToken);

		return Result.Success();
	}
}