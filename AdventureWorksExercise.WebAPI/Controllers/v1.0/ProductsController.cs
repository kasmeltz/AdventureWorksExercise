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
    public class ProductsController : ApiControllerBase
    {
        #region Constructors

        public ProductsController(
            IMapper mapper,
            ILogger<ProductsController> logger,
            EFProductDataServices productDataServices) : 
            base(mapper, logger)
        {
            ProductDataServices = productDataServices;
        }

        #endregion

        #region Members

        protected EFProductDataServices ProductDataServices { get; set; }

        #endregion

        #region Routes

        [MapToApiVersion("1.0")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await ProductDataServices
                .GetById(id, q => q
                    .Include(o => o.ProductProductPhotos)
                        .ThenInclude(o => o.ProductPhoto)
                    .Include(o => o.ProductSubcategory)
                        .ThenInclude(o => o!.ProductCategory));

            if (product == null)
            {
                return NotFound();
            }

            return Ok(Mapper
                .Map<ProductViewModel>(product));
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery]int? offset, 
            [FromQuery]int? limit, 
            [FromQuery]string? sort, 
            [FromQuery]string? search)
        {
            var paginatedQery = PaginatedQueryFromRequestQuery(offset, limit, sort, search);

            var pagedResult = await ProductDataServices
                .ListAsync(paginatedQery, q => q
                    .Include(o => o.ProductProductPhotos)
                        .ThenInclude(o => o.ProductPhoto)
                    .Include(o => o.ProductSubcategory)
                        .ThenInclude(o => o!.ProductCategory));

            return Ok(ToViewModels<Product, ProductViewModel>(pagedResult));
        }

        #endregion
    }
}