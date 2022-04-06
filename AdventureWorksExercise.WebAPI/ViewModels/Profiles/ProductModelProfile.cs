using AdventureWorksExercise.Data.Models;
using AutoMapper;

namespace AdventureWorksExercise.WebAPI.ViewModels.Profiles
{
    public class ProductModelProfile : Profile
    {
        public ProductModelProfile()
        {
            CreateMap<ProductModel, ProductModelViewModel>()
                .ForMember(d => d.Illustrations, o => o.MapFrom(s => s.ProductModelIllustrations))
                .ForMember(d => d.Descriptions, o => o.MapFrom(s => s.ProductModelProductDescriptionCultures));
        }
    }
}
