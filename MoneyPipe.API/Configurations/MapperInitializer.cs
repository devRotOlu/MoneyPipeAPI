using AutoMapper;
using MoneyPipe.Application.DTOs;
using MoneyPipe.Domain.Entities;

namespace MoneyPipe.API.Configurations
{
    public class MapperInitializer : Profile
    {
        public MapperInitializer()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<User, UserDetailsDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()));
            CreateMap<User, AuthResultDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));
        }
    }
}
