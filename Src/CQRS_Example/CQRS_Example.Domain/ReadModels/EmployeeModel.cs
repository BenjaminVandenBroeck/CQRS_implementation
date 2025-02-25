using CQRS_Example.Common.ReadStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Domain.ReadModels
{
    public class EmployeeModel: ReadModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public EmployeeLevelModel Level { get; set; }
    }
}
