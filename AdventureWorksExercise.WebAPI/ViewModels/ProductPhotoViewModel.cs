namespace AdventureWorksExercise.WebAPI.ViewModels
{
    public class ProductPhotoViewModel
    {
        #region Members

        public bool IsPrimary { get; set; }

        public string? ThumbNailPhotoBase64 { get; set; }
       
        public string? ThumbnailPhotoFileName { get; set; }
       
        public string? LargePhotoBase64 { get; set; }

        public string? LargePhotoFileName { get; set; }

        #endregion
    }
}
