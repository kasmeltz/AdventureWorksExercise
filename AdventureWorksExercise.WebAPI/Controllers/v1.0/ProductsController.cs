using AdventureWorksExercise.Data.DataServices;
using AdventureWorksExercise.Data.Models;
using AdventureWorksExercise.WebAPI.ViewModels;
using AdventureWorksExercise.WebAPI.ViewModels.Filtering;
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
            IConfiguration configuration,
            IMapper mapper,
            ILogger<ProductsController> logger,
            EFProductDataServices productDataServices) : 
            base(configuration, mapper, logger)
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
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
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
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetProducts([FromQuery] ProductFilter productFilter)
        {
            try
            {
                var paginatedQuery = PaginatedQueryFromRequestQuery(productFilter);

                productFilter
                    .PopulatePaginatedQuery(paginatedQuery);

                var pagedResult = await ProductDataServices
                    .ListAsync(paginatedQuery, q => q
                        .Include(o => o.ProductProductPhotos)
                            .ThenInclude(o => o.ProductPhoto)
                        .Include(o => o.ProductSubcategory)
                            .ThenInclude(o => o!.ProductCategory));

                return Ok(ToViewModels<Product, ProductViewModel>(pagedResult));
            }
            catch (ArgumentException ex)
            {
                return HandleBadArguments(ex);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        #endregion
    }
}