using KiraSolutions.Infrastructure.Configuration;
using KiraStudios.Application.Adapters;
using KiraStudios.Application.Services.Register;
using KiraStudios.Infrastructure.Respositories.Register;
using Microsoft.Extensions.DependencyInjection;

namespace KiraStudios.CrossCutting.ServiceRegister
{
    public static class RegisterServices
    {
        public static void Register(IServiceCollection services)
        {
            RepositoryServices.Register(services);
            MapperServices.Register(services);
            ApplicationServices.Register(services);
            IdentityConfiguration.Register(services);
        }
    }
}
