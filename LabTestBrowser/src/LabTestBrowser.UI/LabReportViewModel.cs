using System.Collections.ObjectModel;
using System.Windows.Data;
using AsyncAwaitBestPractices.MVVM;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Events;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UI.Dialogs;
using LabTestBrowser.UI.Dialogs.ReportTemplateDialog;
using LabTestBrowser.UseCases.CompleteBloodCounts;
using LabTestBrowser.UseCases.CompleteBloodCounts.Create;
using LabTestBrowser.UseCases.CompleteBloodCounts.Get;
using LabTestBrowser.UseCases.CompleteBloodCounts.GetCreated;
using LabTestBrowser.UseCases.CompleteBloodCounts.ResetReview;
using LabTestBrowser.UseCases.CompleteBloodCounts.Review;
using LabTestBrowser.UseCases.CompleteBloodCounts.Suppress;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.Export;
using LabTestBrowser.UseCases.LabTestReports.Get;
using LabTestBrowser.UseCases.LabTestReports.GetEmpty;
using LabTestBrowser.UseCases.LabTestReports.GetLast;
using LabTestBrowser.UseCases.LabTestReports.GetNext;
using LabTestBrowser.UseCases.LabTestReports.GetPrevious;
using LabTestBrowser.UseCases.LabTestReports.Save;
using LabTestBrowser.UseCases.LabTestReportTemplates;
using LabTestBrowser.UseCases.LabTestReportTemplates.List;
using LabTestBrowser.UseCases.LabTestReportTemplates.ListRegistered;
using MediatR;

namespace LabTestBrowser.UI;

public class LabReportViewModel : BaseViewModel
{
	private readonly IMediator _mediator;
	private readonly ILogger<LabReportViewModel> _logger;
	private readonly ReportTemplateDialogViewModel _reportTemplateDialog;

	private readonly LabRequisitionViewModel _labRequisition;

	
	private object _itemsLock = new object();

	public LabReportViewModel(IMediator mediator,
		ILogger<LabReportViewModel>  logger, 
		ReportTemplateDialogViewModel reportTemplateDialog,
		DialogViewModel dialogViewModel)
	{
		_mediator = mediator;
		_logger = logger;
		_reportTemplateDialog = reportTemplateDialog;
		DialogViewModel = dialogViewModel;

		//TODO: Refactor
		NewCommand = new AsyncCommand(CreateAsync);
		SaveCommand = new AsyncCommand(SaveAsync);
		NextCommand = new AsyncCommand(GetNextAsync);
		PreviousCommand = new AsyncCommand(GetPreviousAsync);
		ResetCommand = new AsyncCommand(ResetAsync);
		ExportCommand = new AsyncCommand(ExportAsync);
		UpdateCommand = new AsyncCommand(UpdateAsync);

		_labRequisition = new LabRequisitionViewModel();
		_labRequisition.LabOrderNumber = 1;
		_labRequisition.LabOrderDate = DateOnly.FromDateTime(DateTime.Now);
		
		var query = new GetLastLabTestReportQuery(_labRequisition.LabOrderDate);
		var report = _mediator.Send(query).GetAwaiter().GetResult();
		_labRequisition.SetLabRequisition(report);

		BindingOperations.EnableCollectionSynchronization(CompleteBloodCounts, _itemsLock);
		Task.Run(async () =>
		{
			//get the data from the previous task a continue the execution on the UI thread

			while (true)
			{
				await GetCompleteBloodCountAsync();
			}
		}); //TODO: Refactor using IObservable
	}

	public LabRequisitionViewModel LabRequisition
	{
		get => _labRequisition;
	}

	public Dialogs.DialogViewModel DialogViewModel { get; private set; }
	
	public ObservableCollection<CompleteBloodCountViewModel> CompleteBloodCounts { get; private set; } = [];
	public CompleteBloodCountViewModel? SelectedCompleteBloodCount { get; set; }

	public AsyncCommand NewCommand { get; private set; }

	public AsyncCommand SaveCommand { get; private set; }

