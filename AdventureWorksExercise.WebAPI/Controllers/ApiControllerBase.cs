using AdventureWorksExercise.Data.Pagination;
using AdventureWorksExercise.WebAPI.ViewModels;
using AdventureWorksExercise.WebAPI.ViewModels.Filtering;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AdventureWorksExercise.WebAPI.Controllers.V1
{
    [ApiController]
    public class ApiControllerBase : ControllerBase
    {
        #region Constructors

        public ApiControllerBase(
            IConfiguration configuration,
            IMapper mapper,
            ILogger logger)
        {
            Configuration = configuration;
            Logger = logger;
            Mapper = mapper;
        }

        #endregion

        #region Members

        protected IConfiguration Configuration { get; set; }

        protected IMapper Mapper { get; set; }

        protected ILogger Logger { get; set; }

        #endregion

        #region Helper Methods

        protected PaginatedQuery PaginatedQueryFromRequestQuery(PaginatedFilter filter)
        {
            if (!filter.Offset.HasValue)
            {
                filter.Offset = 0;
            }

            if (filter.Offset < 0)
            {
                throw new ArgumentException("Offset can not be less than 0");
            }

            var queryDefaults = Configuration
                .GetSection("QueryDefaults");

            int defaultLimit = queryDefaults
                .GetValue<int>("DefaultLimit");

            int maxLimit = queryDefaults
                .GetValue<int>("MaxLimit");

            if (!filter.Limit.HasValue)
            {
                filter.Limit = defaultLimit;
            }

            if (filter.Limit < 0)
            {
                throw new ArgumentException("Limit can not be less than 0");
            }

            if (filter.Limit > maxLimit)
            {
                throw new ArgumentException($"Limit can not be greater than {maxLimit}");
            }

            var paginatedQuery = new PaginatedQuery
            {
                Offset = filter.Offset.Value,
                Limit = filter.Limit.Value
            };

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                string[] sortTerms = filter.SortBy
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

        protected PaginatedResult<K> ToViewModels<T, K>(PaginatedResult<T> pagedResult)
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

        protected IActionResult HandleBadArguments(ArgumentException ex)
        {
            var errorViewModel = new ErrorViewModel
            {
                Message = ex.Message
            };

            return BadRequest(new { errors = new List<ErrorViewModel> { errorViewModel } });
        }

        protected IActionResult HandleException(Exception ex)
        {
            var errorViewModel = new ErrorViewModel
            {
                Message = ex.Message,
                ErrorId = Guid.NewGuid()
            };

            Logger
                .LogError(ex, ex.Message, errorViewModel);

            return Problem("An unexpected error occured", errorViewModel.ErrorId.ToString(), 500, "An unexpected error occured" );
        }

        #endregion
    }
}