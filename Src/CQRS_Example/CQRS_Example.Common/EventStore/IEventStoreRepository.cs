
namespace CQRS_Example.Common.EventStore
{
    public interface IEventStoreRepository<TEntity, TEvent>
        where TEntity : AggregateRoot<TEvent>, new()
        where TEvent: IEvent
    {
        Task<TEntity?> GetByIdAsync(string Id, string? containerName = null);
        Task SaveAsync(TEntity entity, string correlationId, string? containerName = null);
    }
}
