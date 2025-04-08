using LabTestBrowser.Core.LabTestReportTemplateAggregate;
using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.UseCases.LabTestReportTemplates;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class LabTestReportTemplateQueryService : ILabTestReportTemplateQueryService
{
	private readonly ILookup<ReportTemplateIndex, LabTestReportTemplate> _reportTemplateLookup;

	private LabTestReportTemplateQueryService(ILookup<ReportTemplateIndex, LabTestReportTemplate> reportTemplateLookup)
	{
		_reportTemplateLookup = reportTemplateLookup;
	}

	public Task<List<LabTestReportTemplateDto>> GetLabTestReportTemplatesAsync(string facility, string tradeName, int animalId)
	{
		var index = new ReportTemplateIndex(facility, tradeName, animalId);

		var reportTemplates = _reportTemplateLookup[index];

		var dto = reportTemplates
			.Select(template => new LabTestReportTemplateDto
			{
				Title = template.Title,
				Path = template.Path
			})
			.ToList();
		
		return Task.FromResult(dto);
	}

	public static LabTestReportTemplateQueryService Create(LabReportSettings labReportSettings)
	{
		var id = 0;

		var reportTemplates = new List<LabTestReportTemplate>();

		foreach (var facility in labReportSettings.Facilities)
		{
			foreach (var facilityTrademark in facility.Trademarks)
			{
				foreach (var reportTemplate in facilityTrademark.ReportTemplates)
				{
					var entity = new LabTestReportTemplate(facility.Supervisor, facilityTrademark.Title, reportTemplate.AnimalId,
						reportTemplate.LabTestTitle, reportTemplate.TemplatePath);

					entity.Id = id;

					reportTemplates.Add(entity);

					id++;
				}
			}
		}

		var reportTemplateLookup = reportTemplates.ToLookup(template => new ReportTemplateIndex(template.Facility, template.TradeName,
			template.AnimalId), template => template);
		
		return new LabTestReportTemplateQueryService(reportTemplateLookup);
	}

	private record ReportTemplateIndex(string Facility, string TradeName, int AnimalId);
}