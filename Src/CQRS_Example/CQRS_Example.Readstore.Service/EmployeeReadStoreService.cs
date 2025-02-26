
using CQRS_Example.Common.EventStore;
using CQRS_Example.Common.ReadStore;
using CQRS_Example.Database;
using CQRS_Example.Domain.Aggregates;
using CQRS_Example.Domain.ReadModels;
using Microsoft.Azure.Cosmos;

namespace CQRS_Example.Readstore.Service
{
    public class EmployeeReadStoreService : IHostedService
    {
        private readonly ILogger<EmployeeReadStoreService> _logger;
        private readonly ICosmosService _cosmosService;
        private readonly IEventStoreRepository<Employee, EmployeeEvent> _eventStoreRepository;
        private readonly IReadStoreRepository<EmployeeModel> _readStoreRepository;
        private ChangeFeedProcessor? _changeFeedProcessor;

        public EmployeeReadStoreService(ILogger<EmployeeReadStoreService> logger,
            ICosmosService cosmosService,
            IEventStoreRepository<Employee, EmployeeEvent> eventStoreRepository,
            IReadStoreRepository<EmployeeModel> readStoreRepository)
        {
            _logger = logger;
            _cosmosService = cosmosService;
            _eventStoreRepository = eventStoreRepository;
            _readStoreRepository = readStoreRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await PollyExtensions.ExecuteChangeFeedPolicy(async () =>
            {
                _changeFeedProcessor = _cosmosService.GetChangeFeedProcessor<EmployeeEvent>(nameof(EmployeeReadStoreService), HandleChangesAsync);
                await _changeFeedProcessor.StartAsync();
            }, _logger);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_changeFeedProcessor != null)
            {
                await _changeFeedProcessor.StopAsync();
            }
        }

        private async Task HandleChangesAsync(IReadOnlyCollection<EventWrapper<EmployeeEvent>> changes, CancellationToken token)
        {
            foreach (var change in changes)
            {
                using (_logger.BeginScope(new Dictionary<string, object> { { "CorrelationId", change.CorrelationId } }))
                {
                    try
                    {
                        var employee = await _eventStoreRepository.GetByIdAsync(change.AggregateId);
                        if (employee != null)
                        {
                            var employeeModel = new EmployeeModel
                            {
                                Id = employee.Id,
                                Email = employee.Email,
                                EmployeeId = employee.EmployeeId,
                                FirstName = employee.FirstName,
                                LastName = employee.LastName,
                                Level = (EmployeeLevelModel)Enum.Parse(typeof(EmployeeLevelModel), employee.Level.ToString())
                            };
                            await _readStoreRepository.SaveAsync(employeeModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error happening with event {eventId}", change.Id);
                        throw;
                    }
                }
            }
        }
    }
}
