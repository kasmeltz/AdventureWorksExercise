using AdventureWorksExercise.Data.Models;
using AutoMapper;

namespace AdventureWorksExercise.WebAPI.ViewModels.Profiles
{
    public class IllustrationProfile : Profile
    {
        public IllustrationProfile()
        {
            CreateMap<ProductModelIllustration, IllustrationViewModel>()
                .ForMember(d => d.IllustrationId, o => o.MapFrom(s => s.Illustration.IllustrationId))
                .ForMember(d => d.Diagram, o => o.MapFrom(s => s.Illustration.Diagram));
        }
    }
}
