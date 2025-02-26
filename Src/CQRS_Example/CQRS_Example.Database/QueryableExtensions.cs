using CQRS_Example.Common.CQRS;
using Microsoft.Azure.Cosmos.Linq;
using System;
using System.Linq.Dynamic.Core;
namespace CQRS_Example.Common.CQRS
{
    public static class QueryableExtensions
    {
        private static IQueryable<T> AddSorting<T>(this IQueryable<T> queryable, Query query)
        {
            if (string.IsNullOrWhiteSpace(query.OrderBy))
            {
                queryable = queryable.OrderBy(string.Format("{0}searchField: {1}", query.OrderBy, query.OrderDirection));
            }
            return queryable;
        }

        private static IQueryable<T> AddPaging<T>(this IQueryable<T> queryable, Query query)
        {
            if (query.IsPaged)
            {
                queryable= queryable.Skip((query.Page.Value -1)*query.PageSize.Value).Take(query.PageSize.Value);
            }
            return queryable;
        }

        private static IQueryable<T> AddPagingAndSorting<T>(this IQueryable<T> queryable, Query query)
        {
            queryable = queryable.AddSorting(query);
            queryable= queryable.AddPaging(query);
            return queryable;
        }

        public static async Task<PagedResult<T>> ToPagedResult<T>(this IQueryable<T> queryable, Query query)
        {
            var results = new List<T>();
            var totalItemCount = queryable.Count();
            var pagedAndSortedQueryable = queryable.AddPagingAndSorting(query);
            var feed = pagedAndSortedQueryable.ToFeedIterator();
            while (feed.HasMoreResults)
            {
                var page = await feed.ReadNextAsync();
                results.AddRange(page);
            }

            return new PagedResult<T>(query, totalItemCount, results);
        }
    }
}
