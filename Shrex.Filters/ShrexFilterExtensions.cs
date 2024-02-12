using Microsoft.Graph.Models;

namespace Shrex.Filters
{
    public static class ShrexFilterExtensions
    {
        /// <summary>
        /// extension method for list item retreival using SharepointExtensions filters
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="siteId">id of sharepoint site</param>
        /// <param name="listId">id of sharepoint list</param>
        /// <param name="filter">instance of IFilterString used as a $filter query parameter</param>
        /// <param name="allowDangerous">allows to filter non-indexed fields</param>
        /// <param name="expandFields">indicates if response should also fetch all field values, default true</param>
        /// <returns>collection of list items</returns>
        public async static Task<IEnumerable<ListItem>> GetListItems(this Shrex shrex, string siteId, string listId, IFilterString filter, bool allowDangerous = false, bool expandFields = true)
        {
            var items = await shrex.Client.Sites[siteId].Lists[listId]
                .Items
                .GetAsync(config =>
                {
                    if (allowDangerous)
                    {
                        config.Headers.Add("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly");
                    }
                    if (expandFields)
                    {
                        config.QueryParameters.Expand = ["fields"];
                    }
                    config.QueryParameters.Filter = filter.GetFilterString();
                });
            
            if (items is null || items.Value is null)
            {
                throw new NullReferenceException(nameof(items));
            }

            return items.Value;
        }
    }
}
