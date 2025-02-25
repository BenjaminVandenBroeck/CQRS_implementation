
using CQRS_Example.Common.CQRS;
using CQRS_Example.Common.EventStore;
using CQRS_Example.Common.ExceptionHandling;
using CQRS_Example.Common.Logging;
using CQRS_Example.Common.ReadStore;
using CQRS_Example.Domain.Aggregates;
using CQRS_Example.Domain.Commands;
using CQRS_Example.Domain.ReadModels;
using Microsoft.Extensions.Logging;

namespace CQRS_Example.Domain.CommandHandlers
{
    public class PromoteEmployeeCommandHandler : ICommandHandler<PromoteEmployeeCommand>
    {
        private readonly IEventStoreRepository<Employee, EmployeeEvent> _eventStoreRepository;
        private readonly IReadStoreRepository<EmployeeModel> _readStoreRepository;
        private readonly ILogger<PromoteEmployeeCommandHandler> _logger;
        private const string ErrorIdentifier = nameof(PromoteEmployeeCommandHandler);

        public PromoteEmployeeCommandHandler(IEventStoreRepository<Employee, EmployeeEvent> eventStoreRepository,
            IReadStoreRepository<EmployeeModel> readStoreRepository, 
            ILogger<PromoteEmployeeCommandHandler> logger)
        {
            _eventStoreRepository = eventStoreRepository;
            _readStoreRepository = readStoreRepository;
            _logger = logger;
        }

        public async Task HandleAsync(PromoteEmployeeCommand command)
        {
            var employee = await _eventStoreRepository.GetByIdAsync(command.EmployeeId);

            if (employee == null)
            {
                throw _logger.LogAndThrowDomainError("No employee with Id {EmployeeId} found", ErrorIdentifier, ExceptionType.NotFound, command.EmployeeId);
            }


            if(command.Level== EmployeeLevel.CEO)
            {
                var ceo = _readStoreRepository.GetQueryable().Where(x => x.Level == EmployeeLevelModel.CEO).FirstOrDefault();
                if (ceo != null)
                {
                    throw _logger.LogAndThrowDomainError("There can only be one CEO", ErrorIdentifier);
                }
            }

            employee.Promote(command.Level);
            await _eventStoreRepository.SaveAsync(employee, command.correlationId);
        }
    }
}