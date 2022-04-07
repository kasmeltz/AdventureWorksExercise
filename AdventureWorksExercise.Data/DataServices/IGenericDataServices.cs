namespace AdventureWorksExercise.Data.DataServices
{
    public interface IGenericDataServices<T> where T : class
    {
        Task<T?> GetById(int id, Func<IQueryable<T>, IQueryable<T>>? queryOperations = null);
    }
}