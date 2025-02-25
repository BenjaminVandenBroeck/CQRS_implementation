using CQRS_Example.Common.Logging;
using CQRS_Example.Common.ReadStore;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;

namespace CQRS_Example.Database
{
    public class CosmosReadStoreRepository<T> : IReadStoreRepository<T> where T : ReadModel
    {
        protected readonly ICosmosService _cosmosService;
        protected readonly ILogger<CosmosReadStoreRepository<T>> _logger;
        private const string ErrorIdentifier = nameof(CosmosReadStoreRepository<T>);

        public CosmosReadStoreRepository(ICosmosService cosmosService, ILogger<CosmosReadStoreRepository<T>> logger)
        {
            _logger = logger;
            _cosmosService = cosmosService;
        }

        public async Task<T?> GetByIdAsync(string id, string? containerName = null)
        {
            try
            {
                var response = await _cosmosService.GetReadContainer(containerName).ReadItemAsync<T>(id, new PartitionKey(typeof(T).Name));
                return response;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public IQueryable<T> GetQueryable(string? containerName = null)
        {
            return _cosmosService.GetReadContainer(containerName).GetItemLinqQueryable<T>(linqSerializerOptions: _cosmosService.LinqSerializerOptions, requestOptions: new QueryRequestOptions
            {
                PartitionKey = new PartitionKey(typeof(T).Name),
            });
        }

        public async Task SaveAsync(T entity)
        {
            try
            {
                var result = await _cosmosService.GetReadContainer().UpsertItemAsync(entity);
                if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    _logger.LogInformation("Inserted {readModelType} with Id {Id} in readstore", entity.GetType().Name, entity.Id);
                }
                else if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation("Updated {readModelType} with Id {Id} in readstore", entity.GetType().Name, entity.Id);
                }
            }
            catch(Exception ex)
            {
                throw _logger.LogAndThrowDomainError("Exception upserting {readModelType} with Id {Id} in readstore. {exceptionType}: {innerException}", ex, ErrorIdentifier, entity.GetType().Name, entity.Id, ex.GetType().Name);
            }
        }
    }
}
