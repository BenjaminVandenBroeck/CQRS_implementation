
using CQRS_Example.Domain.Aggregates.Events.v1.DTO;

namespace CQRS_Example.Domain.Aggregates.Events.v1
{
    public class EmployeePromoted : EmployeeEvent
    {
        private EmployeePromoted() { }

        public EmployeePromoted(EmployeeLevel oldLevel, EmployeeLevel newLevel)
        {
            OldLevel= EmployeeLevelMapper.GetEmployeeLevelEventDto(oldLevel);
            NewLevel = EmployeeLevelMapper.GetEmployeeLevelEventDto(newLevel);
        }

        public EmployeeLevelEventDto OldLevel { get; set; }
        public EmployeeLevelEventDto NewLevel { get; set; }

        public override void Apply(Employee aggregate)
        {
            aggregate.Level= EmployeeLevelMapper.GetEmployeeLevel(NewLevel);
        }
    }
}
