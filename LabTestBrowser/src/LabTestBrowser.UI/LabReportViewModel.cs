using AsyncAwaitBestPractices.MVVM;
using LabTestBrowser.UseCases.LabTestReports.GetAvailable;
using MediatR;

namespace LabTestBrowser.UI;

public class LabReportViewModel : BaseViewModel
{
	private readonly IMediator _mediator;

	private readonly LabRequisitionViewModel _labRequisitionViewModel;

	public LabReportViewModel(IMediator mediator)
	{
		_mediator = mediator;

		NewCommand = new AsyncCommand(Create);
		_labRequisitionViewModel = new LabRequisitionViewModel();
	}

	public AsyncCommand NewCommand { get; private set; }
	// public AsyncCommand SaveCommand { get; private set; }
	// public AsyncCommand PreviousCommand { get; private set; }
	// public AsyncCommand NextCommand { get; private set; }

	private async Task Create()
	{
		var date = DateOnly.FromDateTime(DateTime.Now);

		var getAvailableLabTestReportQuery = new GetAvailableLabTestReportQuery(date);
		var result = await _mediator.Send(getAvailableLabTestReportQuery);

		_labRequisitionViewModel.SetLabRequisition(result.Value);
	}
}