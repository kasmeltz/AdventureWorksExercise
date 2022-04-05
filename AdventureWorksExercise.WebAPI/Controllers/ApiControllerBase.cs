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

        public PaginatedResult<K> ToViewModels<T,K>(PaginatedResult<T> pagedResult)
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