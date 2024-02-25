using Microsoft.Graph;
using Microsoft.Graph.Models;
using Shrex.Items.Abstractions;
using Shrex.Items.Filters;
using Shrex.Items.Mapping;

namespace Shrex.Items
{
    /// <summary>
    /// 
    /// </summary>
    public static class ShrexItemExtensions
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async static Task<ListItem?> CreateListItem(this Shrex shrex, string listId, IDictionary<string, object> data)
        {
            ListItem item = new ListItem()
            {
                Fields = new FieldValueSet()
                {
                    AdditionalData = data
                },
            };

            var result = await shrex.Client.Sites[shrex.SiteId].Lists[listId].Items
                .PostAsync(item);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async static Task<ListItem?> CreateListItem<T>(this Shrex shrex, string listId, T? entity) where T : IListItemDto
        {
            return await shrex.CreateListItem(listId, GetData(entity));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async static Task<ListItem?> ReadListItem(this Shrex shrex, string listId, string itemId)
        {
            var result = await shrex.Client.Sites[shrex.SiteId].Lists[listId].Items[itemId]
                .GetAsync(config =>
                    {
                        config.QueryParameters.Expand = ["fields"];
                    }
                );

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <param name="itemId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async static Task<ListItem?> UpdateListItem(this Shrex shrex, string listId, string itemId, IDictionary<string, object> data)
        {
            ListItem item = new ListItem()
            {
                Fields = new FieldValueSet()
                {
                    AdditionalData = data
                },
            };

            var result = await shrex.Client.Sites[shrex.SiteId].Lists[listId].Items[itemId]
                .PatchAsync(item);

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <param name="itemId"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async static Task<ListItem?> UpdateListItem<T>(this Shrex shrex, string listId, T entity) where T : IListItemDto
        {
            return await shrex.UpdateListItem(listId, entity.Id, GetData(entity));
        }        


        /// <summary>
        /// 
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public async static Task<bool> DeleteListItem(this Shrex shrex, string listId, string itemId)
        {
            await shrex.Client.Sites[shrex.SiteId].Lists[listId].Items[itemId]
                .DeleteAsync();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static IDictionary<string, object> GetData<T>(T entity) where T : IListItemDto
        {
            Dictionary<string, object> data = new Dictionary<string, object>();

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (property.Name != nameof(entity.Id))
                {
                    if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                    {
                        data[property.Name] = Convert.ToDouble(property.GetValue(entity));
                    }
                    else
                    {
                        data[property.Name] = property.GetValue(entity);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Extension method for list item retreival using SharepointExtensions filters.
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="listId">Id of sharepoint a list.</param>
        /// <param name="filter">Instance of IFilterString used as a $filter query parameter.</param>
        /// <param name="allowDangerous">Allows to filter non-indexed fields. Might cause issues with large lists.</param>
        /// <param name="expandFields">Indicates if response should also fetch all field values, default true.</param>
        /// <returns>Collection of filtered list items.</returns>
        public async static Task<IEnumerable<ListItem>> GetListItems(this Shrex sp, string listId, IFilterString filter, bool allowDangerous = false, bool expandFields = true)
        {
            List<ListItem> result = new List<ListItem>();

            var items = await sp.Client.Sites[sp.SiteId].Lists[listId]
                .Items
                .GetAsync(config =>
                {
                    if (allowDangerous)
                    {
                        config.Headers.Add("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly");
                    }
                    if (expandFields)
                    {
                        config.QueryParameters.Expand = [ "fields" ];
                    }
                    config.QueryParameters.Filter = filter.GetFilterString();
                });
            
            if (items is null || items.Value is null)
            {
                throw new NullReferenceException(nameof(items));
            }


            result.AddRange(items.Value);

            await sp.GetPagedItems(items, result);

            return result;
        }

        /// <summary>
        /// Extension method for list item retreival using SharepointExtensions filters.
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="listId">Id of sharepoint a list.</param>
        /// <param name="filter">Instance of IFilterString used as a $filter query parameter.</param>
        /// <param name="expandQuery"></param>
        /// <param name="allowDangerous">Allows to filter non-indexed fields. Might cause issues with large lists.</param>
        /// <returns>Collection of filtered list items.</returns>
        public async static Task<IEnumerable<ListItem>> GetListItems(this Shrex sp, string listId, IFilterString filter, IExpandQuery expandQuery, bool allowDangerous = false)
        {
            List<ListItem> result = new List<ListItem>();

            var items = await sp.Client.Sites[sp.SiteId].Lists[listId]
                .Items
                .GetAsync(config =>
                {
                    config.QueryParameters.Expand = expandQuery.GetExpandQuery();
                    if (allowDangerous)
                    {
                        config.Headers.Add("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly");
                    }
                    config.QueryParameters.Filter = filter.GetFilterString();
                });

            if (items is null || items.Value is null)
            {
                throw new NullReferenceException(nameof(items));
            }


            result.AddRange(items.Value);

            await sp.GetPagedItems(items, result);

            return result;
        }

        /// <summary>
        /// Extension method for list item retreival using SharepointExtensions filters.
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="listId">Id of sharepoint a list.</param>
        /// <param name="expandQuery"></param>
        /// <param name="allowDangerous">Allows to filter non-indexed fields. Might cause issues with large lists.</param>
        /// <returns>Collection of filtered list items.</returns>
        public async static Task<IEnumerable<ListItem>> GetListItems(this Shrex sp, string listId, IExpandQuery expandQuery, bool allowDangerous = false)
        {
            List<ListItem> result = new List<ListItem>();

            var items = await sp.Client.Sites[sp.SiteId].Lists[listId]
                .Items
                .GetAsync(config =>
                {
                    config.QueryParameters.Expand = expandQuery.GetExpandQuery();
                    if (allowDangerous)
                    {
                        config.Headers.Add("Prefer", "HonorNonIndexedQueriesWarningMayFailRandomly");
                    }
                });

            if (items is null || items.Value is null)
            {
                throw new NullReferenceException(nameof(items));
            }

            result.AddRange(items.Value);

            await sp.GetPagedItems(items, result);

            return result;
        }

        private static async Task GetPagedItems(this Shrex sp, ListItemCollectionResponse? items, List<ListItem> result)
        {
            while (items.OdataNextLink is not null)
            {
                var pageIterator = PageIterator<ListItem, ListItemCollectionResponse>
                    .CreatePageIterator(
                        sp.Client,
                        items,
                        (item) =>
                        {
                            result.Add(item);
                            return true;
                        },
                        (request) => { return request; }
                    );

                await pageIterator.IterateAsync();
            }
        }

        /// <summary>
        /// extension method for list item retreival using SharepointExtensions filters
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="listId">id of sharepoint list</param>
        /// <param name="filter">instance of IFilterString used as a $filter query parameter</param>
        /// <param name="allowDangerous">allows to filter non-indexed fields</param>
        /// <param name="expandFields">indicates if response should also fetch all field values, default true</param>
        /// <returns>Collection of list items</returns>
        public async static Task<ListItem?> GetListItemSingle(this Shrex shrex, string listId, IFilterString filter, bool allowDangerous = false, bool expandFields = true)
        {
            var items = await shrex.Client.Sites[shrex.SiteId].Lists[listId]
                .Items
                .GetAsync(config =>
                {
                    config.QueryParameters.Top = 1;
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

            if (items is null || items.Value is null || items.Value.Count != 1)
            {
                return null;
            }

            return items.Value.First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T Map<T>(this Shrex shrex, string listId, ListItem item) where T : IListItemDto, new()
        {
            T result = new();

            result.Id = item.Id;
            var mapper = DynamicListItemMapperLibrary.GetMapper<T>(shrex, listId);
            mapper.FillObject(result, item);

            return result;
        }

        public static IEnumerable<T> Map<T>(this Shrex shrex, IEnumerable<ListItem?> listItems, string listId) where T : IListItemDto, new()
        {
            var mapper = DynamicListItemMapperLibrary.GetMapper<T>(shrex, listId);

            return listItems.Select(x => {
                var result = new T();
                result.Id = x.Id;
                mapper.FillObject(result, x);
                return result;
            });
        }

        public static async Task<List<string>> GetChoices(this Shrex shrex, string listId, string fieldName)
        {
            var columns = await shrex.Client.Sites[shrex.SiteId].Lists[listId].Columns.GetAsync();
            var column = columns.Value.FirstOrDefault(x => fieldName.Equals(x.Name));
            return column.Choice.Choices ?? throw new ArgumentException("Choice field was not found.");
        }
    }
}
