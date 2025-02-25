using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common.CQRS
{
    public abstract class Command
    {
        public required string correlationId { get; set; }
    }
}
