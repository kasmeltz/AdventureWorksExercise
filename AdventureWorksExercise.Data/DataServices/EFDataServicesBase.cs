using AdventureWorksExercise.Data.Models;
using AdventureWorksExercise.Data.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

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

        #region Abstract Members

        public abstract IQueryable<T> DefaultSort(IQueryable<T> query);

        #endregion

        #region IGenericDataServices 

        public async Task<PaginatedResult<T>> ListAsync(PaginatedQuery pagedQuery, Func<IQueryable<T>, IQueryable<T>>? queryOperations = null)
        {
            PaginatedResult<T> pagedResult = new PaginatedResult<T>(pagedQuery);

            var query = DbSet
                .AsQueryable();

            if (queryOperations != null)
            {
                query = queryOperations(query);
            }

            var searchString = pagedQuery.SearchString;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query
                    .Where(searchString, pagedQuery.SearchValues
                        .ToArray());
            }

            var sortString = pagedQuery.SortString;
            if (string.IsNullOrEmpty(sortString))
            {
                query = DefaultSort(query);
            }
            else
            {
                query = query
                    .OrderBy(sortString);
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

            if (pagedQuery.Page >= pagedResult.PageCount)
            {
                pagedResult.Page = pagedResult.PageCount;
            }
            else
            {
                pagedResult.Page = pagedQuery.Page;
            }

            return pagedResult;
        }

        #endregion

        #region Helper Methods

        #endregion
    }
}
