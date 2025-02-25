using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common
{
    public class MachineInfoService : IInfoService
    {
        private readonly string _name = Assembly.GetEntryAssembly().GetName().Name;

        public string Name
        {
            get { return _name; }
        }
    }
}
