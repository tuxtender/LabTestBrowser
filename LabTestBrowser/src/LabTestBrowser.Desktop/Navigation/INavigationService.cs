using CommunityToolkit.Mvvm.ComponentModel;

namespace LabTestBrowser.Desktop.Navigation;

public interface INavigationService
{
	ObservableObject ViewModel { get; }
	void NavigateTo<TViewModel>() where TViewModel : ObservableObject;
	void NavigateTo(Type vmType);
}