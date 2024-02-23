namespace Shrex.Items.Filters
{
    /// <summary>
    /// Interface used for filtration methods (e.g. <see cref="ShrexItemExtensions.GetListItems(Shrex, string, IFilterString, bool, bool)"/>, <see cref="ShrexItemExtensions.GetListItemSingle(Shrex, string, IFilterString, bool, bool)"/>) of <see cref="Shrex"/>
    /// </summary>
    public interface IFilterString
    {
        /// <summary>
        /// Method for $filter query creation.
        /// </summary>
        /// <returns>A value of $filter query for <see cref="Microsoft.Graph.GraphServiceClient"/> list items request.</returns>
        public string GetFilterString();
    }
}
