using CQRS_Example.Common.EventStore;
using CQRS_Example.Domain.Aggregates.Events.v1.DTO;

namespace CQRS_Example.Domain.Aggregates.Events.v1
{
    public class EmployeeRegistered : EmployeeEvent
    {
        //Needed for serialization purposes
        private EmployeeRegistered() { }

        public EmployeeRegistered(string employeeId, string firstName, string lastName, string? email, EmployeeLevel level)
        {
            EmployeeId = employeeId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Level = EmployeeLevelMapper.GetEmployeeLevelEventDto(level);
        }

        [Unique]
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public EmployeeLevelEventDto Level { get; set; }

        public override void Apply(Employee aggregate)
        {
            aggregate.Id = EmployeeId;
            aggregate.EmployeeId = EmployeeId;
            aggregate.FirstName = FirstName;
            aggregate.LastName = LastName;
            aggregate.Email = Email;
            aggregate.Level = EmployeeLevelMapper.GetEmployeeLevel(Level);
        }
    }
}
