using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LabTestBrowser.UI.LabResult.Messages;
using LabTestBrowser.UI.Notification;
using LabTestBrowser.UseCases.CompleteBloodCounts;
using LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;
using LabTestBrowser.UseCases.CompleteBloodCounts.ListReviewed;
using LabTestBrowser.UseCases.CompleteBloodCounts.ListUnderReview;
using LabTestBrowser.UseCases.CompleteBloodCounts.ResetReview;
using LabTestBrowser.UseCases.CompleteBloodCounts.Review;
using LabTestBrowser.UseCases.CompleteBloodCounts.Suppress;
using MediatR;

namespace LabTestBrowser.UI.LabResult.CompleteBloodCount;

using Localizations = Resources.Strings;

public partial class CompleteBloodCountViewModel : ObservableObject, IRecipient<LabOrderChangedMessage>
{
	private readonly Lazy<Task> _runUpdateTask;
	private readonly object _completeBloodCountLock = new();
	private readonly IMediator _mediator;
	private readonly IGetUpdatedCompleteBloodCountsUseCase _getUpdatedCompleteBloodCountsUseCase;
	private readonly INotificationService _notificationService;
	private readonly ILogger<CompleteBloodCountViewModel> _logger;
	private CompleteBloodCountItemViewModel? _selectedCompleteBloodCount;

	private bool _isExternalUpdate;

	public CompleteBloodCountViewModel(IMediator mediator, IGetUpdatedCompleteBloodCountsUseCase updatedCompleteBloodCountsUseCase,
		INotificationService notificationService, ILogger<CompleteBloodCountViewModel> logger)
	{
		_mediator = mediator;
		_getUpdatedCompleteBloodCountsUseCase = updatedCompleteBloodCountsUseCase;
		_notificationService = notificationService;
		_logger = logger;
		_runUpdateTask = new Lazy<Task>(RunUpdateAsync());

		WeakReferenceMessenger.Default.Register(this, LabOrderSyncToken.FromPrimary);
	}

	public ObservableCollection<CompleteBloodCountItemViewModel> CompleteBloodCounts { get; } = [];

	public CompleteBloodCountItemViewModel? SelectedCompleteBloodCount
	{
		get => _selectedCompleteBloodCount;
		set => SetProperty(ref _selectedCompleteBloodCount, value);
	}

	public Task LoadAsync() => _runUpdateTask.Value;

	public async void Receive(LabOrderChangedMessage message)
	{
		try
		{
			await UpdateExternal(message);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to update complete blood count");
		}
	}

	[RelayCommand]
	private async Task AssignAsync()
	{
		var labOrder = await WeakReferenceMessenger.Default.Send<LabOrderRequestMessage>();
		var command = new ReviewCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id, labOrder.Number, labOrder.Date);
		var result = await _mediator.Send(command);
		var notification = result.ToNotification();

		if (result.IsSuccess)
			notification = result.ToNotification(Localizations.LabReport_TestReported);

		await _notificationService.PublishAsync(notification);
	}

	[RelayCommand]
	private async Task ResetAsync()
	{
		var command = new ResetCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id);
		var result = await _mediator.Send(command);
		var notification = result.ToNotification();

		if (result.IsSuccess)
			notification = result.ToNotification(Localizations.LabReport_TestReset);

		await _notificationService.PublishAsync(notification);
	}

	[RelayCommand]
	private async Task SuppressAsync()
	{
		var labOrder = await WeakReferenceMessenger.Default.Send<LabOrderRequestMessage>();
		var command = new SuppressCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id, labOrder.Date);
		var result = await _mediator.Send(command);
		var notification = result.ToNotification();

		if (result.IsSuccess)
			notification = result.ToNotification(Localizations.LabReport_TestSuppressed);

		await _notificationService.PublishAsync(notification);
	}

	private void Select(int labOrderNumber)
	{
		if (labOrderNumber == SelectedCompleteBloodCount?.LabOrderNumber)
			return;

		var selectedCompleteBloodCount = CompleteBloodCounts.FirstOrDefault(cbc => cbc.LabOrderNumber == labOrderNumber);
		SelectedCompleteBloodCount = selectedCompleteBloodCount;
	}

	[RelayCommand]
	private void Notify()
	{
		if (_isExternalUpdate)
			return;

		var labOrderNumber = SelectedCompleteBloodCount?.LabOrderNumber;
		var labOrderDate = SelectedCompleteBloodCount?.LabOrderDate;
		if (!labOrderNumber.HasValue || !labOrderDate.HasValue)
			return;

		var labOrder = new LabOrder(labOrderNumber.Value, labOrderDate.Value);
		WeakReferenceMessenger.Default.Send(new LabOrderChangedMessage(labOrder), LabOrderSyncToken.FromSecondary);
	}

	private async Task UpdateExternal(LabOrderChangedMessage message)
	{
		_isExternalUpdate = true;
		var (labOrderNumber, labOrderDate) = message.Value;
		await UpdateAsync(labOrderDate);
		Select(labOrderNumber);
		_isExternalUpdate = false;
	}

	private async Task UpdateAsync(DateOnly labOrderDate)
	{
		if (labOrderDate == SelectedCompleteBloodCount?.LabOrderDate)
			return;

		var reviewedCompleteBloodCountsQuery = new ListReviewedCompleteBloodCountsQuery(labOrderDate);
		var underReviewCompleteBloodCountsQuery = new ListUnderReviewCompleteBloodCountsQuery();
		var queryResults = await Task.WhenAll(_mediator.Send(reviewedCompleteBloodCountsQuery),
			_mediator.Send(underReviewCompleteBloodCountsQuery));
		var completeBloodCounts = queryResults.SelectMany(queryResult => queryResult.Value);

		CompleteBloodCounts.Clear();
		completeBloodCounts.ToList().ForEach(cbc => CompleteBloodCounts.Add(new CompleteBloodCountItemViewModel(cbc)));
	}

	private Task RunUpdateAsync()
	{
		BindingOperations.EnableCollectionSynchronization(CompleteBloodCounts, _completeBloodCountLock);
		_ = Task.Run(async () =>
		{
			try
			{
				await UpdateCompleteBloodCountsAsync(_getUpdatedCompleteBloodCountsUseCase.ExecuteAsync());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error updating complete blood count");
			}
		});

		return Task.CompletedTask;
	}

	private async Task UpdateCompleteBloodCountsAsync(IAsyncEnumerable<CompleteBloodCountDto> completeBloodCounts,
		CancellationToken cancellationToken = default)
	{
		await foreach (var completeBloodCount in completeBloodCounts.WithCancellation(cancellationToken))
		{
			var completeBloodCountViewModel = new CompleteBloodCountItemViewModel(completeBloodCount);
			var updatingCompleteBloodCountViewModel = CompleteBloodCounts.FirstOrDefault(cbc => cbc.Id == completeBloodCount.Id);

			if (updatingCompleteBloodCountViewModel == null)
				CompleteBloodCounts.Add(completeBloodCountViewModel);
			else
				updatingCompleteBloodCountViewModel.Update(completeBloodCount);
		}
	}
}