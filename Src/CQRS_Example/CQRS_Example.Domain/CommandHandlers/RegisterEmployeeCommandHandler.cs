using CQRS_Example.Common.CQRS;
using CQRS_Example.Common.EventStore;
using CQRS_Example.Common.Logging;
using CQRS_Example.Domain.Aggregates;
using CQRS_Example.Domain.Commands;
using Microsoft.Extensions.Logging;

namespace CQRS_Example.Domain.CommandHandlers
{
    public class RegisterEmployeeCommandHandler : ICommandHandler<RegisterEmployeeCommand>
    {
        private readonly IEventStoreRepository<Employee, EmployeeEvent> _eventStoreRepository;
        private readonly ILogger<RegisterEmployeeCommandHandler> _logger;
        private const string ErrorIdentifier = nameof(RegisterEmployeeCommandHandler);

        public RegisterEmployeeCommandHandler(IEventStoreRepository<Employee, EmployeeEvent> eventStoreRepository
            , ILogger<RegisterEmployeeCommandHandler> logger)
        {
            _eventStoreRepository = eventStoreRepository;
            _logger = logger;
        }


        public async Task HandleAsync(RegisterEmployeeCommand command)
        {
            var employee = await _eventStoreRepository.GetByIdAsync(command.EmployeeId.ToString());

            if(employee!= null)
            {
                throw _logger.LogAndThrowDomainError("An employee with id {employeeId} has already been found", ErrorIdentifier, command.EmployeeId);
            }

            var newEmployee = Employee.Register(command.EmployeeId, command.FirstName, command.LastName, command.Email, command.Level);
            await _eventStoreRepository.SaveAsync(newEmployee, command.correlationId);
        }
    }
}
