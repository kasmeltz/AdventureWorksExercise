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

        protected PaginatedQuery PaginatedQueryFromRequestQuery(int? offset, int? limit, string? sortBy, string? search)
        {
            if (!offset.HasValue)
            {
                offset = 0;
            }

            if (offset < 0)
            {
                offset = 0;
            }

            if (!limit.HasValue)
            {
                limit = 10;
            }

            if (limit < 0)
            {
                limit = 10;
            }

            if (limit > 100)
            {
                limit = 100;
            }

            var paginatedQuery = new PaginatedQuery
            {
                Offset = offset.Value,
                Limit = limit.Value
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
                        var plusOrMins = sortTerm.Substring(0, 1);

                        if (plusOrMins != "-" &&
                            plusOrMins != "+")
                        {
                            throw new ArgumentException("Sort terms must start with + (ascending) or - (descending)");
                        }

                        var sortDirection = SortDirection.Ascending;

                        if (sortTerm
                            .StartsWith('-'))
                        {
                            sortDirection = SortDirection.Descending;
                        }

                        var cleanedSortTerm = sortTerm
                            .Substring(1, sortTerm.Length - 1);

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