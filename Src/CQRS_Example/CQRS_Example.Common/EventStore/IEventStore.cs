namespace CQRS_Example.Common.EventStore
{
    public interface IEventStore<T> where T : IEvent
    {
        Task<IEnumerable<EventWrapper<T>>> GetWrappedEventsForAggregateAsync(string aggregateId, string? containerName = null);
        Task SaveEventsAsync(string aggregateId, string correlationId, IReadOnlyCollection<T> events, int expectedVersion, string? containerName = null, string? uniqueNessContainerName = null);
    }
}
