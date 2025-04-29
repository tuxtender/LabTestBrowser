using LabTestBrowser.Core.LabTestReportAggregate;
using Microsoft.Extensions.Logging;

namespace LabTestBrowser.UseCases.LabTestReportTemplates.ListRegistered;

public class ListRegisteredLabTestReportTemplatesHandler(
	ILabTestReportTemplateQueryService _queryService,
	IRepository<LabTestReport> _repository,
	IErrorLocalizationService _errorLocalizer,
	ILogger<ListRegisteredLabTestReportTemplatesHandler> _logger)
	: IQueryHandler<ListRegisteredLabTestReportTemplatesQuery, Result<IEnumerable<LabTestReportTemplateDto>>>
{
	public async Task<Result<IEnumerable<LabTestReportTemplateDto>>> Handle(ListRegisteredLabTestReportTemplatesQuery query,
		CancellationToken cancellationToken)
	{
		if (!query.LabTestReportId.HasValue)
			return Result.Invalid(new ValidationError(_errorLocalizer.GetLabTestReportRequired()));

		var report = await _repository.GetByIdAsync(query.LabTestReportId.Value, cancellationToken);
		if (report == null)
		{
			_logger.LogWarning("LabTestReport id: {labTestReportId} not found", query.LabTestReportId);
			return Result.CriticalError(_errorLocalizer.GetApplicationFault());
		}

		var facility = report.SpecimenCollectionCenter.Facility;
		var tradeName = report.SpecimenCollectionCenter.TradeName;
		var animal = report.Patient.Animal;

		var templates = await _queryService.ListAsync(facility, tradeName, animal);

		return Result.Success(templates);
	}
}