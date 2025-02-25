using CQRS_Example.Common.EventStore;
using CQRS_Example.Common.ExceptionHandling;
using CQRS_Example.Domain.Aggregates.Events.v1;

namespace CQRS_Example.Domain.Aggregates
{
    public class Employee : AggregateRoot<EmployeeEvent>
    {
        private string ErrorIdentifier = nameof(Employee);
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public EmployeeLevel Level { get; set; }

        public Employee() { }

        public static Employee Register(string employeeId, string firstName, string lastName, string? email, EmployeeLevel level)
        {
            var employee = new Employee();
            employee.Causes(new EmployeeRegistered(employeeId,firstName, lastName, email, level));
            return employee;
        }

        public void ChangeMail(string? newMail)
        {
            if (string.IsNullOrWhiteSpace(newMail))
            {
                Causes(new MailRemoved(Email));
                return;
            }

            if (string.IsNullOrWhiteSpace(Email) || !Email.Equals(newMail))
            {
                Causes(new MailChanged(Email, newMail));
            }
        }

        public void Promote(EmployeeLevel newLevel)
        {
            if(Level > newLevel)
            {
                throw new BaseException("You cannot promote a {currentLevel} to a lower level of {newLevel}", ErrorIdentifier, Level, newLevel);
            }

            Causes(new EmployeePromoted(Level,newLevel));
        }
    }
}
