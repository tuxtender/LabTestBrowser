using System.Collections.ObjectModel;
using System.Windows.Data;
using AsyncAwaitBestPractices.MVVM;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Events;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UI.Dialogs;
using LabTestBrowser.UI.Dialogs.ReportTemplateDialog;
using LabTestBrowser.UseCases.CompleteBloodCounts.Create;
using LabTestBrowser.UseCases.CompleteBloodCounts.Get;
using LabTestBrowser.UseCases.CompleteBloodCounts.GetCreated;
using LabTestBrowser.UseCases.LabTestReports;
using LabTestBrowser.UseCases.LabTestReports.GetEmpty;
using LabTestBrowser.UseCases.LabTestReports.GetLast;
using LabTestBrowser.UseCases.LabTestReports.GetNext;
using LabTestBrowser.UseCases.LabTestReports.GetPrevious;
using LabTestBrowser.UseCases.LabTestReports.RemoveCompleteBloodCount;
using LabTestBrowser.UseCases.LabTestReports.Save;
using LabTestBrowser.UseCases.LabTestReportTemplates;
using MediatR;

namespace LabTestBrowser.UI;

public class LabReportViewModel : BaseViewModel
{
	private readonly IMediator _mediator;
	private readonly ILogger<LabReportViewModel> _logger;
	private readonly ILabTestReportTemplateQueryService _labTestReportTemplateQueryService;

	private readonly LabRequisitionViewModel _labRequisition;

	
	private object _itemsLock = new object();

	public LabReportViewModel(IMediator mediator,
		ILogger<LabReportViewModel>  logger, 
		ILabTestReportTemplateQueryService  labTestReportTemplateQueryService,
		DialogViewModel dialogViewModel)
	{
		_mediator = mediator;
		_logger = logger;
		_labTestReportTemplateQueryService = labTestReportTemplateQueryService;
		DialogViewModel = dialogViewModel;

		//TODO: Refactor
		NewCommand = new AsyncCommand(CreateAsync);
		SaveCommand = new AsyncCommand(SaveAsync);
		NextCommand = new AsyncCommand(GetNextAsync);
		PreviousCommand = new AsyncCommand(GetPreviousAsync);
		ClearCommand = new AsyncCommand(ClearAsync);
		ExportCommand = new AsyncCommand(ExportAsync);

		_labRequisition = new LabRequisitionViewModel();
		_labRequisition.Specimen = 1;
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
	public AsyncCommand ClearCommand { get; private set; }
	public AsyncCommand ExportCommand { get; private set; }

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
			AgeInDays = _labRequisition.AgeInDays,
			CompleteBloodCountId = SelectedCompleteBloodCount?.Id,
		};

		var result = await _mediator.Send(saveLabTestReportCommand);
		
		if(result.IsSuccess)
			_labRequisition.Id = result.Value;
	}

	private async Task ClearAsync()
	{
		var command = new RemoveCompleteBloodCountCommand(_labRequisition.Id);
		var result = await _mediator.Send(command);
	}

	private async Task ExportAsync() 
	{
		string facility = "ИП Живодеров";
		string tradeName = "Зооскинхэд";
		string animal = "Кошка";
		
		var reportTemplates = await _labTestReportTemplateQueryService.ListAsync(facility, tradeName, animal);
		
		var vm = new ReportTemplateDialogViewModel();
		var input = new ReportTemplateDialogInput
		{
			ReportTemplates = reportTemplates
		};
		
		var dialogOutput = await DialogViewModel.ShowAsync(vm, input);
		
		var allTemplates = await _labTestReportTemplateQueryService.ListAsync();
		
		input = new ReportTemplateDialogInput
		{
			ReportTemplates = allTemplates
		};
		
		dialogOutput = await DialogViewModel.ShowAsync(vm, input);
	}
}