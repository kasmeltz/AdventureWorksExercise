using AdventureWorksExercise.Data.Pagination;

namespace AdventureWorksExercise.Data.DataServices
{
    public interface IGenericDataServices<T> where T : class
    {
        Task<PagedResult<T>> ListAsync(PagedQuery pagedQuery, Func<IQueryable<T>, IQueryable<T>>? queryOperations = null);
    }
}