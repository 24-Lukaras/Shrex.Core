using Shrex.Items.Filters;

namespace Shrex.Items.Abstractions
{
    /// <summary>
    /// Interface used for filtration methods (e.g. <see cref="ShrexItemExtensions.GetListItems(Shrex, string, IExpandQuery)"/>,
    /// <see cref="ShrexItemExtensions.GetListItems(Shrex, string, IFilterString, IExpandQuery, bool)"/>) of <see cref="Shrex"/>
    /// </summary>
    public interface IExpandQuery
    {
        /// <summary>
        /// Method for $expand query creation.
        /// </summary>
        /// <returns>An array of strings used for $expand query in <see cref="Microsoft.Graph.GraphServiceClient"/> list items request.</returns>
        public string[] GetExpandQuery();
    }
}
