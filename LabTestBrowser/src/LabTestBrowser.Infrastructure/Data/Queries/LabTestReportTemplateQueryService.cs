using LabTestBrowser.Core.LabTestReportTemplateAggregate;
using LabTestBrowser.UseCases.LabTestReportTemplates;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class LabTestReportTemplateQueryService : ILabTestReportTemplateQueryService
{
	private readonly Dictionary<int, LabTestReportTemplate> _templates;
	private readonly ILookup<ReportTemplateIndex, LabTestReportTemplate> _reportTemplateLookup;

	public LabTestReportTemplateQueryService(Dictionary<int, LabTestReportTemplate> templates,
		ILookup<ReportTemplateIndex, LabTestReportTemplate> reportTemplateLookup)
	{
		_templates = templates;
		_reportTemplateLookup = reportTemplateLookup;
	}

	public Task<LabTestReportTemplate?> FindById(int labTestReportTemplateId)
	{
		if (!_templates.TryGetValue(labTestReportTemplateId, out var template))
			return Task.FromResult<LabTestReportTemplate?>(null);

		return Task.FromResult<LabTestReportTemplate?>(template);
	}

	public Task<IEnumerable<LabTestReportTemplate>> ListAsync(string facility, string? tradeName, string animal)
	{
		var index = new ReportTemplateIndex(facility, tradeName ?? string.Empty, animal);
		var reportTemplates = _reportTemplateLookup[index];

		return Task.FromResult(reportTemplates);
	}

	public Task<IEnumerable<LabTestReportTemplate>> ListAsync()
	{
		var templates = _reportTemplateLookup
			.SelectMany(group => group);

		return Task.FromResult(templates);
	}
}