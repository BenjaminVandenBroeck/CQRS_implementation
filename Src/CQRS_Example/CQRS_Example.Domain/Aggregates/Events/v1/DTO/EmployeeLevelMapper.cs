
namespace CQRS_Example.Domain.Aggregates.Events.v1.DTO
{
    public static class EmployeeLevelMapper
    {
        public static EmployeeLevelEventDto GetEmployeeLevelEventDto(EmployeeLevel employeeLevel)
        {
            switch (employeeLevel)
            {
                case EmployeeLevel.Manager:
                    return EmployeeLevelEventDto.Manager;
                case EmployeeLevel.CEO:
                    return EmployeeLevelEventDto.CEO;
                case EmployeeLevel.Medior:
                    return EmployeeLevelEventDto.Medior;
                case EmployeeLevel.Senior:
                    return EmployeeLevelEventDto.Senior;
                case EmployeeLevel.Junior:
                default:
                    return EmployeeLevelEventDto.Junior;
            }
        }


        public static EmployeeLevel GetEmployeeLevel(EmployeeLevelEventDto employeeLevel)
        {
            switch (employeeLevel)
            {
                case EmployeeLevelEventDto.Manager:
                    return EmployeeLevel.Manager;
                case EmployeeLevelEventDto.CEO:
                    return EmployeeLevel.CEO;
                case EmployeeLevelEventDto.Medior:
                    return EmployeeLevel.Medior;
                case EmployeeLevelEventDto.Senior:
                    return EmployeeLevel.Senior;
                case EmployeeLevelEventDto.Junior:
                default:
                    return EmployeeLevel.Junior;
            }
        }
    }
}
