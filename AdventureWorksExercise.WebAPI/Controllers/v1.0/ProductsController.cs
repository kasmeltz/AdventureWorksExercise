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
            AdventureWorksDbContext dbContext) : 
            base(configuration, logger)
        {
            DbContext = dbContext;
        }

        #endregion

        #region Members

        protected AdventureWorksDbContext DbContext { get; set; }

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
                var query = DefaultIncludes(DbContext.Products);
                var product = await query
                    .FirstOrDefaultAsync(o => o.ProductId == id);
                    
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
                return Ok(DefaultIncludes(DbContext.Products.AsNoTracking()));               
            }           
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        #endregion
    }
}