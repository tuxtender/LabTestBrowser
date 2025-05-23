using CommunityToolkit.Mvvm.ComponentModel;

namespace LabTestBrowser.Desktop.Navigation;

public class NavigationService(IServiceProvider provider) : ObservableObject, INavigationService
{
	private ObservableObject _viewModel = null!;

	public ObservableObject ViewModel
	{
		get => _viewModel;
		private set => SetProperty(ref _viewModel, value);
	}

	public void NavigateTo<TViewModel>()
		where TViewModel : ObservableObject
	{
		ViewModel = provider.GetRequiredService<TViewModel>();
	}

	public void NavigateTo(Type vmType)
	{
		ViewModel = (ObservableObject)provider.GetRequiredService(vmType);
	}
}