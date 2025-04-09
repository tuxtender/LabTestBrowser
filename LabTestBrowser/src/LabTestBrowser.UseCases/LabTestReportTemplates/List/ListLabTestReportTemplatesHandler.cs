namespace LabTestBrowser.UseCases.LabTestReportTemplates.List;

public class ListLabTestReportTemplatesHandler(ILabTestReportTemplateQueryService _queryService)
	: IQueryHandler<ListLabTestReportTemplatesQuery,
		Result<IEnumerable<LabTestReportTemplateDto>>>
{
	public async Task<Result<IEnumerable<LabTestReportTemplateDto>>> Handle(ListLabTestReportTemplatesQuery request,
		CancellationToken cancellationToken)
	{
		var templates = await _queryService.ListAsync();

		return Result.Success(templates);
	}
}