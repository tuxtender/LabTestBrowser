using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabTestBrowser.UseCases.AnimalSpecies.List;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.SpecimenCollectionCenters.List;
using MediatR;

namespace LabTestBrowser.UI;

public class LabRequisitionViewModel : ObservableObject
{
	private readonly IMediator _mediator;
	private DateOnly _labOrderDate = DateOnly.FromDateTime(DateTime.Now);
	private int _labOrderNumber;

	private string? _facility;
	private string? _tradeName;

	private string? _animal;
	private string? _petOwner;
	private string? _nickname;
	private string? _breed;
	private string? _category;

	private int? _ageInYears;
	private int? _ageInMonths;
	private int? _ageInDays;
	private IReadOnlyCollection<CollectionCenterViewModel> _collectionCenters = [];
	private IReadOnlyCollection<AnimalSpeciesViewModel> _animalSpecies = [];

	public LabRequisitionViewModel(IMediator mediator)
	{
		_mediator = mediator;

		LoadCommand = new AsyncRelayCommand(LoadAsync);
	}

	public IAsyncRelayCommand LoadCommand { get; private set; }

	public int? Id { get; internal set; }

	public DateOnly LabOrderDate
	{
		get => _labOrderDate;
		set => SetProperty(ref _labOrderDate, value);
	}

	public int LabOrderNumber
	{
		get => _labOrderNumber;
		set => SetProperty(ref _labOrderNumber, value);
	}

	public IReadOnlyCollection<CollectionCenterViewModel> CollectionCenters
	{
		get => _collectionCenters;
		private set => SetProperty(ref _collectionCenters, value);
	}

	public string? Facility
	{
		get => _facility;
		set => SetProperty(ref _facility, value);
	}

	public string? TradeName
	{
		get => _tradeName;
		set => SetProperty(ref _tradeName, value);
	}

	public IReadOnlyCollection<AnimalSpeciesViewModel> AnimalSpecies
	{
		get => _animalSpecies;
		private set => SetProperty(ref _animalSpecies, value);
	}

	public string? Animal
	{
		get => _animal;
		set => SetProperty(ref _animal, value);
	}

	public string? PetOwner
	{
		get => _petOwner;
		set => SetProperty(ref _petOwner, value);
	}

	public string? Nickname
	{
		get => _nickname;
		set => SetProperty(ref _nickname, value);
	}

	public string? Breed
	{
		get => _breed;
		set => SetProperty(ref _breed, value);
	}

	public string? Category
	{
		get => _category;
		set => SetProperty(ref _category, value);
	}

	public int? AgeInYears
	{
		get => _ageInYears;
		set => SetProperty(ref _ageInYears, value);
	}

	public int? AgeInMonths
	{
		get => _ageInMonths;
		set => SetProperty(ref _ageInMonths, value);
	}

	public int? AgeInDays
	{
		get => _ageInDays;
		set => SetProperty(ref _ageInDays, value);
	}

	public void SetLabRequisition(LabTestReportDto report)
	{
		Id = report.Id;
		LabOrderDate = report.OrderDate;
		LabOrderNumber = report.OrderNumber;
		Facility = report.Facility;
		TradeName = report.TradeName;
		Animal = report.Animal;
		PetOwner = report.HealthcareProxy;
		Nickname = report.Name;
		Breed = report.Breed;
		Category = report.Category;
		AgeInYears = report.AgeInYears;
		AgeInMonths = report.AgeInMonths;
		AgeInDays = report.AgeInDays;
	}

	private async Task LoadAsync()
	{
		var centers = await _mediator.Send(new ListSpecimenCollectionCentersQuery(null, null));
		CollectionCenters = centers.Value.Select(center => new CollectionCenterViewModel
			{
				Facility = center.Facility!,
				TradeNames = center.TradeNames
			})
			.ToList();

		var animals = await _mediator.Send(new ListAnimalSpeciesQuery(null, null));
		AnimalSpecies = animals.Value.Select(a => new AnimalSpeciesViewModel(a.Name, a.Breeds.ToList(), a.Categories.ToList())).ToList();
	}
}