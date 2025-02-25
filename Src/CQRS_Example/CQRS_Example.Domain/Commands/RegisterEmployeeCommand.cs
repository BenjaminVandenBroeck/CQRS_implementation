using CQRS_Example.Common.CQRS;
using CQRS_Example.Domain.Aggregates;
namespace CQRS_Example.Domain.Commands
{
    public class RegisterEmployeeCommand: Command
    {
        public required string EmployeeId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? Email { get; set; }
        public required EmployeeLevel Level { get; set; }
    }
}
