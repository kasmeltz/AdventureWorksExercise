using AdventureWorksExercise.Data.DataServices;
using AdventureWorksExercise.Data.Models;
using AdventureWorksExercise.Data.Pagination;
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
            ILogger<ProductController> logger, 
            EFProductDataServices productDataServices)
        {
            Logger = logger; 
            ProductDataServices = productDataServices;
        }

        #endregion

        #region Members

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

            return Ok(pagedResult);
        }

        #endregion
    }
}