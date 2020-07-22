using AutoMapper;
using GrupoCinte.Common.Dtos;
using GrupoCinte.Common.Entities;

namespace GrupoCinte.Api.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>();
            CreateMap<User, UserForDetailedDto>();
            CreateMap<UserForAddDto, User>();
            CreateMap<UserForUpdateDto, User>();
        }
    }
}
