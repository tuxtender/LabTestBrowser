using System.Windows;

namespace LabTestBrowser.Desktop.Navigation;

public partial class MainWindow : Window
{
	public MainWindow(MainWindowViewModel viewModel)
	{
		InitializeComponent();
		DataContext = viewModel;
	}
}