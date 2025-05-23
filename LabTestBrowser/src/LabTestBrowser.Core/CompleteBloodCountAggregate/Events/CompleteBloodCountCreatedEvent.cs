namespace LabTestBrowser.Core.CompleteBloodCountAggregate.Events;

public class CompleteBloodCountCreatedEvent(int completeBloodCountId) : DomainEventBase
{
	public int Id { get; init; } = completeBloodCountId;
}