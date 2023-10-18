using AutoMapper;

namespace Domain.AutoMapper;

public class AppMappingDomainModelsToDataAccessModelsProfile : Profile
{
    public AppMappingDomainModelsToDataAccessModelsProfile()
    {
        CreateMap<Models.User, DataAccess.Models.User>().ReverseMap();

    }
}