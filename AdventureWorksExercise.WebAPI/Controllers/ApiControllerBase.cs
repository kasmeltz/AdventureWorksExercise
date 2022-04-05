using AdventureWorksExercise.Data.Pagination;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksExercise.WebAPI.Controllers.V1
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        #region Constructors

        public ApiControllerBase(
            IMapper mapper,
            ILogger logger)
        {
            Logger = logger;
            Mapper = mapper;
        }

        #endregion

        #region Members

        protected IMapper Mapper { get; set; }

        protected ILogger Logger { get; set; }

        #endregion

        #region Helper Methods

        protected PaginatedQuery PaginatedQueryFromRequestQuery(int? page, int? pageSize, string? sortBy, string? search)
        {
            if (!page.HasValue)
            {
                page = 1;
            }

            if (page < 0)
            {
                page = 1;
            }

            if (!pageSize.HasValue)
            {
                pageSize = 10;
            }

            if (pageSize < 0)
            {
                pageSize = 10;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var paginatedQuery = new PaginatedQuery
            {
                Page = page.Value,
                PageSize = pageSize.Value
            };

            if (!string.IsNullOrEmpty(sortBy)) 
            {
                string[] sortTerms = sortBy
                    .Split(',');

                if (sortTerms
                    .Any())
                {
                    foreach (var sortTerm in sortTerms)
                    {
                        var sortDirection = SortDirection.Ascending;

                        if (sortTerm
                            .ToLower()
                            .Contains("desc"))
                        {
                            sortDirection = SortDirection.Descending;
                        }

                        var cleanedSortTerm = sortTerm
                            .ToLower()
                            .Replace("ascending", "")
                            .Replace("asc", "")
                            .Replace("descending", "")
                            .Replace("desc", "")
                            .Trim();

                        paginatedQuery
                            .AddSortTerm(cleanedSortTerm, sortDirection);
                    }
                }
            }

            return paginatedQuery;
        }

        protected PaginatedResult<K> ToViewModels<T,K>(PaginatedResult<T> pagedResult)
        {
            var pagedViewModels = new PaginatedResult<K>(pagedResult.Query)
            {
                Page = pagedResult.Page,
                TotalRecordCount = pagedResult.TotalRecordCount
            };

            var viewModels = new List<K>();

            if (pagedResult.Records != null &&
                pagedResult.Records.Any())
            {
                foreach (var record in pagedResult.Records)
                {
                    viewModels
                        .Add(Mapper.Map<K>(record));
                }
            }

            pagedViewModels.Records = viewModels;

            return pagedViewModels;
        }

        #endregion
    }
}