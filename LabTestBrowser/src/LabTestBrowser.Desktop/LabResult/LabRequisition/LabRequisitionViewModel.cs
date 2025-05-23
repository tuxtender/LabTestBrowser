using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LabTestBrowser.Desktop.LabResult.Messages;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.Get;
using LabTestBrowser.UseCases.LabTestReports.GetLast;
using MediatR;

namespace LabTestBrowser.Desktop.LabResult.LabRequisition;

public partial class LabRequisitionViewModel : ObservableObject, IRecipient<LabOrderChangedMessage>
{
	private readonly IMediator _mediator;
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
	private bool _isExternalUpdate;

	public LabRequisitionViewModel(IMediator mediator, ILogger<LabRequisitionViewModel> logger)
	{
		_mediator = mediator;
		_logger = logger;

		WeakReferenceMessenger.Default.Register(this, LabOrderSyncToken.FromSecondary);
		WeakReferenceMessenger.Default.Register<LabRequisitionViewModel, LabOrderRequestMessage>(this,
			(viewModel, message) => message.Reply(viewModel.LabOrder));
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

	private LabOrder LabOrder => new(LabOrderNumber, LabOrderDate);

	public async void Receive(LabOrderChangedMessage message)
	{
		try
		{
			await UpdateExternal(message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error updating lab requisition");
		}
	}

	public async Task LoadAsync()
	{
		await RestoreSessionAsync(LabOrderDate);

		Notify();
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

	private async Task UpdateExternal(LabOrderChangedMessage message)
	{
		_isExternalUpdate = true;
		var (labOrderNumber, labOrderDate) = message.Value;
		await UpdateAsync(labOrderNumber);
		_isExternalUpdate = false;
	}
}