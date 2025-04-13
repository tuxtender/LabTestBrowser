using LabTestBrowser.Core.LabTestReportTemplateAggregate;
using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.UseCases.LabTestReportTemplates;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class LabTestReportTemplateQueryService : ILabTestReportTemplateQueryService
{
	private readonly Dictionary<int, LabTestReportTemplate> _templates;
	private readonly ILookup<ReportTemplateIndex, LabTestReportTemplate> _reportTemplateLookup;

	private LabTestReportTemplateQueryService(Dictionary<int, LabTestReportTemplate> templates, ILookup<ReportTemplateIndex, LabTestReportTemplate> reportTemplateLookup)
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

		var templates = reportTemplates.ToDictionary(template => template.Id);
		
		var reportTemplateLookup = reportTemplates.ToLookup(template => new ReportTemplateIndex(template.Facility, template.TradeName,
			template.Animal), template => template);

		return new LabTestReportTemplateQueryService(templates, reportTemplateLookup);
	}

	private record ReportTemplateIndex(string Facility, string TradeName, string Animal);
}