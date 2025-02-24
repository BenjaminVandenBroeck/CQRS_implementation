using NodaTime;

namespace CQRS_Example.Common.EventStore
{
    public abstract class Entity
    {
        protected Entity() { }

        public Guid Id { get; set; }
        public Instant LastUpdatedOn { get; set; }
        public string LastUpdatedBy {  get; set; }
    }
}
