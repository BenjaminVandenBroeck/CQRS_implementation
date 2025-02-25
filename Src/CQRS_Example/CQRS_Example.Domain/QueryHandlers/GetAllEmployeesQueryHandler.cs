using CQRS_Example.Common.CQRS;
using CQRS_Example.Domain.Aggregates;
using CQRS_Example.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Domain.QueryHandlers
{
    public class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery, IList<Employee>>
    {
        public Task<IList<Employee>> HandleAsync(GetAllEmployeesQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
