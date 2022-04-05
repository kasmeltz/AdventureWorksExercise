using AdventureWorksExercise.Data.Models;
using AutoMapper;

namespace AdventureWorksExercise.WebAPI.ViewModels.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(d => d.Photos, o => o.MapFrom(s => s.ProductProductPhotos))
                .ForMember(d => d.Subcategory, o => o.MapFrom(s => s.ProductSubcategory!.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.ProductSubcategory!.ProductCategory!.Name));
        }
    }
}
