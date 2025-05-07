using LabTestBrowser.UI.Dialogs;
using LabTestBrowser.UI.LabResult;
using LabTestBrowser.UI.LabResult.CompleteBloodCount;
using LabTestBrowser.UI.LabResult.ReportExportDialog;
using LabTestBrowser.UI.Navigation;
using LabTestBrowser.UI.Notification;
using LabTestBrowser.UI.SearchLabResult;

namespace LabTestBrowser.UI.Configurations;

public static class PresentationConfigs
{
	public static IServiceCollection AddPresentationConfigs(this IServiceCollection services)
	{
		services.AddSingleton<MainWindowViewModel>();
		services.AddSingleton<DialogViewModel>();
		services.AddSingleton<ReportExportDialogViewModel>();
		services.AddSingleton<LabResult.LabRequisition.LabRequisitionViewModel>();
		services.AddSingleton<CompleteBloodCountViewModel>();
		services.AddSingleton<StatusBarViewModel>();
		services.AddSingleton<INotificationService, NotificationService>();
		services.AddSingleton<INavigationService, NavigationService>();
		services.AddSingleton<LabResultViewModel>();
		services.AddSingleton<SearchLabResultViewModel>();

		return services;
	}
}