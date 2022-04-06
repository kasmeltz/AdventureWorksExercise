using AdventureWorksExercise.Data.Models;
using AutoMapper;

namespace AdventureWorksExercise.WebAPI.ViewModels.Profiles
{
    public class ProductDescriptionCultureProfile : Profile
    {
        public ProductDescriptionCultureProfile()
        {
            CreateMap<ProductModelProductDescriptionCulture, ProductDescriptionCultureViewModel>()
                .ForMember(d => d.Culture, o => o.MapFrom(s => s.Culture.Name))
                .ForMember(d => d.CultureId, o => o.MapFrom(s => s.CultureId))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.ProductDescription.Description));
        }
    }
}
