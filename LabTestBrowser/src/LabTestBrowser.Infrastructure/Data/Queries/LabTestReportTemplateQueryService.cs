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

	public Task<IEnumerable<LabTestReportTemplateDto>> ListAsync(string facility, string? tradeName, string animal)
	{
		var index = new ReportTemplateIndex(facility, tradeName ?? string.Empty, animal);

		var reportTemplates = _reportTemplateLookup[index];

		var dto = reportTemplates
			.Select(template => new LabTestReportTemplateDto
			{
				Title = template.Title,
				Path = template.Path
			});

		return Task.FromResult(dto);
	}

	public Task<IEnumerable<LabTestReportTemplateDto>> ListAsync()
	{
		var templates = _reportTemplateLookup
			.SelectMany(group => group)
			.Select(template => new LabTestReportTemplateDto
			{
				Title = template.Title,
				Path = template.Path
			});

		return Task.FromResult(templates);
	}

	public static LabTestReportTemplateQueryService Create(LabReportSettings labReportSettings, AnimalSettings animalSettings)
	{
		var animals = animalSettings.Animals.ToDictionary(animal => animal.Id, animal => animal.Title);

		var id = 0;

		var reportTemplates = new List<LabTestReportTemplate>();

		foreach (var facility in labReportSettings.Facilities)
		foreach (var facilityTrademark in facility.Trademarks)
		foreach (var reportTemplate in facilityTrademark.ReportTemplates)
		{
			var animal = animals[reportTemplate.AnimalId];
			var entity = new LabTestReportTemplate(facility.Supervisor, facilityTrademark.Title, animal,
				reportTemplate.LabTestTitle, reportTemplate.TemplatePath);

			entity.Id = id;

			reportTemplates.Add(entity);

			id++;
		}

		var reportTemplateLookup = reportTemplates.ToLookup(template => new ReportTemplateIndex(template.Facility, template.TradeName,
			template.Animal), template => template);

		return new LabTestReportTemplateQueryService(reportTemplateLookup);
	}

	private record ReportTemplateIndex(string Facility, string TradeName, string Animal);
}