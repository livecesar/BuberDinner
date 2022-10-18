using System.Reflection;
using Mapster;
using MapsterMapper;

namespace BuberDinner.Api.Mapping;

public static class DependencyInjection
{
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            var confing = TypeAdapterConfig.GlobalSettings;
            confing.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton(confing);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
}