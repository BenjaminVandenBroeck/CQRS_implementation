using CQRS_Example.Common.Logging;
using Microsoft.Extensions.Logging;

namespace CQRS_Example.Common.EventStore
{
    public class EventStoreRepository<TEntity, TEvent>: IEventStoreRepository<TEntity, TEvent>
        where TEntity : AggregateRoot<TEvent>, new()
        where TEvent : IEvent
    {
        protected readonly IEventStore<TEvent> _eventStore;
        protected readonly ILogger<EventStoreRepository<TEntity, TEvent>> _logger;
        public EventStoreRepository(IEventStore<TEvent> eventStore, ILogger<EventStoreRepository<TEntity, TEvent>> logger)
        {
            _eventStore = eventStore;
            _logger = logger;
        }

        public async Task<TEntity?> GetByIdAsync(string Id, string? containerName = null)
        {
            var wrappedEvents = await _eventStore.GetWrappedEventsForAggregateAsync(Id, containerName);

            if (!wrappedEvents.Any())
            {
                return null;
            }

            var entity = new TEntity();
            var events = wrappedEvents.Select(e => e.Event);
            entity.LoadFromHistory(events);
            var lastEvent= wrappedEvents.Last();
            entity.LastUpdatedOn = lastEvent.CreatedOn;
            entity.LastUpdatedBy = lastEvent.CreatedBy;

            return entity;
        }

        public async Task SaveAsync(TEntity entity, string correlationId, string? containerName = null)
        {
            var domainEvents = entity.GetDomainEvents();

            try
            {
                await _eventStore.SaveEventsAsync(entity.Id, correlationId, domainEvents, entity.Version - domainEvents.Count, containerName);
            }
            catch (UniquenessException ex)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw _logger.LogAndThrowDomainError("Exception saving aggregateRoot with id {aggregateId} and correlationId {correlationId). {exceptionType}: {innerExceptionMessage}",
                    ex,
                    nameof(EventStoreRepository<TEntity, TEvent>),
                    entity.Id, correlationId,ex.GetType().Name);
            }

            entity.ClearDomainEvents();
        }
    }
}
