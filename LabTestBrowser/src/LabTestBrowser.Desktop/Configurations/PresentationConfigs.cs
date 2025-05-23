using LabTestBrowser.Desktop.LabResult;
using LabTestBrowser.Desktop.LabResult.CompleteBloodCount;
using LabTestBrowser.Desktop.LabResult.LabRequisition;
using LabTestBrowser.Desktop.Navigation;
using LabTestBrowser.Desktop.Notification;
using LabTestBrowser.Desktop.SearchLabResult;

namespace LabTestBrowser.Desktop.Configurations;

public static class PresentationConfigs
{
	public static IServiceCollection AddPresentationConfigs(this IServiceCollection services)
	{
		services.AddSingleton<MainWindowViewModel>();
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