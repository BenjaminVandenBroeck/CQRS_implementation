using CQRS_Example.Common.CQRS;
using CQRS_Example.Common.EventStore;
using CQRS_Example.Common.ExceptionHandling;
using CQRS_Example.Common.Logging;
using CQRS_Example.Domain.Aggregates;
using CQRS_Example.Domain.Commands;
using Microsoft.Extensions.Logging;

namespace CQRS_Example.Domain.CommandHandlers
{
    public class UpdateMailCommandHandler : ICommandHandler<UpdateMailCommand>
    {
        private readonly IEventStoreRepository<Employee, EmployeeEvent> _eventStoreRepository;
        private readonly ILogger<UpdateMailCommandHandler> _logger;
        private const string ErrorIdentifier= nameof(UpdateMailCommandHandler);

        public UpdateMailCommandHandler(IEventStoreRepository<Employee, EmployeeEvent> eventStoreRepository
            , ILogger<UpdateMailCommandHandler> logger)
        {
            _eventStoreRepository = eventStoreRepository;
            _logger = logger;
        }

        public async Task HandleAsync(UpdateMailCommand command)
        {
            var employee = await _eventStoreRepository.GetByIdAsync(command.EmployeeId);

            if (employee== null)
            {
                throw _logger.LogAndThrowDomainError("No employee with Id {EmployeeId} found", ErrorIdentifier, ExceptionType.NotFound, command.EmployeeId);
            }

            employee.ChangeMail(command.Email);
            await _eventStoreRepository.SaveAsync(employee, command.correlationId);
        }
    }
}
