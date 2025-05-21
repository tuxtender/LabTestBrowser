namespace LabTestBrowser.Infrastructure.Data;

public class NoOpDomainEventDispatcher : IDomainEventDispatcher
{
	public Task DispatchAndClearEvents(IEnumerable<IHasDomainEvents> entitiesWithEvents) => Task.CompletedTask;
}