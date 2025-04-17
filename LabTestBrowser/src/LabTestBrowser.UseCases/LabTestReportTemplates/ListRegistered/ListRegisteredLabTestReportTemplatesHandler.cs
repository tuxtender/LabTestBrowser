using LabTestBrowser.Core.LabTestReportAggregate;

namespace LabTestBrowser.UseCases.LabTestReportTemplates.ListRegistered;

public class ListRegisteredLabTestReportTemplatesHandler(
	ILabTestReportTemplateQueryService _queryService,
	IRepository<LabTestReport> _repository)
	: IQueryHandler<ListRegisteredLabTestReportTemplatesQuery, Result<IEnumerable<LabTestReportTemplateDto>>>
{
	public async Task<Result<IEnumerable<LabTestReportTemplateDto>>> Handle(ListRegisteredLabTestReportTemplatesQuery query,
		CancellationToken cancellationToken)
	{
		if (!query.LabTestReportId.HasValue)
			return Result.NotFound();

		var report = await _repository.GetByIdAsync(query.LabTestReportId.Value, cancellationToken);

		if (report == null)
			return Result.NotFound();

		var facility = report.SpecimenCollectionCenter.Facility;
		var tradeName = report.SpecimenCollectionCenter.TradeName;
		var animal = report.Patient.Animal;

		var templates =
			await _queryService.ListAsync(facility, tradeName, animal);

		return Result.Success(templates);
	}
}