using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using System.Data;

namespace Shrex.Services
{
    /// <summary>
    /// Extension class for <see cref="Shrex"/> containing methods regarding dependency injection registration.
    /// </summary>
    public static class ShrexServicesExtensions
    {
        /// <summary>
        /// Registers a singleton of <see cref="Shrex"/> for a SharePoint site.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <param name="siteId">Id a SharePoint site.</param>
        /// <returns>A reference to this instance after operation is completed.</returns>
        /// <exception cref="NullReferenceException">Thrown if no service of type <see cref="GraphServiceClient"/> is registered.</exception>
        /// <exception cref="Exception">Thrown when an instance of <see cref="Shrex"/> was already registered.</exception>
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

        /// <summary>
        /// Registers a keyed singleton of <see cref="Shrex"/> for a SharePoint site.
        /// </summary>
        /// <param name="services">Instance of <see cref="IServiceCollection"/>.</param>
        /// <param name="key">Key used as alias for the SharePoint site.</param>
        /// <param name="siteId">Id a SharePoint site.</param>
        /// <returns>A reference to this instance after operation is completed.</returns>
        /// <exception cref="NullReferenceException">Thrown if no service of type <see cref="GraphServiceClient"/> is registered.</exception>
        /// <exception cref="Exception">Thrown when an instance of <see cref="Shrex"/> was already registered.</exception>
        public static IServiceCollection AddShrex(this IServiceCollection services, string key, string siteId)
        {

            var clientService = (services.FirstOrDefault(x => x.ServiceType == typeof(GraphServiceClient))?.ImplementationInstance as GraphServiceClient);
            var currentShrexService = services.FirstOrDefault(x => x.IsKeyedService && key.Equals(x.ServiceKey) && x.ServiceType == typeof(Shrex));

            if (clientService is null)
            {
                throw new NullReferenceException("No GraphServiceClient has been registered.");
            }
            if (currentShrexService is not null)
            {
                throw new DuplicateNameException($"Keyed service of Shrex with key {key} already exists.");
            }

            services.AddKeyedSingleton(key, clientService.SP(siteId));
            return services;
        }

    }
}
