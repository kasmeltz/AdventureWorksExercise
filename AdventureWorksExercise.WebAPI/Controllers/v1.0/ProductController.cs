using AdventureWorksExercise.Data.DataServices;
using AdventureWorksExercise.Data.Models;
using AdventureWorksExercise.Data.Pagination;
using AdventureWorksExercise.WebAPI.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorksExercise.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductController : ControllerBase
    {
        #region Constructors

        public ProductController(
            IMapper mapper,
            ILogger<ProductController> logger,
            EFProductDataServices productDataServices)
        {
            Logger = logger;
            ProductDataServices = productDataServices;
            Mapper = mapper;
        }

        #endregion

        #region Members

        protected IMapper Mapper { get; set; }

        protected EFProductDataServices ProductDataServices { get; set; }

        protected ILogger Logger { get; set; }

        #endregion

        #region Routes

        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var pagedQuery = new PaginatedQuery
            {
                Page = 1,
                PageSize = 10
            };

            pagedQuery
                .AddSortTerm("Name", SortDirection.Descending);

            pagedQuery
                .StartsWith("Name", "Classic");

            var pagedResult = await ProductDataServices
                .ListAsync(pagedQuery, q => q
                    .Include(o => o.ProductProductPhotos)
                        .ThenInclude(o => o.ProductPhoto)
                    .Include(o => o.ProductSubcategory)
                        .ThenInclude(o => o!.ProductCategory));

            return Ok(ToViewModels<Product, ProductViewModel>(pagedResult));
        }

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