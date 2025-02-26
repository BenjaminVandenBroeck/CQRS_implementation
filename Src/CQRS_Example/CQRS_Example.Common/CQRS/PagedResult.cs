using CQRS_Example.Common.ExceptionHandling;

namespace CQRS_Example.Common.CQRS
{
    public class PagedResult<T>
    {
        public PagedResult(Query query, int totalItemCount, List<T> results)
        {
            TotalItemCount = totalItemCount;
            if (totalItemCount == 0)
            {
                Page = 0;
                PageSize = 0;
                TotalPageCount = 0;
            }
            else
            {
                if (query.IsPaged)
                {
                    Page = query.Page.Value;
                    PageSize = query.PageSize.Value;
                }
                else
                {
                    Page = 1;
                    PageSize = TotalItemCount;
                }
                TotalPageCount = (int)Math.Ceiling((double)totalItemCount / PageSize);
            }

            if (Page > TotalPageCount)
            {
                throw new BaseException("Page number {page} is greater then the total page count {totalPageCount}", nameof(PagedResult<T>), Page, TotalPageCount);
            }

            OrderBy = query.OrderBy;
            OrderDirection = query.OrderDirection;
            Results = results;
        }

        private PagedResult()
        {

        }

        public int Page { get; private init; }
        public int PageSize { get; private init; }
        public int TotalPageCount { get; private init; }
        public int TotalItemCount { get; private init; }
        public string? OrderBy { get; private init; }
        public OrderDirection? OrderDirection { get; private init; }
        public List<T> Results { get; private init; }

        public PagedResult<U> ConvertTo<U>(Func<T, U> converter)
        {
            var pagedResult = new PagedResult<U>
            {
                Page = Page,
                PageSize = PageSize,
                TotalPageCount = TotalPageCount,
                TotalItemCount = TotalItemCount,
                OrderBy = OrderBy,
                OrderDirection = OrderDirection,
                Results = Results.Select(converter).ToList()
            };
            return pagedResult;
        }
    }
}
