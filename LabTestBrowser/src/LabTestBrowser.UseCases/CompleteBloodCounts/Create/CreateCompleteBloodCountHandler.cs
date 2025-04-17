using LabTestBrowser.Core.CompleteBloodCountAggregate;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Events;
using MediatR;

namespace LabTestBrowser.UseCases.CompleteBloodCounts.Create;

public class CreateCompleteBloodCountHandler(IRepository<CompleteBloodCount> _repository, IMediator _mediator, ICbcTestResultReader _reader) : ICommandHandler<CreateCompleteBloodCountCommand, Result<int>>
{
	private readonly IMediator _mediator = _mediator;

	public async Task<Result<int>> Handle(CreateCompleteBloodCountCommand request, CancellationToken cancellationToken)
	{
		var cbc = new CompleteBloodCount(request.ExternalId, request.ObservationDateTime);
		
		cbc.SetWhiteBloodCell(request.WhiteBloodCell);
		cbc.SetLymphocytePercent(request.LymphocytePercent);
		cbc.SetMonocytePercent(request.MonocytePercent);
		cbc.SetNeutrophilPercent(request.NeutrophilPercent);
		cbc.SetEosinophilPercent(request.EosinophilPercent);
		cbc.SetBasophilPercent(request.BasophilPercent);
		cbc.SetRedBloodCell(request.RedBloodCell);
		cbc.SetHemoglobin(request.Hemoglobin);
		cbc.SetHematocrit(request.Hematocrit);
		cbc.SetMeanCorpuscularVolume(request.MeanCorpuscularVolume);
		cbc.SetMeanCorpuscularHemoglobin(request.MeanCorpuscularHemoglobin);
		cbc.SetMeanCorpuscularHemoglobinConcentration(request.MeanCorpuscularHemoglobinConcentration);
		cbc.SetRedBloodCellDistributionWidth(request.RedBloodCellDistributionWidth);
		cbc.SetPlatelet(request.Platelet);
		cbc.SetMeanPlateletVolume(request.MeanPlateletVolume);
		
		await _repository.AddAsync(cbc, cancellationToken);
		// await _mediator.Publish(new CompleteBloodCountCreatedEvent(cbc.Id), cancellationToken);

		var dto = cbc.ConvertToCompleteBloodCountDto();

		await _reader.WriteAsync(dto);

		return cbc.Id;
	}
}