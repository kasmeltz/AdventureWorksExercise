namespace AdventureWorksExercise.WebAPI.ViewModels
{
    public class ProductModelViewModel
    {
        #region Members

        public int ProductModelId { get; set; }

        public string Name { get; set; } = null!;

        public string? CatalogDescription { get; set; }

        public string? Instructions { get; set; }

        public List<IllustrationViewModel>? Illustrations { get; set; }

        public List<ProductDescriptionCultureViewModel>? Descriptions { get; set; }

        #endregion
    }
}
