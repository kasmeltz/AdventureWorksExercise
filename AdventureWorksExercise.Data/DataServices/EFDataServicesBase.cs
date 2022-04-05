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

        public async Task<PagedResult<T>> ListAsync(PagedQuery pagedQuery, Func<IQueryable<T>, IQueryable<T>>? queryOperations = null)
        {
            PagedResult<T> pagedResult = new PagedResult<T>
            {
                Query = pagedQuery
            };

            var query = DbSet
                .AsQueryable();

            if (queryOperations != null)
            {
                query = queryOperations(query);
            }

            int totalRecordCount = await query
                .CountAsync();

            pagedResult.TotalRecordCount = totalRecordCount;

            int offset = (pagedQuery.Page - 1) * pagedQuery.PageSize;

            query = query
                .Skip(offset)
                .Take(pagedQuery.PageSize);

            pagedResult.Records = await query
                .ToListAsync();

            return pagedResult;
        }

        #endregion
    }
}
