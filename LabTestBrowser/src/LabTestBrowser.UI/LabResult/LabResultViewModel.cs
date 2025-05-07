using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabTestBrowser.UI.Dialogs;
using LabTestBrowser.UI.LabResult.ReportExportDialog;
using LabTestBrowser.UI.Notification;
using CompleteBloodCountViewModel = LabTestBrowser.UI.LabResult.CompleteBloodCount.CompleteBloodCountViewModel;
using ReportExportDialogViewModel = LabTestBrowser.UI.LabResult.ReportExportDialog.ReportExportDialogViewModel;

namespace LabTestBrowser.UI.LabResult;

using Localizations = Resources.Strings;

public partial class LabResultViewModel : ObservableObject
{
	private readonly ReportExportDialogViewModel _reportExportDialog;
	private readonly INotificationService _notificationService;
	private readonly DialogViewModel _dialog;

	public LabResultViewModel(INotificationService notificationService,
		ReportExportDialogViewModel reportExportDialog,
		LabRequisition.LabRequisitionViewModel labRequisition,
		CompleteBloodCountViewModel completeBloodCountViewModel,
		DialogViewModel dialogViewModel,
		StatusBarViewModel statusBar)
	{
		_reportExportDialog = reportExportDialog;
		_dialog = dialogViewModel;
		StatusBar = statusBar;
		LabRequisition = labRequisition;
		CompleteBloodCount = completeBloodCountViewModel;
		_notificationService = notificationService;
	}

	public LabRequisition.LabRequisitionViewModel LabRequisition { get; }
	public CompleteBloodCountViewModel CompleteBloodCount { get; }
	public StatusBarViewModel StatusBar { get; private set; }

	[RelayCommand]
	private async Task CreateAsync()
	{
		await LabRequisition.CreateAsync();
	}

	[RelayCommand]
	private async Task NextAsync()
	{
		await LabRequisition.NextAsync();
	}

	[RelayCommand]
	private async Task PreviousAsync()
	{
		await LabRequisition.PreviousAsync();
	}

	[RelayCommand]
	private async Task SaveAsync()
	{
		await LabRequisition.SaveAsync();
		await CompleteBloodCount.AssignAsync();
	}

	[RelayCommand]
	private async Task ExportAsync()
	{
		var dialogInput = new ReportExportDialogInput(LabRequisition.Id);
		await _dialog.ShowAsync(_reportExportDialog, dialogInput);
	}

	[RelayCommand]
	private async Task LoadAsync()
	{
		var notification = new NotificationMessage
		{
			Title = Localizations.LabReport_Loading
		};
		await _notificationService.PublishAsync(notification);

		await LabRequisition.LoadAsync();
		await CompleteBloodCount.LoadAsync();

		notification = new NotificationMessage
		{
			Title = Localizations.LabReport_Idle
		};
		await _notificationService.PublishAsync(notification);
	}
}