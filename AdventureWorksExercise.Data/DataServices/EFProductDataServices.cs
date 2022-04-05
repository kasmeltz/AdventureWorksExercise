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

        public override IQueryable<Product> DefaultSort(IQueryable<Product> query) => query.OrderBy(o => o.ProductId);

        #endregion
    }
}