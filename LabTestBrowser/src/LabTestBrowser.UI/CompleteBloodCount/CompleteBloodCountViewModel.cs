using System.Collections.ObjectModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LabTestBrowser.UI.Notification;
using LabTestBrowser.UI.RequestMessages;
using LabTestBrowser.UseCases.CompleteBloodCounts;
using LabTestBrowser.UseCases.CompleteBloodCounts.GetUpdatedStream;
using LabTestBrowser.UseCases.CompleteBloodCounts.ListReviewed;
using LabTestBrowser.UseCases.CompleteBloodCounts.ListUnderReview;
using LabTestBrowser.UseCases.CompleteBloodCounts.ResetReview;
using LabTestBrowser.UseCases.CompleteBloodCounts.Review;
using LabTestBrowser.UseCases.CompleteBloodCounts.Suppress;
using MediatR;

namespace LabTestBrowser.UI;

using Localizations = Resources.Strings;

public partial class CompleteBloodCountViewModel : ObservableObject
{
	private readonly object _completeBloodCountLock = new();

	private bool _isExternalUpdated;
	
	private readonly IMediator _mediator;
	private readonly IGetUpdatedCompleteBloodCountsUseCase _getUpdatedCompleteBloodCountsUseCase;
	private readonly INotificationService _notificationService;
	private readonly ILogger<CompleteBloodCountViewModel> _logger;
	private CompleteBloodCountItemViewModel? _selectedCompleteBloodCount;

	public CompleteBloodCountViewModel(IMediator mediator, IGetUpdatedCompleteBloodCountsUseCase updatedCompleteBloodCountsUseCase,
		INotificationService notificationService, ILogger<CompleteBloodCountViewModel> logger)
	{
		_mediator = mediator;
		_getUpdatedCompleteBloodCountsUseCase = updatedCompleteBloodCountsUseCase;
		_notificationService = notificationService;
		_logger = logger;

		// WeakReferenceMessenger.Default.Register<LabOrderDateChangedMessage>(this, Update);
		// WeakReferenceMessenger.Default.Register<LabOrderNumberChangedMessage>(this, Select);
		
		// WeakReferenceMessenger.Default.Register<LabOrderChangedMessage>(this, Update);
		

		
		WeakReferenceMessenger.Default.Register<LabOrderChangedMessage>(this, Update);

		
	
	}

	

	public ObservableCollection<CompleteBloodCountItemViewModel> CompleteBloodCounts { get; } = [];

	public CompleteBloodCountItemViewModel? SelectedCompleteBloodCount
	{
		get => _selectedCompleteBloodCount;
		set => SetProperty(ref _selectedCompleteBloodCount, value);
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

	private async void Save(object recipient, SaveRequestedMessage message)
	{
		try
		{
			await AssignAsync();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to save complete blood count");
		}
	}
	
	[RelayCommand]
	public async Task AssignAsync()
	{
		var labOrder = await WeakReferenceMessenger.Default.Send<LabOrderRequestMessage>();
		var command = new ReviewCompleteBloodCountCommand(SelectedCompleteBloodCount?.Id, labOrder.Number, labOrder.Date);
		var result = await _mediator.Send(command);
		var notification = result.ToNotification();

		if (result.IsSuccess)
			notification = result.ToNotification(Localizations.LabReport_TestReported);

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
	private void Select()
	{
		if(_isExternalUpdated)
			return;
		
		var labOrderNumber = SelectedCompleteBloodCount?.LabOrderNumber;

		if (!labOrderNumber.HasValue)
			return;

		WeakReferenceMessenger.Default.Send(new LabOrderNumberChangedMessage(labOrderNumber.Value));
	}

	private async void Update(object recipient, LabOrderChangedMessage message)
	{
		try
		{
			_isExternalUpdated = true;
			await UpdateAsync(message.Value.Date);
			Select(message.Value.Number);
			_isExternalUpdated = false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to update complete blood count");
		}
	}

	private async Task UpdateAsync(DateOnly labOrderDate)
	{
		if(labOrderDate == SelectedCompleteBloodCount?.LabOrderDate)
			return;
		
		var reviewedCompleteBloodCountsQuery = new ListReviewedCompleteBloodCountsQuery(labOrderDate);
		var underReviewCompleteBloodCountsQuery = new ListUnderReviewCompleteBloodCountsQuery();
		var queryResults = await Task.WhenAll(_mediator.Send(reviewedCompleteBloodCountsQuery),
			_mediator.Send(underReviewCompleteBloodCountsQuery));
		var completeBloodCounts = queryResults.SelectMany(queryResult => queryResult.Value);

		CompleteBloodCounts.Clear();
		completeBloodCounts.ToList().ForEach(cbc => CompleteBloodCounts.Add(new CompleteBloodCountItemViewModel(cbc)));
		
	
	}

	[RelayCommand]
	public void Load()
	{
		BindingOperations.EnableCollectionSynchronization(CompleteBloodCounts, _completeBloodCountLock);
		Task.Run(async () =>
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
	}

	private async Task UpdateCompleteBloodCountsAsync(IAsyncEnumerable<CompleteBloodCountDto> completeBloodCounts,
		CancellationToken cancellationToken = default)
	{
		await foreach (var completeBloodCount in completeBloodCounts.WithCancellation(cancellationToken))
		{
			var completeBloodCountViewModel = new CompleteBloodCountItemViewModel(completeBloodCount);
			var updatingCompleteBloodCountViewModel = CompleteBloodCounts.FirstOrDefault(cbc => cbc.Id == completeBloodCount.Id);

			if (updatingCompleteBloodCountViewModel != null)
				CompleteBloodCounts.Remove(updatingCompleteBloodCountViewModel);

			CompleteBloodCounts.Add(completeBloodCountViewModel);
		}
	}
}