using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;
using ModernWpf.Controls;

namespace LabTestBrowser.UI;

public class NumberBoxCommandBehavior: Behavior<NumberBox>
{
	public static readonly DependencyProperty CommandProperty =
		DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(NumberBoxCommandBehavior));

	public ICommand Command
	{
		get => (ICommand)GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

	private bool _isUserInteraction;

	protected override void OnAttached()
	{
		base.OnAttached();

		AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
		AssociatedObject.GotFocus += OnGotFocus;
		AssociatedObject.LostFocus += OnLostFocus;
		AssociatedObject.ValueChanged += OnValueChanged;
	}



	protected override void OnDetaching()
	{
		base.OnDetaching();

		AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
		AssociatedObject.GotFocus -= OnGotFocus;
		AssociatedObject.LostFocus -= OnLostFocus;
		AssociatedObject.ValueChanged -= OnValueChanged;
	}

	private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e) => _isUserInteraction = true;
	private void OnGotFocus(object sender, RoutedEventArgs e) => _isUserInteraction = true;
	private void OnLostFocus(object sender, RoutedEventArgs e) => _isUserInteraction = false;

	private void OnValueChanged(object sender, NumberBoxValueChangedEventArgs e)
	{
		if (_isUserInteraction && Command?.CanExecute((int)e.NewValue) == true)
		{
			Command.Execute((int)e.NewValue);
		}
	}
}