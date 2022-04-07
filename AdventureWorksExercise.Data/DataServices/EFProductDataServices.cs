using AdventureWorksExercise.Data.Models;
using Microsoft.Extensions.Logging;

namespace AdventureWorksExercise.Data.DataServices
{
    public class EFProductDataServices : EFDataServicesBase<Product>
    {
        #region Constructors

        public EFProductDataServices(
            ILogger<EFProductDataServices> logger,
            AdventureWorksDbContext dbContext) : base(logger, dbContext)
        { 
        }

        #endregion

        #region EFDataServicesBase 

        public override IQueryable<Product> GetQuery(int id, IQueryable<Product> query) => query.Where(o => o.ProductId == id);

        #endregion        
    }
}