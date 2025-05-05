using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using ModernWpf.Controls;

namespace LabTestBrowser.UI;

public class ValidationBehavior: Behavior<AutoSuggestBox>
{
	public Func<string, string> Validator { get; set; } = null!;

	protected override void OnAttached()
	{
		base.OnAttached();
		AssociatedObject.LostFocus += OnLostFocus;
	}

	protected override void OnDetaching()
	{
		base.OnDetaching();
		AssociatedObject.LostFocus -= OnLostFocus;
	}

	private void OnLostFocus(object sender, RoutedEventArgs e)
	{
		var error = Validator?.Invoke(AssociatedObject.Text);
		if (!string.IsNullOrEmpty(error))
		{
			AssociatedObject.BorderBrush = Brushes.Red;
			AssociatedObject.ToolTip = error;
		}
		else
		{
			AssociatedObject.ClearValue(Control.BorderBrushProperty);
			AssociatedObject.ClearValue(Control.ToolTipProperty);
		}
	}
}