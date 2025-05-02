using System.Collections.ObjectModel;
using System.Windows.Data;
using Ardalis.Result;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabTestBrowser.Core.CompleteBloodCountAggregate.Events;
using LabTestBrowser.Core.LabTestReportAggregate;
using LabTestBrowser.UI.Dialogs;
using LabTestBrowser.UI.Dialogs.ReportExportDialog;
using LabTestBrowser.UI.Notification;
using LabTestBrowser.UseCases.CompleteBloodCounts;
using LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;
using LabTestBrowser.UseCases.CompleteBloodCounts.ListReviewed;
using LabTestBrowser.UseCases.CompleteBloodCounts.ListUnderReview;
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

using Localizations = Resources.Strings;

public class LabReportViewModel : ObservableObject
{
	private readonly IMediator _mediator;
	private readonly ReportExportDialogViewModel _reportExportDialog;

	private readonly LabRequisitionViewModel _labRequisition;
	private readonly INotificationService _notificationService;

	private readonly object _completeBloodCountLock = new();
	private CompleteBloodCountViewModel? _selectedCompleteBloodCount;

	public LabReportViewModel(IMediator mediator,
		IGetUpdatedCompleteBloodCountsUseCase getUpdatedCompleteBloodCountsUseCase,
		INotificationService notificationService, 
		ReportExportDialogViewModel reportExportDialog,
		LabRequisitionViewModel labRequisition,
		DialogViewModel dialogViewModel,
		StatusBarViewModel statusBar)
	{
		_mediator = mediator;
		_reportExportDialog = reportExportDialog;
		DialogViewModel = dialogViewModel;
		StatusBar = statusBar;

		//TODO: Refactor
		NewCommand = new AsyncRelayCommand(CreateAsync);
		SaveCommand = new AsyncRelayCommand(SaveAsync);
		NextCommand = new AsyncRelayCommand(GetNextAsync);
		PreviousCommand = new AsyncRelayCommand(GetPreviousAsync);
		ResetCommand = new AsyncRelayCommand(ResetAsync);
		ExportCommand = new AsyncRelayCommand(ExportAsync);
		UpdateCommand = new AsyncRelayCommand(UpdateAsync);
		UpdateReportCommand = new AsyncRelayCommand(UpdateReportAsync); //TODO: CommandParameter
		UpdateByTestResultSelectCommand = new AsyncRelayCommand(UpdateReportByTestResultSelectAsync);
		SuppressCommand = new AsyncRelayCommand(SuppressAsync);
		AssignCommand = new AsyncRelayCommand(AssignAsync);

		_labRequisition = labRequisition;
		_notificationService = notificationService;
		_labRequisition.LabOrderDate = DateOnly.FromDateTime(DateTime.Now);
		UpdateAsync().GetAwaiter().GetResult();

		BindingOperations.EnableCollectionSynchronization(CompleteBloodCounts, _completeBloodCountLock);
		Task.Run(async () => await UpdateCompleteBloodCountsAsync(getUpdatedCompleteBloodCountsUseCase.ExecuteAsync())); 
	}

	public LabRequisitionViewModel LabRequisition
	{
		get => _labRequisition;
	}

	public DialogViewModel DialogViewModel { get; private set; }
	public StatusBarViewModel StatusBar { get; private set; }

	public ObservableCollection<CompleteBloodCountViewModel> CompleteBloodCounts { get; private set; } = [];

	public CompleteBloodCountViewModel? SelectedCompleteBloodCount
	{
		get => _selectedCompleteBloodCount;
		set => SetProperty(ref _selectedCompleteBloodCount, value);
	}

	public IAsyncRelayCommand NewCommand { get; private set; }
	public IAsyncRelayCommand SaveCommand { get; private set; }
	public IAsyncRelayCommand PreviousCommand { get; private set; }
	public IAsyncRelayCommand NextCommand { get; private set; }
	public IAsyncRelayCommand ResetCommand { get; private set; }
	public IAsyncRelayCommand ExportCommand { get; private set; }
	public IAsyncRelayCommand UpdateCommand { get; private set; }
	public IAsyncRelayCommand UpdateReportCommand { get; private set; }
	public IAsyncRelayCommand SuppressCommand { get; private set; }
	public IAsyncRelayCommand AssignCommand { get; private set; }
	public IAsyncRelayCommand UpdateByTestResultSelectCommand { get; private set; }

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

	private async Task UpdateCompleteBloodCountsAsync(IAsyncEnumerable<CompleteBloodCountDto> completeBloodCounts,
		CancellationToken cancellationToken = default)
	{
		await foreach (var completeBloodCount in completeBloodCounts.WithCancellation(cancellationToken))
		{
			var completeBloodCountViewModel = new CompleteBloodCountViewModel(completeBloodCount);
			var updatingCompleteBloodCountViewModel = CompleteBloodCounts.FirstOrDefault(cbc => cbc.Id == completeBloodCount.Id);

			if (updatingCompleteBloodCountViewModel != null)
				CompleteBloodCounts.Remove(updatingCompleteBloodCountViewModel);

			CompleteBloodCounts.Add(completeBloodCountViewModel);
		}
	}

