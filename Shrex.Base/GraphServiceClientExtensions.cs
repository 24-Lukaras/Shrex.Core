using Microsoft.Graph;

namespace Shrex
{
    /// <summary>
    /// Extension class for <see cref="GraphServiceClient" /> that can create <see cref="Shrex"/> base class. Please use <see cref="SP(GraphServiceClient, string)"/> for initialization.
    /// </summary>
    public static class GraphServiceClientExtensions
    {
        /// <summary>
        /// Base method of <see cref="Shrex">Shrex</see>. For further usage call methods that extends <see cref="Shrex"/> class.
        /// </summary>
        /// <param name="client">Instance of <see cref="GraphServiceClient"/>.</param>
        /// <param name="siteId">Id of a SharePoint site. Note that id is not validate upon initialization.</param>
        /// <returns>Instance of <see cref="Shrex"/> for usage of extension methods.</returns>
        public static Shrex SP(this GraphServiceClient client, string siteId)
        {
            return new Shrex(client, siteId);
        }
    }
}
