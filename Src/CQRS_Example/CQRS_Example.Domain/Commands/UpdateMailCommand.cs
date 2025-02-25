using CQRS_Example.Common.CQRS;

namespace CQRS_Example.Domain.Commands
{
    public class UpdateMailCommand: Command
    {
        public required string EmployeeId {  get; set; }
        public string? Email { get; set; }
    }
}
