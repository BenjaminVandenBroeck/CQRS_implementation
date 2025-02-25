namespace CQRS_Example.Common.CQRS
{
    public abstract class Query
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public string? OrderBy { get; set; }

        public OrderDirection OrderDirection { get; set; }

        public bool IsPaged => this is { Page: > 0, PageSize: > 0 };
    }
}
