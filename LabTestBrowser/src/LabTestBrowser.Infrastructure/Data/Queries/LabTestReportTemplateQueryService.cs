using LabTestBrowser.Core.LabTestReportTemplateAggregate;
using LabTestBrowser.UseCases.LabTestReportTemplates;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class LabTestReportTemplateQueryService : ILabTestReportTemplateQueryService
{
	private readonly Dictionary<int, LabTestReportTemplate> _templates;
	private readonly ILookup<ReportTemplateIndex, LabTestReportTemplate> _reportTemplateLookup;

	public LabTestReportTemplateQueryService(Dictionary<int, LabTestReportTemplate> templates, ILookup<ReportTemplateIndex, LabTestReportTemplate> reportTemplateLookup)
	{
		_templates = templates;
		_reportTemplateLookup = reportTemplateLookup;
	}

	public Task<LabTestReportTemplateDto?> FindById(int labTestReportTemplateId)
	{
		if (!_templates.TryGetValue(labTestReportTemplateId, out var template))
			return Task.FromResult<LabTestReportTemplateDto?>(null);

		var dto = new LabTestReportTemplateDto
		{
			Id = template.Id,
			Title = template.Title,
			Path = template.Path,
			FileFormat = template.TemplateFileExtension.FileFormat,
			FileExtension = template.TemplateFileExtension.FileExtension
		};

		return Task.FromResult<LabTestReportTemplateDto?>(dto);
	}

	public Task<IEnumerable<LabTestReportTemplateDto>> ListAsync(string facility, string? tradeName, string animal)
	{
		var index = new ReportTemplateIndex(facility, tradeName ?? string.Empty, animal);

		var reportTemplates = _reportTemplateLookup[index];

		var dto = reportTemplates
			.Select(template => new LabTestReportTemplateDto
			{
				Id = template.Id,
				Title = template.Title,
				Path = template.Path,
				FileFormat = template.TemplateFileExtension.FileFormat,
				FileExtension = template.TemplateFileExtension.FileExtension
			});

		return Task.FromResult(dto);
	}

	public Task<IEnumerable<LabTestReportTemplateDto>> ListAsync()
	{
		var templates = _reportTemplateLookup
			.SelectMany(group => group)
			.Select(template => new LabTestReportTemplateDto
			{
				Id = template.Id,
				Title = template.Title,
				Path = template.Path,
				FileFormat = template.TemplateFileExtension.FileFormat,
				FileExtension = template.TemplateFileExtension.FileExtension
			});

		return Task.FromResult(templates);
	}
}