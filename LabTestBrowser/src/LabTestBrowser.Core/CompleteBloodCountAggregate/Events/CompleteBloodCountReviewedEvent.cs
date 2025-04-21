namespace LabTestBrowser.Core.CompleteBloodCountAggregate.Events;

public class CompleteBloodCountReviewedEvent(int completeBloodCountId) : DomainEventBase
{
	public int CompleteBloodCountId { get; init; } = completeBloodCountId;
}