	public AsyncCommand PreviousCommand { get; private set; }
	public AsyncCommand NextCommand { get; private set; }
	public AsyncCommand ResetCommand { get; private set; }
	public AsyncCommand ExportCommand { get; private set; }
	public AsyncCommand UpdateCommand { get; private set; }

	private async Task CreateAsync()
	{
		var getEmptyLabTestReportQuery = new GetEmptyLabTestReportQuery(_labRequisition.LabOrderDate);
		var result = await _mediator.Send(getEmptyLabTestReportQuery);
		_labRequisition.SetLabRequisition(result.Value);
	}

	private async Task GetNextAsync()
	{
		var getNextLabTestReportQuery = new GetNextLabTestReportQuery(_labRequisition.LabOrderNumber, _labRequisition.LabOrderDate);
		var result = await _mediator.Send(getNextLabTestReportQuery);
		_labRequisition.SetLabRequisition(result.Value);
	}

	private async Task GetPreviousAsync()
	{
		var getPreviousLabTestReportQuery = new GetPreviousLabTestReportQuery(_labRequisition.LabOrderNumber, _labRequisition.LabOrderDate);
		var result = await _mediator.Send(getPreviousLabTestReportQuery);
		_labRequisition.SetLabRequisition(result.Value);
	}

	private async Task GetCompleteBloodCountAsync() 
	{
		var completeBloodCount = await _mediator.Send(new GetCreatedCompleteBloodCountQuery());
		var completeBloodCountViewModel = new CompleteBloodCountViewModel(completeBloodCount.Value);
		CompleteBloodCounts.Add(completeBloodCountViewModel);
	}

	private async Task SaveAsync()
	{
		var saveLabTestReportCommand = new SaveLabTestReportCommand
		{
			Id = _labRequisition.Id,
			SequenceNumber = _labRequisition.LabOrderNumber,
			Date = _labRequisition.LabOrderDate,
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

		if (result.IsSuccess)
			_labRequisition.SetLabRequisition(result.Value);

		var reviewCompleteBloodCountCommand =
			new ReviewCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id, _labRequisition.LabOrderNumber, _labRequisition.LabOrderDate);
		result = await _mediator.Send(reviewCompleteBloodCountCommand);
	}

	private async Task ResetAsync()
	{
		var command = new ResetCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id);
		var result = await _mediator.Send(command);
	}

	private async Task SuppressAsync()
	{
		var command = new SuppressCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id, _labRequisition.LabOrderDate);
		var result = await _mediator.Send(command);
	}

	private async Task UpdateAsync()
	{
		var reportQuery = new GetLastLabTestReportQuery(_labRequisition.LabOrderDate);
		var report = await _mediator.Send(reportQuery);
		_labRequisition.SetLabRequisition(report);

		//TODO: Get reviewed, not reviewed and suppressed CBC
		
		// var completeBloodCountsQuery = new ListCompleteBloodCountsByDateQuery(_labRequisition.Date);
		// var cbcResult = await _mediator.Send(completeBloodCountsQuery);
		var completeBloodCounts = new List<CompleteBloodCountDto>();

		CompleteBloodCounts.Clear();
		completeBloodCounts.ToList().ForEach(cbc => CompleteBloodCounts.Add(new CompleteBloodCountViewModel(cbc)));
	}

	private async Task ExportAsync()
	{
		//TODO: Encapsulate export into ReportTemplateDialogViewModel
		var query = new ListRegisteredLabTestReportTemplatesQuery(_labRequisition.Id);
		var result = await _mediator.Send(query);

		if (!result.IsSuccess)
			return;

		var reportTemplates = result.Value;
		var dialogInput = new ReportTemplateDialogInput
		{
			ReportTemplates = reportTemplates
		};

		var dialogOutput = await DialogViewModel.ShowAsync(_reportTemplateDialog, dialogInput);

		if (dialogOutput.DialogResult == ReportTemplateDialogResult.Cancel)
			return;

		if(dialogOutput.ReportTemplates == null)
			return;
		
		var templateIds = dialogOutput.ReportTemplates.Select(template => template.Id);
		
		var command = new ExportLabTestReportCommand(_labRequisition.Id, templateIds);
		result = await _mediator.Send(command);
	}
}