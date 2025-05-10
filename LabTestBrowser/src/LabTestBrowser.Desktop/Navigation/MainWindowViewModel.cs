using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabTestBrowser.Desktop.Dialogs;
using LabTestBrowser.Desktop.Notification;

namespace LabTestBrowser.Desktop.Navigation;

public partial class MainWindowViewModel(INavigationService navigation, StatusBarViewModel statusBar, DialogViewModel dialog)
	: ObservableObject
{
	public INavigationService Navigation { get; } = navigation;
	public DialogViewModel Dialog { get; } = dialog;
	public StatusBarViewModel StatusBar { get; } = statusBar;

	[RelayCommand]
	private void NavigateTo(Type vmType)
	{
		Navigation.NavigateTo(vmType);
	}
}