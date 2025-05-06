using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LabTestBrowser.UI.Dialogs;
using LabTestBrowser.UI.Dialogs.ReportExportDialog;
using LabTestBrowser.UI.Notification;
using LabTestBrowser.UI.RequestMessages;

namespace LabTestBrowser.UI;

using Localizations = Resources.Strings;

public partial class LabReportViewModel : ObservableObject
{
	private readonly ReportExportDialogViewModel _reportExportDialog;
	private readonly INotificationService _notificationService;

	public LabReportViewModel(INotificationService notificationService,
		ReportExportDialogViewModel reportExportDialog,
		LabRequisitionViewModel labRequisition,
		CompleteBloodCountViewModel completeBloodCountViewModel,
		DialogViewModel dialogViewModel,
		StatusBarViewModel statusBar,
		ILogger<LabReportViewModel> logger)
	{
		_reportExportDialog = reportExportDialog;
		DialogViewModel = dialogViewModel;
		StatusBar = statusBar;
		LabRequisition = labRequisition;
		CompleteBloodCount = completeBloodCountViewModel;
		_notificationService = notificationService;
	}

	public LabRequisitionViewModel LabRequisition { get; }
	public CompleteBloodCountViewModel CompleteBloodCount { get; }
	public DialogViewModel DialogViewModel { get; private set; }
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
		// await LabRequisition.SaveAsync();
		// await CompleteBloodCount.AssignAsync();
		
		await Task.WhenAll(LabRequisition.SaveAsync(), CompleteBloodCount.AssignAsync());
		
		// WeakReferenceMessenger.Default.Send(new SaveRequestedMessage());
		// return Task.CompletedTask;
	}

	[RelayCommand]
	private async Task ExportAsync()
	{
		var labTestReportId = await WeakReferenceMessenger.Default.Send<LabTestReportIdRequestMessage>();
		var dialogInput = new ReportExportDialogInput(labTestReportId);
		await DialogViewModel.ShowAsync(_reportExportDialog, dialogInput);
	}

	[RelayCommand]
	private async Task LoadAsync()
	{
		var notification = new NotificationMessage
		{
			Title = Localizations.LabReport_Loading
		};
		await _notificationService.PublishAsync(notification);

		// await LabRequisition.LoadAsync();
		CompleteBloodCount.Load();

		notification = new NotificationMessage
		{
			Title = Localizations.LabReport_Idle
		};
		await _notificationService.PublishAsync(notification);
	}
}