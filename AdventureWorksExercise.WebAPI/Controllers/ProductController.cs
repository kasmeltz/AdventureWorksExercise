using Microsoft.AspNetCore.Mvc;
using AdventureWorksExercise.Data.DataServices;
using AdventureWorksExercise.Data.Models;

namespace AdventureWorksExercise.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var items = new List<Product>();
            
            return Ok(items);
        }

        #endregion
    }
}