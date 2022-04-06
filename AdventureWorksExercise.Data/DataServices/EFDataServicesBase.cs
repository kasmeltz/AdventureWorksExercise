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
                try
                {
                    query = query
                        .OrderBy(sortString);
                }
                catch
                {
                    throw new ArgumentException($"{sortString} is not a sortable field");
                }
            }

            int totalRecordCount = await query
                .CountAsync();

            pagedResult.TotalRecordCount = totalRecordCount;

            int offset = pagedQuery.Offset;
            int limit = pagedQuery.Limit;

            query = query
                .Skip(offset)
                .Take(limit);

            pagedResult.Records = await query
                .ToListAsync();

            return pagedResult;
        }

        #endregion

        #region Helper Methods

        #endregion
    }
}
