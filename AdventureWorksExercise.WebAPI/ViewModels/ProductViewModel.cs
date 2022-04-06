namespace AdventureWorksExercise.WebAPI.ViewModels
{
    public class ProductViewModel
    {
        #region FilterTranslations

        public static string[] FilterTranslations = new string[]
        {
            "Subcategory:ProductSubcategory.Name",
            "Category:ProductSubcategory.ProductCategory.Name",
            "Photos.IsPrimary:ProductProductPhotos.Primary",
            "Photos.ThumbnailPhotoFileName:ProductProductPhotos.ProductPhoto.ThumbnailPhotoFileName",
            "Photos.LargePhotoFileName:ProductProductPhotos.ProductPhoto.LargePhotoFileName",
        };

        #endregion

        #region Members

        public int ProductId { get; set; }

        public string? Name { get; set; }

        public string ProductNumber { get; set; } = null!;
       
        public bool? MakeFlag { get; set; }
       
        public bool? FinishedGoodsFlag { get; set; }
        
        public string? Color { get; set; }
        
        public short SafetyStockLevel { get; set; }
        
        public short ReorderPoint { get; set; }
        
        public decimal StandardCost { get; set; }
        
        public decimal ListPrice { get; set; }
        
        public string? Size { get; set; }
        
        public string? SizeUnitMeasureCode { get; set; }
        
        public string? WeightUnitMeasureCode { get; set; }
        
        public decimal? Weight { get; set; }
        
        public int DaysToManufacture { get; set; }
        
        public string? ProductLine { get; set; }
        
        public string? Class { get; set; }
        
        public string? Style { get; set; }
        
        public int? ProductSubcategoryId { get; set; }
        
        public int? ProductModelId { get; set; }
        
        public DateTime SellStartDate { get; set; }
        
        public DateTime? SellEndDate { get; set; }
        
        public DateTime? DiscontinuedDate { get; set; }

        public string? Subcategory { get; set; }

        public string? Category { get; set; }

        public List<ProductPhotoViewModel>? Photos { get; set; }

        #endregion
    }
}
