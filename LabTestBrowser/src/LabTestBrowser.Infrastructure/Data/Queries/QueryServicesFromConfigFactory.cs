using LabTestBrowser.Core.LabTestReportTemplateAggregate;
using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.UseCases.AnimalSpecies;
using LabTestBrowser.UseCases.LabTestReportTemplates;
using LabTestBrowser.UseCases.SpecimenCollectionCenters;

namespace LabTestBrowser.Infrastructure.Data.Queries;

public class QueryServicesFromConfigFactory
{
	private readonly LabReportSettings _labReportSettings;
	private readonly AnimalSettings _animalSettings;

	public QueryServicesFromConfigFactory(LabReportSettings labReportSettings, AnimalSettings animalSettings)
	{
		_labReportSettings = labReportSettings;
		_animalSettings = animalSettings;
	}

	public IListSpecimenCollectionCentersQueryService CreateListSpecimenCollectionCentersQueryService()
	{
		var centers = _labReportSettings.Facilities
			.Select(facility => new SpecimenCollectionCenterDto
			{
				Facility = facility.Supervisor,
				TradeNames = facility.Trademarks.Select(trademark => trademark.Title)
			});

		return new ListSpecimenCollectionCentersQueryService(centers);
	}

	public ILisAnimalSpeciesQueryService CreateListAnimalSpeciesQueryService()
	{
		var animals = _animalSettings.Animals
			.Select(animal => new AnimalSpeciesDto
			{
				Name = animal.Title,
				Categories = animal.Genders,
				Breeds = animal.Breeds
			});

		return new ListAnimalSpeciesQueryService(animals);
	}

	public ILabTestReportTemplateQueryService CreateLabTestReportTemplateQueryService()
	{
		var animals = _animalSettings.Animals.ToDictionary(animal => animal.Id, animal => animal.Title);

		var id = 0;

		var reportTemplates = new List<LabTestReportTemplate>();

		foreach (var facility in _labReportSettings.Facilities)
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
}