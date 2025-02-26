using CQRS_Example.Common.CQRS;

namespace CQRS_Example.API.Model.Base
{
    public class PagedEnvelope<T>: Envelope<IList<T>>
    {
        protected internal PagedEnvelope(PagedResult<T> pagedResult): base(pagedResult.Results)
        {
            Page=pagedResult.Page;
            PageSize=pagedResult.PageSize;
            TotalPageCount = pagedResult.TotalPageCount;
            TotalItemCount =pagedResult.TotalItemCount;
            OrderBy = pagedResult.OrderBy;
            OrderDirection =pagedResult.OrderDirection;
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPageCount { get; set; } 
        public int TotalItemCount { get; set; }
        public string? OrderBy {  get; set; }   

        public OrderDirection? OrderDirection { get; set; }
    }
}
