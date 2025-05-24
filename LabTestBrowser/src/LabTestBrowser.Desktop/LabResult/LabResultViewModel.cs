using Ardalis.Result;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabTestBrowser.Desktop.Dialogs;
using LabTestBrowser.Desktop.LabResult.CompleteBloodCount;
using LabTestBrowser.Desktop.LabResult.LabRequisition;
using LabTestBrowser.Desktop.LabResult.ReportExportDialog;
using LabTestBrowser.Desktop.Notification;
using LabTestBrowser.UseCases.LabTestReports.GetEmpty;
using LabTestBrowser.UseCases.LabTestReports.GetNext;
using LabTestBrowser.UseCases.LabTestReports.GetPrevious;
using LabTestBrowser.UseCases.LabTestReports.Match;
using LabTestBrowser.UseCases.LabTestReports.Save;
using MediatR;

namespace LabTestBrowser.Desktop.LabResult;

public partial class LabResultViewModel : ObservableObject
{
	private readonly IMediator _mediator;
	private readonly ReportExportDialogViewModel _reportExportDialog;
	private readonly INotificationService _notificationService;
	private readonly DialogViewModel _dialog;

	public LabResultViewModel(IMediator mediator,
		INotificationService notificationService,
		ReportExportDialogViewModel reportExportDialog,
		LabRequisitionViewModel labRequisition,
		CompleteBloodCountViewModel completeBloodCountViewModel,
		DialogViewModel dialogViewModel)
	{
		_mediator = mediator;
		_reportExportDialog = reportExportDialog;
		_dialog = dialogViewModel;
		LabRequisition = labRequisition;
		CompleteBloodCount = completeBloodCountViewModel;
		_notificationService = notificationService;
	}

	public LabRequisitionViewModel LabRequisition { get; }
	public CompleteBloodCountViewModel CompleteBloodCount { get; }

	[RelayCommand]
	private async Task CreateAsync()
	{
		var getEmptyLabTestReportQuery = new GetEmptyLabTestReportQuery(LabRequisition.LabOrderDate);
		var result = await _mediator.Send(getEmptyLabTestReportQuery);
		LabRequisition.SetLabRequisition(result.Value);
	}

	[RelayCommand]
	private async Task NextAsync()
	{
		var getNextLabTestReportQuery = new GetNextLabTestReportQuery(LabRequisition.LabOrderNumber, LabRequisition.LabOrderDate);
		var result = await _mediator.Send(getNextLabTestReportQuery);
		LabRequisition.SetLabRequisition(result.Value);
	}

	[RelayCommand]
	private async Task PreviousAsync()
	{
		var getPreviousLabTestReportQuery = new GetPreviousLabTestReportQuery(LabRequisition.LabOrderNumber, LabRequisition.LabOrderDate);
		var result = await _mediator.Send(getPreviousLabTestReportQuery);
		LabRequisition.SetLabRequisition(result.Value);
	}

	[RelayCommand]
	private async Task SaveAsync()
	{
		var saveLabTestReportCommand = new SaveLabTestReportCommand
		{
			Id = LabRequisition.Id,
			OrderNumber = LabRequisition.LabOrderNumber,
			OrderDate = LabRequisition.LabOrderDate,
			Facility = LabRequisition.Facility,
			TradeName = LabRequisition.TradeName,
			PetOwner = LabRequisition.PetOwner,
			Nickname = LabRequisition.Nickname,
			Animal = LabRequisition.Animal,
			Category = LabRequisition.Category,
			Breed = LabRequisition.Breed,
			AgeInYears = LabRequisition.AgeInYears,
			AgeInMonths = LabRequisition.AgeInMonths,
			AgeInDays = LabRequisition.AgeInDays,
			CompleteBloodCountId = CompleteBloodCount.SelectedCompleteBloodCount?.Id
		};

		var result = await _mediator.Send(saveLabTestReportCommand);
		var notification = result.ToNotification("LabReport_ReportSavingFailed");

		if (result.IsSuccess)
		{
			LabRequisition.SetLabRequisition(result);
			notification = result.ToNotification("LabReport_ReportSaved");
		}

		await _notificationService.PublishAsync(notification);
	}

	[RelayCommand]
	private async Task ExportAsync()
	{
		var matchCommand = new MatchLabTestReportCommand
		{
			Id = LabRequisition.Id,
			OrderNumber = LabRequisition.LabOrderNumber,
			OrderDate = LabRequisition.LabOrderDate,
			Facility = LabRequisition.Facility,
			TradeName = LabRequisition.TradeName,
			PetOwner = LabRequisition.PetOwner,
			Nickname = LabRequisition.Nickname,
			Animal = LabRequisition.Animal,
			Category = LabRequisition.Category,
			Breed = LabRequisition.Breed,
			AgeInYears = LabRequisition.AgeInYears,
			AgeInMonths = LabRequisition.AgeInMonths,
			AgeInDays = LabRequisition.AgeInDays,
		};

		var result = await _mediator.Send(matchCommand);
		if (result.IsSuccess)
		{
			var dialogInput = new ReportExportDialogInput(LabRequisition.Id);
			await _dialog.ShowAsync(_reportExportDialog, dialogInput);
			return;
		}

		var notification = result.ToNotification();

		if (result.IsError())
			notification.Level = NotificationLevel.Warning;

		await _notificationService.PublishAsync(notification);
	}

	[RelayCommand]
	private async Task LoadAsync()
	{
		var notification = new NotificationMessage
		{
			Title = "LabReport_Loading"
		};
		await _notificationService.PublishAsync(notification);

		await LabRequisition.LoadAsync();
		await CompleteBloodCount.LoadAsync();

		notification = new NotificationMessage
		{
			Title = "LabReport_Idle"
		};
		await _notificationService.PublishAsync(notification);
	}
}