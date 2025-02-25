using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common.CQRS
{
    public interface ICommandHandler<T> where T: Command
    {
        Task HandleAsync(T command);
    }
}
