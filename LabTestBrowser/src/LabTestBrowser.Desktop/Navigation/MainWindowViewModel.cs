using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabTestBrowser.Desktop.Notification;

namespace LabTestBrowser.Desktop.Navigation;

public partial class MainWindowViewModel(INavigationService navigation, StatusBarViewModel statusBar)
	: ObservableObject
{
	public INavigationService Navigation { get; } = navigation;
	public StatusBarViewModel StatusBar { get; } = statusBar;

	[RelayCommand]
	private void NavigateTo(Type vmType)
	{
		Navigation.NavigateTo(vmType);
	}
}