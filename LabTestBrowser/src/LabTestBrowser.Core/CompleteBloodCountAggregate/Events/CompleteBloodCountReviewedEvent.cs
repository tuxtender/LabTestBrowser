namespace LabTestBrowser.Core.CompleteBloodCountAggregate.Events;

public class CompleteBloodCountReviewedEvent(int completeBloodCountId, ReviewResult reviewResult) : DomainEventBase
{
	public int CompleteBloodCountId { get; init; } = completeBloodCountId;
	public ReviewResult ReviewResult { get; init; } = reviewResult;
}