	private async Task SaveAsync()
	{
		var saveLabTestReportCommand = new SaveLabTestReportCommand
		{
			Id = _labRequisition.Id,
			OrderNumber = _labRequisition.LabOrderNumber,
			OrderDate = _labRequisition.LabOrderDate,
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
			CompleteBloodCountId = SelectedCompleteBloodCount?.Id
		};

		var result = await _mediator.Send(saveLabTestReportCommand);
		var notification = result.ToNotification(Localizations.LabReport_ReportSavingFailed);

		if (result.IsSuccess)
		{
			_labRequisition.SetLabRequisition(result);
			notification = result.ToNotification(Localizations.LabReport_ReportSaved);
		}

		await _notificationService.PublishAsync(notification);
	}

	private async Task ResetAsync()
	{
		var command = new ResetCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id);
		var result = await _mediator.Send(command);
		var notification = result.ToNotification();

		if (result.IsSuccess)
			notification = result.ToNotification(Localizations.LabReport_TestReset);

		await _notificationService.PublishAsync(notification);
	}

	private async Task SuppressAsync()
	{
		var command = new SuppressCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id, _labRequisition.LabOrderDate);
		var result = await _mediator.Send(command);
		var notification = result.ToNotification();

		if (result.IsSuccess)
			notification = result.ToNotification(Localizations.LabReport_TestSuppressed);

		await _notificationService.PublishAsync(notification);
	}

	private async Task AssignAsync()
	{
		var command = new ReviewCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id, _labRequisition.LabOrderNumber,
			_labRequisition.LabOrderDate);
		var result = await _mediator.Send(command);
		var notification = result.ToNotification();

		if (result.IsSuccess)
			notification = result.ToNotification(Localizations.LabReport_TestReported);

		await _notificationService.PublishAsync(notification);
	}

	private async Task UpdateAsync()
	{
		var notification = new NotificationMessage
		{
			Title = Localizations.LabReport_Loading
		};
		await _notificationService.PublishAsync(notification);

		var reviewedCompleteBloodCountsQuery = new ListReviewedCompleteBloodCountsQuery(_labRequisition.LabOrderDate);
		var underReviewCompleteBloodCountsQuery = new ListUnderReviewCompleteBloodCountsQuery();
		var queryResults = await Task.WhenAll(_mediator.Send(reviewedCompleteBloodCountsQuery),
			_mediator.Send(underReviewCompleteBloodCountsQuery));
		var completeBloodCounts = queryResults.SelectMany(queryResult => queryResult.Value);

		CompleteBloodCounts.Clear();
		completeBloodCounts.ToList().ForEach(cbc => CompleteBloodCounts.Add(new CompleteBloodCountViewModel(cbc)));

		var reportQuery = new GetLastLabTestReportQuery(_labRequisition.LabOrderDate);
		var report = await _mediator.Send(reportQuery);
		_labRequisition.SetLabRequisition(report);

		notification = new NotificationMessage
		{
			Title = Localizations.LabReport_Idle
		};
		await _notificationService.PublishAsync(notification);
	}

	private async Task UpdateReportAsync()
	{
		var query = new GetLabTestReportQuery(_labRequisition.LabOrderNumber, _labRequisition.LabOrderDate);
		var report = await _mediator.Send(query);
		
		if(!report.IsSuccess)
			return;
		
		_labRequisition.SetLabRequisition(report);
		var selectedCompleteBloodCount = CompleteBloodCounts.FirstOrDefault(cbc => cbc.LabOrderNumber == _labRequisition.LabOrderNumber);
		SelectedCompleteBloodCount = selectedCompleteBloodCount;
	}

	private async Task UpdateReportByTestResultSelectAsync()
	{
		if (SelectedCompleteBloodCount == null)
			return;
		
		var labOrderNumber = SelectedCompleteBloodCount.LabOrderNumber;
		var labOrderDate = SelectedCompleteBloodCount.LabOrderDate;

		if (!labOrderNumber.HasValue || !labOrderDate.HasValue)
			return;

		var query = new GetLabTestReportQuery(labOrderNumber.Value, labOrderDate.Value);
		var report = await _mediator.Send(query);
		
		if(report.IsSuccess)
			_labRequisition.SetLabRequisition(report);
	}

	private async Task ExportAsync()
	{
		var dialogInput = new ReportExportDialogInput(_labRequisition.Id);
		await DialogViewModel.ShowAsync(_reportExportDialog, dialogInput);
	}
}