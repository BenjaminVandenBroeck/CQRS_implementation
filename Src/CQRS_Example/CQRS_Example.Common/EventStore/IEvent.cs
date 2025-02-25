using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common.EventStore
{
    public interface IEvent
    {
        void Apply(IAggregateRoot aggregateRoot);
    }

    public interface IEvent<in T> : IEvent where T : IAggregateRoot
    {
        void Apply(T aggregateRoot);
    }
}
