using LabTestBrowser.Desktop.Dialogs;
using LabTestBrowser.Desktop.LabResult.LabRequisition;
using LabTestBrowser.Desktop.Navigation;
using LabTestBrowser.Desktop.Notification;
using LabTestBrowser.Desktop.SearchLabResult;
using CompleteBloodCountViewModel = LabTestBrowser.Desktop.LabResult.CompleteBloodCount.CompleteBloodCountViewModel;
using LabResultViewModel = LabTestBrowser.Desktop.LabResult.LabResultViewModel;
using MainWindowViewModel = LabTestBrowser.Desktop.Navigation.MainWindowViewModel;
using ReportExportDialogViewModel = LabTestBrowser.Desktop.LabResult.ReportExportDialog.ReportExportDialogViewModel;
using StatusBarViewModel = LabTestBrowser.Desktop.Notification.StatusBarViewModel;

namespace LabTestBrowser.Desktop.Configurations;

public static class PresentationConfigs
{
	public static IServiceCollection AddPresentationConfigs(this IServiceCollection services)
	{
		services.AddSingleton<MainWindowViewModel>();
		services.AddSingleton<DialogViewModel>();
		services.AddSingleton<ReportExportDialogViewModel>();
		services.AddSingleton<LabRequisitionViewModel>();
		services.AddSingleton<CompleteBloodCountViewModel>();
		services.AddSingleton<StatusBarViewModel>();
		services.AddSingleton<INotificationService, NotificationService>();
		services.AddSingleton<INavigationService, NavigationService>();
		services.AddSingleton<LabResultViewModel>();
		services.AddSingleton<SearchLabResultViewModel>();

		return services;
	}
}