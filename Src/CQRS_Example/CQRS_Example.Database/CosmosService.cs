using CQRS_Example.Common.EventStore;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace CQRS_Example.Database
{
    public class CosmosService : ICosmosService
    {
        private readonly CosmosOptions _options;
        private readonly Microsoft.Azure.Cosmos.Database _database;
        private readonly ILogger<CosmosService> _logger;

        public CosmosService(IOptions<CosmosOptions> options, ILogger<CosmosService> logger)
        {
            _logger = logger;
            _options = options.Value;
            var client = new CosmosClient(_options.ConnectionString, new CosmosClientOptions
            {
                Serializer = new JsonCosmosSerializer(new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb))
            });
            _database = client.GetDatabase(_options.DatabaseName);
        }

        public CosmosLinqSerializerOptions LinqSerializerOptions
        {
            get
            {
                return new CosmosLinqSerializerOptions
                {
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                };
            }
        }

        public ChangeFeedProcessor GetChangeFeedProcessor<T>(string processorName, Container.ChangesHandler<EventWrapper<T>> onChangesDelegate, string? containerName = null) where T: IEvent
        {
            var container = GetEventsContainer(containerName);
            var changeFeedProcessor = container.GetChangeFeedProcessorBuilder(processorName, onChangesDelegate)
                .WithErrorNotification(HandleChangeFeedProcessorException)
                .WithInstanceName(Guid.NewGuid().ToString())
                .WithLeaseContainer(GetLeaseContainer())
                .WithStartTime(DateTime.MinValue.ToUniversalTime())
                .Build();
            return changeFeedProcessor;
        }

        public Container GetEventsContainer(string? containerName = null)
        {
            return string.IsNullOrWhiteSpace(containerName) ? _database.GetContainer(_options.EventsContainerName) : _database.GetContainer(containerName);
        }

        public Container GetLeaseContainer()
        {
            return _database.GetContainer(_options.LeasesContainerName);
        }

        public Container GetReadContainer(string? containerName =null)
        {
            return string.IsNullOrWhiteSpace(containerName) ? _database.GetContainer(_options.ReadContainerName) : _database.GetContainer(containerName);
        }

        public Container GetUniqueChecksContainer(string? containerName = null)
        {
            return string.IsNullOrWhiteSpace(containerName) ? _database.GetContainer(_options.UniqueChecksContainerName) : _database.GetContainer(containerName);
        }

        private Task HandleChangeFeedProcessorException(string leaseToken, Exception exception)
        {
            if(exception is ChangeFeedProcessorUserException userException)
            {
                _logger.LogError(userException.InnerException, "Exception happened during changefeedprocessor with leaseToken {leaseToken}", leaseToken);
            }
            else
            {
                _logger.LogError(exception, "Exception happened during changefeedprocessor with leaseToken {leaseToken}", leaseToken);
            }
            return Task.CompletedTask;
        }
    }
}
