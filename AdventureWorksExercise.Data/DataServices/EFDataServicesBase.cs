using AdventureWorksExercise.Data.Models;
using AdventureWorksExercise.Data.Pagination;
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

        protected DbSet<T> DbSet { get; set; }

        #endregion

        #region IGenericDataServices 

        public async Task<PagedResult<T>> ListAsync(PagedQuery query, Func<IQueryable<T>, IQueryable<T>> queryOperations = null)
        {
            PagedResult<T> pagedResult = new PagedResult<T>
            {
                Query = query
            };

            pagedResult.Records = await DbSet
                .ToListAsync();

            return pagedResult;
        }

        #endregion
    }
}
