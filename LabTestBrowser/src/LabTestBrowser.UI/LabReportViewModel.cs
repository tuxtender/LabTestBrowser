using AsyncAwaitBestPractices.MVVM;
using LabTestBrowser.UseCases.LabTestReports.GetEmpty;
using LabTestBrowser.UseCases.LabTestReports.GetNext;
using LabTestBrowser.UseCases.LabTestReports.GetPrevious;
using LabTestBrowser.UseCases.LabTestReports.Save;
using MediatR;

namespace LabTestBrowser.UI;

public class LabReportViewModel : BaseViewModel
{
	private readonly IMediator _mediator;

	private readonly LabRequisitionViewModel _labRequisition;

	public LabReportViewModel(IMediator mediator)
	{
		_mediator = mediator;

		NewCommand = new AsyncCommand(CreateAsync);
		SaveCommand = new AsyncCommand(SaveAsync);
		NextCommand = new AsyncCommand(GetNextAsync);
		PreviousCommand = new AsyncCommand(GetPreviousAsync);

		_labRequisition = new LabRequisitionViewModel();

		_labRequisition.Date = DateOnly.FromDateTime(DateTime.Now);
	}

	public LabRequisitionViewModel LabRequisition
	{
		get => _labRequisition;
	}

	public AsyncCommand NewCommand { get; private set; }

	public AsyncCommand SaveCommand { get; private set; }

	public AsyncCommand PreviousCommand { get; private set; }
	public AsyncCommand NextCommand { get; private set; }

	private async Task CreateAsync()
	{
		var getEmptyLabTestReportQuery = new GetEmptyLabTestReportQuery(_labRequisition.Date);
		var result = await _mediator.Send(getEmptyLabTestReportQuery);
		_labRequisition.SetLabRequisition(result.Value);
	}

	private async Task GetNextAsync()
	{
		var getNextLabTestReportQuery = new GetNextLabTestReportQuery(_labRequisition.Id);
		var result = await _mediator.Send(getNextLabTestReportQuery);
		_labRequisition.SetLabRequisition(result.Value);
	}

	private async Task GetPreviousAsync()
	{
		var getPreviousLabTestReportQuery = new GetPreviousLabTestReportQuery(_labRequisition.Id);
		var result = await _mediator.Send(getPreviousLabTestReportQuery);
		_labRequisition.SetLabRequisition(result.Value);
	}

	private async Task SaveAsync()
	{
		var saveLabTestReportCommand = new SaveLabTestReportCommand
		{
			Id = _labRequisition.Id,
			Specimen = _labRequisition.Specimen,
			Date = _labRequisition.Date,
			Facility = _labRequisition.Facility,
			TradeName = _labRequisition.TradeName,
			PetOwner = _labRequisition.PetOwner,
			Nickname = _labRequisition.Nickname,
			Animal = _labRequisition.Animal,
			Category = _labRequisition.Category,
			Breed = _labRequisition.Breed,
			AgeInYears = _labRequisition.AgeInYears,
			AgeInMonths = _labRequisition.AgeInMonths,
			AgeInDays = _labRequisition.AgeInDays
		};

		var result = await _mediator.Send(saveLabTestReportCommand);
		
		if(result.IsSuccess)
			_labRequisition.Id = result.Value;
	}
}