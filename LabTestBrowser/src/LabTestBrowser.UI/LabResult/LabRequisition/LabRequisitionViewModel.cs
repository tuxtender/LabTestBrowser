using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LabTestBrowser.UI.LabResult.Messages;
using LabTestBrowser.UI.Notification;
using LabTestBrowser.UseCases.AnimalSpecies.List;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.Get;
using LabTestBrowser.UseCases.LabTestReports.GetEmpty;
using LabTestBrowser.UseCases.LabTestReports.GetLast;
using LabTestBrowser.UseCases.LabTestReports.GetNext;
using LabTestBrowser.UseCases.LabTestReports.GetPrevious;
using LabTestBrowser.UseCases.LabTestReports.Save;
using LabTestBrowser.UseCases.SpecimenCollectionCenters.List;
using MediatR;

namespace LabTestBrowser.UI.LabResult.LabRequisition;

using Localizations = Resources.Strings;

public partial class LabRequisitionViewModel : ObservableObject, IRecipient<LabOrderChangedMessage>
{
	private readonly IMediator _mediator;
	private readonly INotificationService _notificationService;
	private readonly ILogger<LabRequisitionViewModel> _logger;

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
	private IReadOnlyCollection<LabResult.LabRequisition.AnimalSpeciesViewModel> _animalSpecies = [];
	private bool _isExternalUpdate;

	public LabRequisitionViewModel(IMediator mediator, INotificationService notificationService, ILogger<LabRequisitionViewModel> logger)
	{
		_mediator = mediator;
		_notificationService = notificationService;
		_logger = logger;

		WeakReferenceMessenger.Default.Register(this, LabOrderSyncToken.FromSecondary);
		WeakReferenceMessenger.Default.Register<LabRequisitionViewModel, LabOrderRequestMessage>(this,
			(r, m) => { m.Reply(new LabOrder(LabOrderNumber, LabOrderDate)); });
	}

	public int? Id { get; private set; }

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

	public IReadOnlyCollection<LabResult.LabRequisition.AnimalSpeciesViewModel> AnimalSpecies
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

	public async void Receive(LabOrderChangedMessage message)
	{
		try
		{
			_isExternalUpdate = true;

			var (labOrderNumber, labOrderDate) = message.Value;
			await UpdateAsync(labOrderNumber);

			_isExternalUpdate = false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating lab requisition");
		}
	}

	public async Task CreateAsync()
	{
		var getEmptyLabTestReportQuery = new GetEmptyLabTestReportQuery(LabOrderDate);
		var result = await _mediator.Send(getEmptyLabTestReportQuery);
		SetLabRequisition(result.Value);
	}

	public async Task NextAsync()
	{
		var getNextLabTestReportQuery = new GetNextLabTestReportQuery(LabOrderNumber, LabOrderDate);
		var result = await _mediator.Send(getNextLabTestReportQuery);
		SetLabRequisition(result.Value);
	}

	public async Task PreviousAsync()
	{
		var getPreviousLabTestReportQuery = new GetPreviousLabTestReportQuery(LabOrderNumber, LabOrderDate);
		var result = await _mediator.Send(getPreviousLabTestReportQuery);
		SetLabRequisition(result.Value);
	}

	public async Task SaveAsync()
	{
		var saveLabTestReportCommand = new SaveLabTestReportCommand
		{
			Id = Id,
			OrderNumber = LabOrderNumber,
			OrderDate = LabOrderDate,
			Facility = Facility,
			TradeName = TradeName,
			PetOwner = PetOwner,
			Nickname = Nickname,
			Animal = Animal,
			Category = Category,
			Breed = Breed,
			AgeInYears = AgeInYears,
			AgeInMonths = AgeInMonths,
			AgeInDays = AgeInDays
		};

		var result = await _mediator.Send(saveLabTestReportCommand);
		var notification = result.ToNotification(Localizations.LabReport_ReportSavingFailed);

		if (result.IsSuccess)
		{
			SetLabRequisition(result);
			notification = result.ToNotification(Localizations.LabReport_ReportSaved);
		}

		await _notificationService.PublishAsync(notification);
	}

	// [RelayCommand]
	public async Task LoadAsync()
	{
		var centers = await _mediator.Send(new ListSpecimenCollectionCentersQuery(null, null));
		CollectionCenters = centers.Value.Select(center => new CollectionCenterViewModel
			{
				Facility = center.Facility!,
				TradeNames = center.TradeNames
			})
			.ToList();

		var animals = await _mediator.Send(new ListAnimalSpeciesQuery(null, null));
		AnimalSpecies = animals.Value.Select(a => new LabResult.LabRequisition.AnimalSpeciesViewModel(a.Name, a.Breeds.ToList(), a.Categories.ToList())).ToList();

		await RestoreSessionAsync(LabOrderDate);
	}

	[RelayCommand]
	private async Task UpdateAsync(int labOrderNumber)
	{
		var query = new GetLabTestReportQuery(labOrderNumber, LabOrderDate);
		var result = await _mediator.Send(query);
		SetLabRequisition(result.Value);
	}

	[RelayCommand]
	private async Task RestoreSessionAsync(DateOnly labOrderDate)
	{
		var query = new GetLastLabTestReportQuery(labOrderDate);
		var result = await _mediator.Send(query);
		SetLabRequisition(result.Value);
	}

	[RelayCommand]
	private void Notify()
	{
		if (_isExternalUpdate)
			return;

		var labOrder = new LabOrder(LabOrderNumber, LabOrderDate);
		WeakReferenceMessenger.Default.Send(new LabOrderChangedMessage(labOrder), LabOrderSyncToken.FromPrimary);
	}

	private void SetLabRequisition(LabTestReportDto report)
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
}