using LabTestBrowser.Core.CompleteBloodCountAggregate.Events;
using LabTestBrowser.Core.Interfaces;

namespace LabTestBrowser.Core.CompleteBloodCountAggregate.Handlers;

internal class CompleteBloodCountReviewedHandler(
	ICompleteBloodCountUpdateNotifier notifier,
	ILogger<CompleteBloodCountReviewedHandler> logger) : INotificationHandler<CompleteBloodCountReviewedEvent>
{
	public async Task Handle(CompleteBloodCountReviewedEvent domainEvent, CancellationToken cancellationToken)
	{
		await notifier.NotifyAsync(domainEvent.CompleteBloodCountId);
		logger.LogInformation("Complete blood count id: {completeBloodCountId} is {reviewResult}", domainEvent.CompleteBloodCountId,
			domainEvent.ReviewResult.ToString());
	}
}