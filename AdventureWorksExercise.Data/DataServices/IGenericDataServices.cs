using AdventureWorksExercise.Data.Pagination;

namespace AdventureWorksExercise.Data.DataServices
{
    public interface IGenericDataServices<T> where T : class
    {
        Task<PaginatedResult<T>> ListAsync(PaginatedQuery pagedQuery, Func<IQueryable<T>, IQueryable<T>>? queryOperations = null);
    }
}