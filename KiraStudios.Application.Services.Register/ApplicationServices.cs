using KiraStudios.Application.IdentityService.Identity;
using KiraStudios.Application.Services.IdentityContracts.Identity;
using KiraStudios.Application.Services.TokenContracts.Tracking;
using KiraStudios.Application.Services.UbicationContracts.Ubication;
using KiraStudios.Application.TokenService.Tracking;
using KiraStudios.Application.UbicationService.Ubication;
using Microsoft.Extensions.DependencyInjection;

namespace KiraStudios.Application.Services.Register
{
    public static class ApplicationServices
    {
        public static void Register(IServiceCollection services)
        {
            //  Ubication
            services.AddTransient<ICountryAppService, CountryAppService>();
            services.AddTransient<IStateAppService, StateAppService>();
            services.AddTransient<ICityAppService, CityAppService>();

            //  Authorization
            services.AddTransient<IUserAppService, UserAppService>();
            services.AddTransient<IUserClaimAppService, UserClaimAppService>();
            services.AddTransient<IRoleAppService, RoleAppService>();
            services.AddTransient<IUserRoleAppService, UserRoleAppService>();
            services.AddTransient<IRoleControlAppService, RoleControlAppService>();
            services.AddTransient<IRefreshTokenAppService, RefreshTokenAppService>();

            //  Tracking
            services.AddTransient<ITrackingTokenAppService, TrackingTokenAppService>();
        }
    }
}
