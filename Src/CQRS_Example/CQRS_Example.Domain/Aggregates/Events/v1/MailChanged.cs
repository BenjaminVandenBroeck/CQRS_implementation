using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Domain.Aggregates.Events.v1
{
    public class MailChanged : EmployeeEvent
    {
        private MailChanged() { }

        public MailChanged(string? oldMail, string newMail)
        {
            OldMail = oldMail;
            NewMail = newMail;
        }

        public string? OldMail { get; set; }
        public string? NewMail { get; set; }

        public override void Apply(Employee aggregate)
        {
            aggregate.Email = NewMail;
        }
    }
}
