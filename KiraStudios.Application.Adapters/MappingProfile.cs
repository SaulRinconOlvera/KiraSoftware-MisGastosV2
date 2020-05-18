using AutoMapper;
using KiraStudios.Application.TokenViewModel.Tracking;
using KiraStudios.Application.UbicationViewModel.Ubication;
using KiraStudios.Domain.IdentityModel.Identity;
using KiraStudios.Domain.IdentityModel.Navegation;
using KiraStudios.Domain.TokenModel.Tracking;
using KiraStudios.Domain.UbicationModel.Ubication;
using KiraSudios.Application.IdentityViewModel.Identity;
using KiraSudios.Application.IdentityViewModel.Navegation;

namespace KiraStudios.Application.Adapters
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Country, CountryViewModel>().ReverseMap();
            CreateMap<State, StateViewModel>().ReverseMap();
            CreateMap<City, CityViewModel>().ReverseMap();
            CreateMap<User, UserViewModel>()
                .ForMember(m => m.PasswordHash, o => o.Ignore())
                .ForMember(m => m.SecurityStamp, o => o.Ignore());
            CreateMap<UserLogin, UserLoginViewModel>().ReverseMap();
            CreateMap<UserClaim, UserClaimViewModel>().ReverseMap();
            CreateMap<UserViewModel, User>();
            CreateMap<UserRole, UserRoleViewModel>().ReverseMap();
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<RoleControl, RoleControlViewModel>().ReverseMap();
            CreateMap<Control, ControlViewModel>().ReverseMap();
            CreateMap<RefreshToken, RefreshTokenViewModel>().ReverseMap();
            CreateMap<TrackingToken, TrackingTokenViewModel>().ReverseMap();
        }
    }
}
