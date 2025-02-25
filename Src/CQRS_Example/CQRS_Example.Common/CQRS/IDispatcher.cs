using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common.CQRS
{
    public interface IDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : Query;
        Task DispatchAsync<TCommand>(TCommand command) where TCommand : Command;
    }
}
