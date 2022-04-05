using AdventureWorksExercise.Data.Models;
using AutoMapper;

namespace AdventureWorksExercise.WebAPI.ViewModels.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductViewModel>();
        }
    }
}
