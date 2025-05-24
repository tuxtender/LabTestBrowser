namespace LabTestBrowser.Desktop.Dialogs;

public interface IDialogContentViewModel<TInput, TOutput>
	where TInput : IDialogContentInput where TOutput : IDialogContentOutput
{
	Task InitializeAsync(TInput parameters, TaskCompletionSource<TOutput> taskCompletionSource);
}