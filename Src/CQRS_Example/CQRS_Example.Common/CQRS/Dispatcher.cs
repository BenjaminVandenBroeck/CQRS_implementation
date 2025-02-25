using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common.CQRS
{
    public class Dispatcher : IDispatcher
    {
        private readonly IServiceScope _serviceScope;

        public Dispatcher(IServiceProvider serviceProvider)
        {
            _serviceScope = serviceProvider.CreateScope();
        }

        public Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query)
            where TQuery : Query
        {
            var handler = _serviceScope.ServiceProvider.GetService<IQueryHandler<TQuery, TResult>>();
            if (handler == null)
            {
                throw new Exception($"The dispatcher could not find a queryhandler for {nameof(TQuery)}/{nameof(TResult)}");
            }
            return handler.HandleAsync(query);
        }

        public Task DispatchAsync<TCommand>(TCommand command) where TCommand : Command
        {
            var handler = _serviceScope.ServiceProvider.GetService<ICommandHandler<TCommand>>();
            if (handler == null)
            {
                throw new Exception($"The dispatcher could not find a commandHandler for {nameof(TCommand)}");
            }
            return handler.HandleAsync(command);
        }
    }
}
