using LabTestBrowser.Core.CompleteBloodCountAggregate.Events;
using LabTestBrowser.Core.Interfaces;

namespace LabTestBrowser.Core.CompleteBloodCountAggregate.Handlers;

internal class CompleteBloodCountReviewedHandler(
	ICompleteBloodCountUpdateChannel updateChannel,
	ILogger<CompleteBloodCountReviewedHandler> logger) : INotificationHandler<CompleteBloodCountReviewedEvent>
{
	public async Task Handle(CompleteBloodCountReviewedEvent domainEvent, CancellationToken cancellationToken)
	{
		await updateChannel.WriteAsync(domainEvent.CompleteBloodCountId);
		logger.LogTrace("CBC: {completeBloodCountId} reviewed", domainEvent.CompleteBloodCountId);
	}
}