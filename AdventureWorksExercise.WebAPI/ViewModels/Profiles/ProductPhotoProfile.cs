using AdventureWorksExercise.Data.Models;
using AutoMapper;
using System.Text;

namespace AdventureWorksExercise.WebAPI.ViewModels.Profiles
{
    public class ProductPhotoProfile : Profile
    {
        public ProductPhotoProfile()
        {
            CreateMap<ProductProductPhoto, ProductPhotoViewModel>()
                .ForMember(d => d.IsPrimary, o => o.MapFrom(s => s.Primary))
                .ForMember(d => d.ThumbnailPhotoFileName, o => o.MapFrom(s => s.ProductPhoto!.ThumbnailPhotoFileName))
                .ForMember(d => d.ThumbNailPhotoBase64, o => o.MapFrom(s => Convert.ToBase64String(s.ProductPhoto!.ThumbNailPhoto!)))
                .ForMember(d => d.LargePhotoFileName, o => o.MapFrom(s => s.ProductPhoto!.LargePhotoFileName))
                .ForMember(d => d.LargePhotoBase64, o => o.MapFrom(s => Convert.ToBase64String(s.ProductPhoto!.LargePhoto!)));
        }
    }
}
