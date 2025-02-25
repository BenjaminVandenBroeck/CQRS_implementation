using CQRS_Example.Common.EventStore;
using Microsoft.Azure.Cosmos;
using static Microsoft.Azure.Cosmos.Container;

namespace CQRS_Example.Database
{
    public interface ICosmosService
    {
        Container GetEventsContainer(string? containerName = null);
        Container GetLeaseContainer();
        Container GetReadContainer(string? containerName = null );
        Container GetUniqueChecksContainer(string? containerName = null);
        ChangeFeedProcessor GetChangeFeedProcessor<T>(string processorName, ChangesHandler<EventWrapper<T>> onChangesDelegate, string? containerName= null) where T:IEvent ;

        CosmosLinqSerializerOptions LinqSerializerOptions { get; }
    }
}
