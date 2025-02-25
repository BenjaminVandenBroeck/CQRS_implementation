using CQRS_Example.Common.CQRS;
using CQRS_Example.Domain.Aggregates;

namespace CQRS_Example.Domain.Commands
{
    public class PromoteEmployeeCommand: Command
    {
        public required string EmployeeId { get; set; }
        public required EmployeeLevel Level { get; set; }
    }
}
