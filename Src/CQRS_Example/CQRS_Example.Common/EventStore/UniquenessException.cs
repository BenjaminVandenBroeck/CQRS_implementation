using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common.EventStore
{
    public class UniquenessException: Exception
    {
        public string PropertyName { get; }

        public string Value { get; }

        public UniquenessException(string message, string propertyName, string value, Exception innerException): base(message, innerException) 
        {
            PropertyName = propertyName;
            Value = value;  
        }
    }
}
