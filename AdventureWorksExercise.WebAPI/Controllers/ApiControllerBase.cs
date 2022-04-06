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