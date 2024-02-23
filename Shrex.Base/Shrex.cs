using Microsoft.Graph;

namespace Shrex
{
    /// <summary>
    /// <see cref="Shrex"/> base class meant to be extended in other packages. Please use <see cref="GraphServiceClientExtensions.SP(GraphServiceClient, string)"/> during initialization for better experience.
    /// </summary>
    public class Shrex : IDisposable
    {
        /// <summary>
        /// Id of a SharePoint site stored for further usage in extension methods.
        /// </summary>
        public string SiteId { get; private set; }

        /// <summary>
        /// Instance of <see cref="GraphServiceClient"/> stored for further usage in extension methods.
        /// </summary>
        public GraphServiceClient Client { get; private set; }

        /// <summary>
        /// Default constructor for <see cref="Shrex"/>. Please use <see cref="GraphServiceClientExtensions.SP(GraphServiceClient, string)"/> for better experience.
        /// </summary>
        /// <param name="client">Instance of <see cref="GraphServiceClient"/>.</param>
        /// <param name="siteId">Id of a SharePoint site.</param>
        public Shrex(GraphServiceClient client, string siteId)
        {
            Client = client;
            SiteId = siteId;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
