namespace CQRS_Example.Common.EventStore
{
    public abstract class AggregateRoot<T>: Entity, IAggregateRoot where T: IEvent
    {
        private readonly IList<T> _domainEvents = new List<T>();

        protected AggregateRoot() { }

        public int Version { get; private set; }

        public IReadOnlyCollection<T> GetDoaminEvents()
        {
            return _domainEvents.ToList();
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }

        public void LoadFromHistory(IEnumerable<T> domainEvents)
        {
            foreach(var domainEvent in domainEvents)
            {
                Apply(domainEvent);
            }
        }

        protected void Causes (T domainEvent)
        {
            _domainEvents.Add(domainEvent);
            Apply(domainEvent);
        }

        protected void Apply(T domainEvent)
        {
            domainEvent.Apply(this);
            Version++;
        }
    }
}
