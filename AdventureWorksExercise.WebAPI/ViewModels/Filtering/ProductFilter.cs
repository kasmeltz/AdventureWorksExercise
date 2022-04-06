using AdventureWorksExercise.Data.Pagination;

namespace AdventureWorksExercise.WebAPI.ViewModels.Filtering
{
    /// <summary>
    /// Represents the input to a Product API request that describes how to filter the results.
    /// </summary>
    public class ProductFilter : PaginatedFilter
    {
        #region Members

        public string? Name { get; set; }
        
        public string? ProductNumber { get; set; }

        public string? Category { get; set; }

        public string? SubCategory { get; set; }

        #endregion

        #region PaginatedFilter

        private static Dictionary<string,string> _modelFieldTranslations = new Dictionary<string,string>
        {
            ["subcategory"] = "productsubcategory.name",
            ["category"] = "productsubcategory.productcategory.name",
            ["photos"] = "productproductphotos",
            ["model"] = "productmodel"
        };

        protected override Dictionary<string, string> ModelFieldTranslations => _modelFieldTranslations;

        #endregion

        #region Public Methods

        public void PopulatePaginatedQuery(PaginatedQuery paginatedQuery)
        {
            paginatedQuery
                .ResetSearch();

            SetPaginatedQueryTerm(paginatedQuery, "Name", Name);
            SetPaginatedQueryTerm(paginatedQuery, "ProductNumber", ProductNumber);
            SetPaginatedQueryTerm(paginatedQuery, "ProductSubcategory.Name", SubCategory);
            SetPaginatedQueryTerm(paginatedQuery, "ProductSubcategory.ProductCategory.Name", Category);
        }

        #endregion
    }
}
