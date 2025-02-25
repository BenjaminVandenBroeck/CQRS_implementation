namespace CQRS_Example.Common.ReadStore
{
    public interface IReadStoreRepository<T> where T: ReadModel
    {
        Task<T?> GetByIdAsync(string id, string? containerName = null);
        IQueryable<T> GetQueryable(string? containerName = null);
        Task SaveAsync(T entity);   
    }
}
