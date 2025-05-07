using System.Windows;

namespace LabTestBrowser.UI.Navigation;

public partial class MainWindow : Window
{
	public MainWindow(MainWindowViewModel viewModel)
	{
		InitializeComponent();
		DataContext = viewModel;
	}
}