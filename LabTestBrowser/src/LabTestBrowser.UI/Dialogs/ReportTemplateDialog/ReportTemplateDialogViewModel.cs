using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabTestBrowser.UseCases.LabTestReports;

namespace LabTestBrowser.UI.Dialogs.ReportTemplateDialog;

public class ReportTemplateDialogViewModel : ObservableObject,
	IDialogContentViewModel<ReportTemplateDialogInput, ReportTemplateDialogOutput>
{
	private TaskCompletionSource<ReportTemplateDialogOutput>? _tcs;
	private List<LabTestReportTemplateViewModel> _reportTemplates = [];

	public ReportTemplateDialogViewModel()
	{
		FilterCommand = new RelayCommand(Filter);
		CancelCommand = new RelayCommand(Cancel);
	}

	public ICommand? FilterCommand { get; private set; } = null;
	public ICommand? CancelCommand { get; private set; } = null;

	public List<LabTestReportTemplateViewModel> LabTestReportTemplates
	{
		get => _reportTemplates;
		set => SetProperty(ref _reportTemplates, value);
	}

	public void Initialize(ReportTemplateDialogInput parameters, TaskCompletionSource<ReportTemplateDialogOutput> taskCompletionSource)
	{
		LabTestReportTemplates = parameters.ReportTemplates
			.Select(template => new LabTestReportTemplateViewModel
			{
				Id = template.Id,
				Path = template.Path,
				Title = template.Title
			})
			.ToList();

		_tcs = taskCompletionSource;
	}

	private void Filter()
	{
		var templates = LabTestReportTemplates
			.Where(template => template.IsSelected)
			.Select(vm => new LabTestReportTemplate
			{
				Id = vm.Id,
				Path = vm.Path,
				Title = vm.Title,
			})
			.ToList();

		var dialogOutput = new ReportTemplateDialogOutput
		{
			DialogResult = ReportTemplateDialogResult.Ok,
			ReportTemplates = templates
		};

		_tcs?.SetResult(dialogOutput);
	}

	private void Cancel()
	{
		var dialogOutput = new ReportTemplateDialogOutput
		{
			DialogResult = ReportTemplateDialogResult.Cancel
		};

		_tcs?.SetResult(dialogOutput);
	}
}