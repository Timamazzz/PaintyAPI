using AutoMapper;

namespace PaintyAPI.AutoMapper;

public class AppMappingPresentationModelsToDomainModelsProfile : Profile
{
    public AppMappingPresentationModelsToDomainModelsProfile()
    {
        CreateMap<Models.User.UserAuth, Domain.Models.User>().ReverseMap();
        CreateMap<Models.User.UserRegister, Domain.Models.User>().ReverseMap();
        CreateMap<Models.User.Friend, Domain.Models.User>().ReverseMap();
        CreateMap<Domain.Models.User, Models.User.UserResponse>().ReverseMap();

    }
}