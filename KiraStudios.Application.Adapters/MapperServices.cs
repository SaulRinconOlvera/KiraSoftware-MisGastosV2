using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace KiraStudios.Application.Adapters
{
    public static class MapperServices
    {
        public static IMapper GetMapper { get; private set; }

        public static void Register(IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            GetMapper = mappingConfig.CreateMapper();
            services.AddSingleton(GetMapper);
        }
    }
}
