using System.Collections.ObjectModel;
using System.Windows.Data;
using AsyncAwaitBestPractices.MVVM;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UseCases.CompleteBloodCounts;
using LabTestBrowser.UseCases.LabTestReports.GetEmpty;
using LabTestBrowser.UseCases.LabTestReports.GetLast;
using LabTestBrowser.UseCases.LabTestReports.GetNext;
using LabTestBrowser.UseCases.LabTestReports.GetPrevious;
using LabTestBrowser.UseCases.LabTestReports.Save;
using MediatR;

namespace LabTestBrowser.UI;

public class LabReportViewModel : BaseViewModel
{
	private readonly IMediator _mediator;

	private readonly LabRequisitionViewModel _labRequisition;

	
	private object _itemsLock = new object();

	public LabReportViewModel(IMediator mediator, ICbcTestResultReader reader)
	{
		_mediator = mediator;

		//TODO: Refactor
		NewCommand = new AsyncCommand(CreateAsync);
		SaveCommand = new AsyncCommand(SaveAsync);
		NextCommand = new AsyncCommand(GetNextAsync);
		PreviousCommand = new AsyncCommand(GetPreviousAsync);

		_labRequisition = new LabRequisitionViewModel();

		_labRequisition.Date = DateOnly.FromDateTime(DateTime.Now);
		
		var getLastLabTestReportQuery = new GetLastLabTestReportQuery(_labRequisition.Date);
		
		// var report = _mediator.Send(getLastLabTestReportQuery).GetAwaiter().GetResult();
		// _labRequisition.SetLabRequisition(report);
		
		BindingOperations.EnableCollectionSynchronization(CompleteBloodCounts, _itemsLock);

		Task.Run(async () =>

		{
			//get the data from the previous task a continue the execution on the UI thread

			while (true)
			{
				var cbc = await reader.ReadAsync();
				var completeBloodCountViewModel = new CompleteBloodCountViewModel(cbc);

				CompleteBloodCounts.Add(completeBloodCountViewModel);
			}
		});
		
	}

	public LabRequisitionViewModel LabRequisition
	{
		get => _labRequisition;
	}

	public ObservableCollection<CompleteBloodCountViewModel> CompleteBloodCounts { get; private set; } = [];
	
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