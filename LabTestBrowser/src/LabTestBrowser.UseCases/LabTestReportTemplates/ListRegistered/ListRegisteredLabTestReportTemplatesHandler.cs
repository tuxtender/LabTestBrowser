using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.Core.LabTestReportAggregate.Specifications;

namespace LabTestBrowser.UseCases.LabTestReportTemplates.ListRegistered;

public class ListRegisteredLabTestReportTemplatesHandler(
	ILabTestReportTemplateQueryService _queryService,
	IRepository<LabTestReport> _repository)
	: IQueryHandler<ListRegisteredLabTestReportTemplatesQuery, Result<IEnumerable<LabTestReportTemplateDto>>>
{
	public async Task<Result<IEnumerable<LabTestReportTemplateDto>>> Handle(ListRegisteredLabTestReportTemplatesQuery request,
		CancellationToken cancellationToken)
	{
		var spec = new LabTestReportBySpecimenSpec(request.Specimen, request.Date);
		var report = await _repository.FirstOrDefaultAsync(spec, cancellationToken);

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