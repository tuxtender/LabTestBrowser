using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LabTestBrowser.Desktop.Dialogs;
using LabTestBrowser.Desktop.Notification;
using LabTestBrowser.UseCases.Export;
using LabTestBrowser.UseCases.LabTestReportTemplates.List;
using LabTestBrowser.UseCases.LabTestReportTemplates.ListRegistered;
using MediatR;

namespace LabTestBrowser.Desktop.LabResult.ReportExportDialog;

using Localizations = UI.Resources.Strings;

public partial class ReportExportDialogViewModel : ObservableObject,
	IDialogContentViewModel<ReportExportDialogInput, ReportExportDialogOutput>
{
	private readonly IMediator _mediator;
	private readonly INotificationService _notificationService;
	private TaskCompletionSource<ReportExportDialogOutput>? _tcs;
	private IReadOnlyCollection<LabTestReportTemplateViewModel> _reportTemplates = [];
	private int? _labTestReportId;
	private bool _isFiltered;

	public ReportExportDialogViewModel(IMediator mediator, INotificationService notificationService)
	{
		_mediator = mediator;
		_notificationService = notificationService;
	}

	public IReadOnlyCollection<LabTestReportTemplateViewModel> LabTestReportTemplates
	{
		get => _reportTemplates;
		private set => SetProperty(ref _reportTemplates, value);
	}

	public bool IsFilterEnabled
	{
		get => _isFiltered;
		set => SetProperty(ref _isFiltered, value);
	}

	public async Task InitializeAsync(ReportExportDialogInput parameters,
		TaskCompletionSource<ReportExportDialogOutput> taskCompletionSource)
	{
		_labTestReportId = parameters.LabTestReportId;
		_tcs = taskCompletionSource;
		IsFilterEnabled = true;
		LabTestReportTemplates = await GetTemplatesAsync(parameters.LabTestReportId);
	}

	private async Task<List<LabTestReportTemplateViewModel>> GetTemplatesAsync(int? labTestReportTemplateId)
	{
		var query = new ListRegisteredLabTestReportTemplatesQuery(labTestReportTemplateId);
		var result = await _mediator.Send(query);
		if (!result.IsSuccess)
		{
			await _notificationService.PublishAsync(result.ToNotification(Localizations.LabReport_ExportFailed));
			var dialogOutput = new ReportExportDialogOutput(ReportExportDialogResult.Error);
			_tcs?.SetResult(dialogOutput);
			return [];
		}

		var labTestReportTemplates = result.Value
			.Select(template => new LabTestReportTemplateViewModel
			{
				Id = template.Id,
				Path = template.Path,
				Title = template.Title
			})
			.ToList();

		return labTestReportTemplates;
	}

	[RelayCommand]
	private async Task ExportAsync()
	{
		var templateIds = LabTestReportTemplates
			.Where(template => template.IsSelected)
			.Select(template => template.Id);

		var command = new ExportLabTestReportCommand(_labTestReportId, templateIds);
		var result = await _mediator.Send(command);
		var notification = result.ToNotification();
		var dialogOutput = new ReportExportDialogOutput(ReportExportDialogResult.Error);

		if (result.IsSuccess)
		{
			notification = result.ToNotification(Localizations.LabReport_ReportExported);
			dialogOutput = new ReportExportDialogOutput(ReportExportDialogResult.Ok);
		}

		await _notificationService.PublishAsync(notification);
		_tcs?.SetResult(dialogOutput);
	}

	[RelayCommand]
	private async Task FilterAsync()
	{
		if (IsFilterEnabled)
		{
			LabTestReportTemplates = await GetTemplatesAsync(_labTestReportId);
			return;
		}

		var query = new ListLabTestReportTemplatesQuery(null, null);
		var result = await _mediator.Send(query);

		LabTestReportTemplates = result.Value
			.Select(template => new LabTestReportTemplateViewModel
			{
				Id = template.Id,
				Path = template.Path,
				Title = template.Title
			})
			.ToList();
	}

	[RelayCommand]
	private void Cancel()
	{
		var dialogOutput = new ReportExportDialogOutput(ReportExportDialogResult.Cancel);
		_tcs?.SetResult(dialogOutput);
	}
}