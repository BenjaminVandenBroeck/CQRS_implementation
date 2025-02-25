using CQRS_Example.Common.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS_Example.Common.CQRS
{
    public static class QueryableExtensions
    {
        private static IQueryable<T> AddSorting<T>(this IQueryable<T> queryable, Query query)
        {

        }
    }
}
