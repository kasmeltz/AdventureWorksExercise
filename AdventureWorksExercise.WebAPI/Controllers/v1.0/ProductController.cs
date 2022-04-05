using AdventureWorksExercise.Data.DataServices;
using AdventureWorksExercise.Data.Models;
using AdventureWorksExercise.Data.Pagination;
using AdventureWorksExercise.WebAPI.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

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
            var pagedQuery = new PagedQuery
            {
                Page = 1,
                PageSize = 10
            };

            var pagedResult = await ProductDataServices
                .ListAsync(pagedQuery, q => q
                    .Select(o => new Product { ProductId = o.ProductId }));

            return Ok(ToViewModels<Product, ProductViewModel>(pagedResult));
        }

        #endregion

        #region Helper Methods

        public PagedResult<K> ToViewModels<T,K>(PagedResult<T> pagedResult)
        {
            var pagedViewModels = new PagedResult<K>
            {
                Query = pagedResult.Query,
                TotalRecordCount = pagedResult.TotalRecordCount
            };

            var viewModels = new List<K>();

            foreach (var record in pagedResult.Records)
            {
                viewModels
                    .Add(Mapper.Map<K>(record));
            }

            pagedViewModels.Records = viewModels;

            return pagedViewModels;
        }

        #endregion
    }
}