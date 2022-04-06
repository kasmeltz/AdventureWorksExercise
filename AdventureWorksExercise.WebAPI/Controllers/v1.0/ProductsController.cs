using AdventureWorksExercise.Data.DataServices;
using AdventureWorksExercise.Data.Models;
using AdventureWorksExercise.WebAPI.JSON;
using AdventureWorksExercise.WebAPI.ViewModels;
using AdventureWorksExercise.WebAPI.ViewModels.Filtering;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        protected Func<IQueryable<Product>, IQueryable<Product>> DefaultIncludes = q => q
            .Include(o => o.ProductProductPhotos)
                .ThenInclude(o => o.ProductPhoto)
            .Include(o => o.ProductSubcategory)
                .ThenInclude(o => o!.ProductCategory)
            .Include(o => o.ProductModel)
                .ThenInclude(o => o!.ProductModelProductDescriptionCultures)
                    .ThenInclude(o => o.Culture)
            .Include(o => o.ProductModel)
                    .ThenInclude(o => o!.ProductModelProductDescriptionCultures)
                        .ThenInclude(o => o.ProductDescription)
            .Include(o => o.ProductModel)
                .ThenInclude(o => o!.ProductModelIllustrations)
                    .ThenInclude(o => o.Illustration);

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
                    .GetById(id, DefaultIncludes);

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
                var queryDefaults = Configuration
                    .GetSection("QueryDefaults");

                int defaultLimit = queryDefaults
                    .GetValue<int>("DefaultLimit");

                int maxLimit = queryDefaults
                    .GetValue<int>("MaxLimit");

                var paginatedQuery = productFilter
                    .ToPaginatedQuery(defaultLimit, maxLimit);

                var pagedResult = await ProductDataServices
                    .ListAsync(paginatedQuery, DefaultIncludes);

                return new JsonResult(ToViewModels<Product, ProductViewModel>(pagedResult),
                    new JsonSerializerSettings
                    {
                        ContractResolver = new SelectedFieldContractResolver<ProductViewModel>(productFilter)
                    });
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