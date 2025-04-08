using CommunityToolkit.Mvvm.ComponentModel;

namespace LabTestBrowser.UI.Dialogs;

public class DialogViewModel : ObservableObject
{
	private object? _dialogContentViewModel;
	private bool _isVisible;

	public object? DialogContentViewModel
	{
		get => _dialogContentViewModel;
		set => SetProperty(ref _dialogContentViewModel, value);
	}

	public bool IsVisible
	{
		get => _isVisible;
		set => SetProperty(ref _isVisible, value);
	}

	public async Task<TOutput> ShowAsync<TInput, TOutput>(IDialogContentViewModel<TInput, TOutput> dialogContentViewModel, TInput input)
		where TInput : IDialogContentInput where TOutput : IDialogContentOutput
	{
		IsVisible = true;
		DialogContentViewModel = dialogContentViewModel;
		var taskCompletionSource = new TaskCompletionSource<TOutput>();
		dialogContentViewModel.Initialize(input, taskCompletionSource);
		var result = await taskCompletionSource.Task.WaitAsync(CancellationToken.None);
		IsVisible = false;

		return result;
	}
}