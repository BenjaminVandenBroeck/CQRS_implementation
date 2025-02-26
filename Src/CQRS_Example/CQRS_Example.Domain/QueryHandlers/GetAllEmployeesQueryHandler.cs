using CQRS_Example.Common.CQRS;
using CQRS_Example.Common.ReadStore;
using CQRS_Example.Domain.Queries;
using CQRS_Example.Domain.ReadModels;
using Microsoft.Extensions.Logging;
namespace CQRS_Example.Domain.QueryHandlers
{
    public class GetAllEmployeesQueryHandler : IQueryHandler<GetAllEmployeesQuery, PagedResult<EmployeeModel>>
    {
        private readonly IReadStoreRepository<EmployeeModel> _readStoreRepository;
        private readonly ILogger<GetAllEmployeesQueryHandler> _logger; 

        public GetAllEmployeesQueryHandler(IReadStoreRepository<EmployeeModel> readStoreRepository, ILogger<GetAllEmployeesQueryHandler> logger)
        {
            _readStoreRepository = readStoreRepository;
            _logger = logger;
        }

        public async Task<PagedResult<EmployeeModel>> HandleAsync(GetAllEmployeesQuery query)
        {
            var queryable = _readStoreRepository.GetQueryable();

            var pagedResult = await queryable.ToPagedResult(query);
            return pagedResult;
        }
    }
}
