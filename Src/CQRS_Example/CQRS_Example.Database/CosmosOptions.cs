using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Database
{
    public class CosmosOptions
    {
        public required string ConnectionString { get; set; }
        public required string DatabaseName { get; set; }

        public string EventsContainerName { get; set; } = "ContainerNotFound";
        public string ReadContainerName { get; set; } = "ContainerNotFound";
        public string LeasesContainerName { get; set; } = "Leases";
        public string UniqueChecksContainerName { get; set; } = "ContainerNotFound";
    }
}
