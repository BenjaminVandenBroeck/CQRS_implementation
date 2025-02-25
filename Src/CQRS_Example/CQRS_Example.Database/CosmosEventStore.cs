using CQRS_Example.Common;
using CQRS_Example.Common.EventStore;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Logging;
using NodaTime;

namespace CQRS_Example.Database
{
    public class CosmosEventStore<T> : IEventStore<T> where T : IEvent
    {
        private readonly ICosmosService _cosmosService;
        private readonly IInfoService _infoService;
        private readonly ILogger<CosmosEventStore<T>> _logger;

        public CosmosEventStore(ICosmosService cosmosService, IInfoService infoService, ILogger<CosmosEventStore<T>> logger)
        {
            _cosmosService = cosmosService;
            _infoService = infoService;
            _logger = logger;
        }

        public async Task<IEnumerable<EventWrapper<T>>> GetWrappedEventsForAggregateAsync(string aggregateId, string? containerName = null)
        {
            var result = new List<EventWrapper<T>>();
            var feed = _cosmosService.GetEventsContainer(containerName).GetItemLinqQueryable<EventWrapper<T>>(linqSerializerOptions: _cosmosService.LinqSerializerOptions)
                .Where(x => x.AggregateId == aggregateId).OrderBy(x => x.Version).ToFeedIterator();
            while (feed.HasMoreResults)
            {
                var response = await feed.ReadNextAsync();
                result.AddRange(response);
            }

            return result;

        }

        public async Task SaveEventsAsync(string aggregateId, string correlationId, IReadOnlyCollection<T> events, int expectedVersion, string? containerName = null, string? uniqueNessContainerName = null)
        {
            if (!events.Any())
            {
                _logger.LogInformation("No events to save");
                return;
            }

            var committedEvents = await GetWrappedEventsForAggregateAsync(aggregateId);
            var currentCommitedVersion = committedEvents.LastOrDefault()?.Version ?? 0;
            if (currentCommitedVersion != expectedVersion)
            {
                throw new ConcurrencyException("Expected version does not match current commited version");
            }
            var i = expectedVersion;
            var uniqueChecksContainer = _cosmosService.GetUniqueChecksContainer();
            var eventsContainer = _cosmosService.GetEventsContainer(containerName);
            var uniquenessTransactionLog = new Dictionary<string, string>();

            try
            {
                var batch = eventsContainer.CreateTransactionalBatch(new Microsoft.Azure.Cosmos.PartitionKey(aggregateId));
                foreach (var @event in events)
                {
                    await CheckUniquePropertiesAsync(@event, uniqueChecksContainer, uniquenessTransactionLog);
                    var wrapper = new EventWrapper<T>
                    {
                        Id = Guid.NewGuid().ToString(),
                        AggregateId = aggregateId,
                        CorrelationId = correlationId,
                        Event = @event,
                        Version = ++i,
                        CreatedOn = SystemClock.Instance.GetCurrentInstant(),
                        CreatedBy = _infoService.Name
                    };
                    batch.CreateItem(wrapper);
                }

                var response = await batch.ExecuteAsync();
                if (!response.IsSuccessStatusCode)
                {
                    foreach (var result in response)
                    {
                        if (result.StatusCode == System.Net.HttpStatusCode.Conflict)
                        {
                            throw new ConcurrencyException("Event version already exists for this aggregate");
                        }
                    }
                    _logger.LogError("Failed to save events for aggregate {aggregateId}", aggregateId);
                    throw new Exception("Failed to save events");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception happened when saving events");
                await RollBackUniqueTransactions(uniquenessTransactionLog, uniqueChecksContainer, aggregateId);
                throw;
            }
        }


        private async Task CheckUniquePropertiesAsync(T @event, Container uniqueChecksContainer, Dictionary<string, string> transactionlog)
        {
            var uniqueProperties = @event.GetType().GetProperties().Where(x => x.IsDefined(typeof(UniqueAttribute), true));
            foreach(var property in  uniqueProperties)
            {
                var uniqueValue = property.GetValue(@event).ToString();
                try
                {
                    var uniqueCheck = new UniqueCheck
                    {
                        Id = Guid.NewGuid().ToString(),
                        PropertyName = property.Name,
                        UniqueValue = uniqueValue.ToLowerInvariant()
                    };

                    await uniqueChecksContainer.CreateItemAsync(uniqueCheck);
                    transactionlog.Add(uniqueCheck.Id, uniqueCheck.PropertyName);
                }
                catch(CosmosException ex) when (ex.StatusCode== System.Net.HttpStatusCode.Conflict)
                {
                    var exceptionMessage = string.Format("Value {0} is not unique for event property with name {1}", uniqueValue,property.Name);
                    throw new UniquenessException(exceptionMessage, property.Name, uniqueValue, ex);
                }
            }
        }

        private async Task RollBackUniqueTransactions(Dictionary<string, string> transactionLog, Container uniquenessContainer, string aggregateId)
        {
            _logger.LogError("Rolling back transaction for aggregate {aggregateId}", aggregateId);

            foreach(var transaction in transactionLog)
            {
                try
                {
                    await uniquenessContainer.DeleteItemAsync<UniqueCheck>(transaction.Key, new PartitionKey(transaction.Value));
                }
                catch(Exception ex) 
                {
                    _logger.LogError("Failed to delete unique check with id {uniquenessProperty} and value {uniquenessPropertyValue} while rolling back transaction", transaction.Key, transaction.Value);
                }
            }
            _logger.LogError("Rolled back transaction for aggregate {aggregateId}", aggregateId);
        }
    }
}
