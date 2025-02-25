using CQRS_Example.Common.EventStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Domain.Aggregates
{
    public abstract class EmployeeEvent : IEvent<Employee>
    {
        public abstract void Apply(Employee aggregate);

        public void Apply(IAggregateRoot aggregate)
        {
            Apply((Employee)aggregate);
        }
    }
}
