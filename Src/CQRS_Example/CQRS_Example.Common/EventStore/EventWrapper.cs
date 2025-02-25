using NodaTime;

namespace CQRS_Example.Common.EventStore
{
    public class EventWrapper<T> where T : IEvent
    {
        public string Id { get; set; }
        public string AggregateId { get; set; }
        public string CorrelationId { get; set; }
        public int Version { get; set; }
        public Instant CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public T Event { get; set; }
    }
}
