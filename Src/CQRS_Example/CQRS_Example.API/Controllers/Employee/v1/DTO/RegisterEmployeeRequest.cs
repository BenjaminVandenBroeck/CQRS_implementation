using CQRS_Example.Domain.Aggregates.Events.v1.DTO;

namespace CQRS_Example.API.Controllers.Employee.v1.DTO
{
    public class RegisterEmployeeRequest
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public EmployeeLevel Level { get; set; }
    }
}
