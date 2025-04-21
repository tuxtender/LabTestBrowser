using LabTestBrowser.Infrastructure.Data.Settings;
using LabTestBrowser.UseCases.AnimalSpecies;
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
}