using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Domain.Aggregates.Events.v1
{
    public class MailRemoved : EmployeeEvent
    {
        private MailRemoved() { }

        public MailRemoved(string? oldMail)
        {
            OldMail = oldMail;
        }

        public string? OldMail { get; set; }

        public override void Apply(Employee aggregate)
        {
            aggregate.Email = null;
        }
    }
}
