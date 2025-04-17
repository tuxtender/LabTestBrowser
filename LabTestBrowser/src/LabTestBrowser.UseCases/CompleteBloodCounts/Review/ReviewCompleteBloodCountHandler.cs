using LabTestBrowser.Core.CompleteBloodCountAggregate;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Review;

public class ReviewCompleteBloodCountHandler(IRepository<CompleteBloodCount> _repository)
	: ICommandHandler<ReviewCompleteBloodCountCommand, Result>
{
	public async Task<Result> Handle(ReviewCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		if (!request.CompleteBloodCountId.HasValue)
			return Result.Error();

		var cbc = await _repository.GetByIdAsync(request.CompleteBloodCountId.Value, cancellationToken);

		if (cbc == null)
			return Result.NotFound();

		var accessionNumber = AccessionNumber.Create(request.SequenceNumber, request.Date);

		if (!accessionNumber.IsSuccess)
			return Result.Error();

		cbc.Review(accessionNumber);
		await _repository.UpdateAsync(cbc, cancellationToken);

		return Result.Success();
	}
}