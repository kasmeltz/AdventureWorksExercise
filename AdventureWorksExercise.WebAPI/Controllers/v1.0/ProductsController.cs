using AdventureWorksExercise.Data.DataServices;
using AdventureWorksExercise.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
            ILogger<ProductsController> logger,
            EFProductDataServices productDataServices) : 
            base(configuration, logger)
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
        [EnableQuery]
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

                return Ok(product);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [EnableQuery]
        public IActionResult GetProducts()
        {
            try
            {               
                return Ok(DefaultIncludes(ProductDataServices.DbSet.AsNoTracking()));               
            }           
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        #endregion
    }
}