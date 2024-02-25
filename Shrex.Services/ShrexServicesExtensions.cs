using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System.Data;

namespace Shrex.Services
{
    public static class ShrexServicesExtensions
    {
        public static IServiceCollection AddShrex(this IServiceCollection services, string siteId)
        {
            var clientService = (services.FirstOrDefault(x => x.ServiceType == typeof(GraphServiceClient))?.ImplementationInstance as GraphServiceClient);
            var currentShrexService = services.FirstOrDefault(x => !x.IsKeyedService && x.ServiceType == typeof(Shrex));

            if (clientService is null)
            {
                throw new NullReferenceException("No GraphServiceClient has been registered.");
            }
            if (currentShrexService is not null)
            {
                throw new Exception("Unkeyed service of Shrex already exists.");
            }

            services.AddSingleton(clientService.SP(siteId));
            return services;
        }

        public static IServiceCollection AddShrex(this IServiceCollection services, string alias, string siteId)
        {

            var clientService = (services.FirstOrDefault(x => x.ServiceType == typeof(GraphServiceClient))?.ImplementationInstance as GraphServiceClient);
            var currentShrexService = services.FirstOrDefault(x => x.IsKeyedService && alias.Equals(x.ServiceKey) && x.ServiceType == typeof(Shrex));

            if (clientService is null)
            {
                throw new NullReferenceException("No GraphServiceClient has been registered.");
            }
            if (currentShrexService is not null)
            {
                throw new DuplicateNameException($"Keyed service of Shrex with key {alias} already exists.");
            }

            services.AddKeyedSingleton(alias, clientService.SP(siteId));
            return services;
        }

    }
}
