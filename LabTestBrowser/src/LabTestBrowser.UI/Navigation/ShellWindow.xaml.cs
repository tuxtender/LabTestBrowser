using System.Windows;

namespace LabTestBrowser.UI.Navigation;

public partial class ShellWindow : Window
{
	public ShellWindow(ShellViewModel viewModel)
	{
		InitializeComponent();
		DataContext = viewModel;
	}
}