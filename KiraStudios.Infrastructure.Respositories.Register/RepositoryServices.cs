using KiraStudios.Domain.IdentityRepository;
using KiraStudios.Domain.RepositoryBase;
using KiraStudios.Domain.TokenRepository;
using KiraStudios.Domain.UbicationRepository;
using KiraStudios.Infraestructure.IdentityRepository;
using KiraStudios.Infraestructure.TrackingRepository.Tracking;
using KiraStudios.Infraestructure.UbicationRepository.Ubication;
using KiraStudios.Infrastructure.RepositoryBase;
using Microsoft.Extensions.DependencyInjection;

namespace KiraStudios.Infrastructure.Respositories.Register
{
    public static class RepositoryServices
    {
        public static void Register(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ICountryRepository, CountryRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<ICityRepository, CityRepository>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserLoginRepository, UserLoginRepository>();
            services.AddTransient<IUserRoleRepository, UserRoleRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRoleControlRepository, RoleControlRepository>();
            services.AddTransient<IUserClaimRepository, UserClaimRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();

            //  Tracking
            services.AddTransient<ITrackingTokenRepository, TrackingTokenRepository>();
        }
    }
}
