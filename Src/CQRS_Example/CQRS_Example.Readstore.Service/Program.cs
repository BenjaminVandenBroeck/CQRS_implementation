using CQRS_Example.Common;
using CQRS_Example.Common.EventStore;
using CQRS_Example.Common.ReadStore;
using CQRS_Example.Database;
using CQRS_Example.Readstore.Service;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<EmployeeReadStoreService>();


builder.Services.Configure<CosmosOptions>(builder.Configuration.GetSection("COSMOSDB_SETTINGS"));
builder.Services.AddSingleton<IInfoService, MachineInfoService>();
builder.Services.AddSingleton<ICosmosService, CosmosService>();
builder.Services.AddSingleton(typeof(IEventStore<>), typeof(CosmosEventStore<>));
builder.Services.AddSingleton(typeof(IEventStoreRepository<,>), typeof(EventStoreRepository<,>));
builder.Services.AddSingleton(typeof(IReadStoreRepository<>), typeof(CosmosReadStoreRepository<>));


var host = builder.Build();
host.Run();
