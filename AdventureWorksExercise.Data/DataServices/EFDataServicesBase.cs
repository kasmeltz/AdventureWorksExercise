using AdventureWorksExercise.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AdventureWorksExercise.Data.DataServices
{
    public abstract class EFDataServicesBase<T> : IGenericDataServices<T> where T: class
    {
        #region Constructors

        public EFDataServicesBase(
            ILogger logger,
            AdventureWorksDbContext dbContext)
        {
            Logger = logger;
            DbContext = dbContext;
            DbSet = dbContext
                .Set<T>();
        }

        #endregion

        #region Members

        protected ILogger Logger { get; set; }

        protected AdventureWorksDbContext DbContext { get; set; }

        public DbSet<T> DbSet { get; protected set; }

        #endregion

        #region Abstract Members

        public abstract IQueryable<T> GetQuery(int id, IQueryable<T> query);

        #endregion

        #region IGenericDataServices 

        public async Task<T?> GetById(int id, Func<IQueryable<T>, IQueryable<T>>? queryOperations = null)
        {
            var query = DbSet
                .AsQueryable();

            if (queryOperations != null)
            {
                query = queryOperations(query);
            }

            query = GetQuery(id, query);

            return await query
                .FirstOrDefaultAsync();
        }

        #endregion
    }
}
