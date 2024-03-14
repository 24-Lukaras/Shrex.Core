using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ExternalConnectors;
using Shrex.Items.Abstractions;
using Shrex.Items.Filters;
using Shrex.Items.Mapping;

namespace Shrex.Items
{
    /// <summary>
    /// Extension class for <see cref="Shrex"/> containing methods regarding SharePoint items.
    /// </summary>
    public static class ShrexItemExtensions
    {

        /// <summary>
        /// Creates <see cref="ListItem"/> in a SharePoint list.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of SharePoint list where to create <see cref="ListItem"/>.</param>
        /// <param name="data">Dictionary of field values.</param>
        /// <returns>Created ListItem.</returns>
        public async static Task<ListItem> CreateListItem(this Shrex sp, string listId, IDictionary<string, object> data)
        {
            ListItem item = new()
            {
                Fields = new FieldValueSet()
                {
                    AdditionalData = data
                },
            };

            var result = await sp.Client.Sites[sp.SiteId].Lists[listId].Items
                .PostAsync(item);

            if (result is null) throw new NullReferenceException(nameof(result));

            return result;
        }

        /// <summary>
        /// Creates <see cref="ListItem"/> in a SharePoint list.
        /// </summary>
        /// <typeparam name="T">Type implementing <see cref="IListItemDto"/>.</typeparam>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of SharePoint list where to create <see cref="ListItem"/>.</param>
        /// <param name="entity">Entity implementing <see cref="IListItemDto"/>, that provides list item values.</param>
        /// <returns>Created ListItem.</returns>
        public async static Task<ListItem> CreateListItem<T>(this Shrex sp, string listId, T entity) where T : IListItemDto
        {
            ArgumentNullException.ThrowIfNull(listId);
            ArgumentNullException.ThrowIfNull(entity);

            var result = await sp.CreateListItem(listId, GetData(entity));

            if (result is null) throw new NullReferenceException(nameof(result));

            return result;
        }

        /// <summary>
        /// Tries to find a <see cref="ListItem"/> from a SharePoint list based on Id.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of SharePoint list where to look for <see cref="ListItem"/>.</param>
        /// <param name="itemId">Id of desired <see cref="ListItem"/>.</param>
        /// <returns>Found ListItem.</returns>
        public async static Task<ListItem> ReadListItem(this Shrex sp, string listId, string itemId)
        {
            ArgumentNullException.ThrowIfNull(listId);
            ArgumentNullException.ThrowIfNull(itemId);

            var result = await sp.Client.Sites[sp.SiteId].Lists[listId].Items[itemId]
                .GetAsync(config =>
                    {
                        config.QueryParameters.Expand = ["fields"];
                    }
                );

            return result is null ? throw new KeyNotFoundException(nameof(itemId)) : result;
        }

        /// <summary>
        /// Tries to find a <see cref="ListItem"/> from a SharePoint list based on Id. Allows usage of custom expand query.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of SharePoint list where to look for <see cref="ListItem"/>.</param>
        /// <param name="itemId">Id of desired <see cref="ListItem"/>.</param>
        /// <param name="expandQuery">Graph $expand query provider. (e.g. <see cref="DefaultExpandQuery.Instance"/> or <see cref="DynamicExpandQuery{T}"/>)</param>
        /// <returns>Found ListItem.</returns>
        public async static Task<ListItem> ReadListItem(this Shrex sp, string listId, string itemId, IExpandQuery expandQuery)
        {
            ArgumentNullException.ThrowIfNull(listId);
            ArgumentNullException.ThrowIfNull(itemId);
            ArgumentNullException.ThrowIfNull(expandQuery);

            var result = await sp.Client.Sites[sp.SiteId].Lists[listId].Items[itemId]
                .GetAsync(config =>
                {
                    config.QueryParameters.Expand = expandQuery.GetExpandQuery();
                }
            );

            return result is null ? throw new KeyNotFoundException(nameof(itemId)) : result;
        }


        /// <summary>
        /// Updates <see cref="ListItem"/> in a SharePoint list.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of SharePoint list where to update <see cref="ListItem"/>.</param>
        /// <param name="itemId">Id of desired <see cref="ListItem"/>.</param>
        /// <param name="data">Dictionary of field values.</param>
        /// <returns>Updated ListItem.</returns>
        public async static Task<ListItem> UpdateListItem(this Shrex sp, string listId, string itemId, IDictionary<string, object> data)
        {
            ArgumentNullException.ThrowIfNull(listId);
            ArgumentNullException.ThrowIfNull(itemId);
            ArgumentNullException.ThrowIfNull(data);

            ListItem item = new()
            {
                Fields = new FieldValueSet()
                {
                    AdditionalData = data
                },
            };

            var result = await sp.Client.Sites[sp.SiteId].Lists[listId].Items[itemId]
                .PatchAsync(item);

            return result is null ? throw new KeyNotFoundException(nameof(itemId)) : result;
        }

        /// <summary>
        /// Updates <see cref="ListItem"/> in a SharePoint list.
        /// </summary>
        /// <typeparam name="T">Type implementing <see cref="IListItemDto"/>.</typeparam>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of SharePoint list where to update <see cref="ListItem"/>.</param>
        /// <param name="entity">Entity implementing <see cref="IListItemDto"/>, that provides list item values.</param>
        /// <returns>Updated ListItem.</returns>
        public async static Task<ListItem> UpdateListItem<T>(this Shrex sp, string listId, T entity) where T : IListItemDto
        {
            ArgumentNullException.ThrowIfNull(listId);
            ArgumentNullException.ThrowIfNull(entity);
            ArgumentNullException.ThrowIfNull(entity.Id);

            var result = await sp.UpdateListItem(listId, entity.Id, GetData(entity));

            return result is null ? throw new KeyNotFoundException(nameof(entity.Id)) : result;
        }

        /// <summary>
        /// Deletes <see cref="ListItem"/> from a SharePoint list.
        /// </summary>
        /// <param name="shrex">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of SharePoint list where to delete <see cref="ListItem"/> from.</param>
        /// <param name="itemId">Id of desired <see cref="ListItem"/>.</param>
        /// <returns>Either true or exception if the deletion fails.</returns>
        public async static Task<bool> DeleteListItem(this Shrex shrex, string listId, string itemId)
        {
            ArgumentNullException.ThrowIfNull(listId);
            ArgumentNullException.ThrowIfNull(itemId);

            await shrex.Client.Sites[shrex.SiteId].Lists[listId].Items[itemId]
                .DeleteAsync();

            return true;
        }

        /// <summary>
        /// Creates dictionary of field values to create or update <see cref="ListItem"/> request.
        /// </summary>
        /// <typeparam name="T">Type implementing <see cref="IListItemDto"/>.</typeparam>
        /// <param name="entity">Entity implementing <see cref="IListItemDto"/>, that provides list item values.</param>
        /// <returns>Dictionary of field values.</returns>
        private static Dictionary<string, object> GetData<T>(T entity) where T : IListItemDto
        {
            ArgumentNullException.ThrowIfNull(entity);

            Dictionary<string, object> data = [];

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
                        #pragma warning disable CS8601
                        data[property.Name] = property.GetValue(entity);
                        #pragma warning restore CS8601
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Extension method for list item retreival using SharepointExtensions filters.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of sharepoint a list.</param>
        /// <param name="filter">Instance of IFilterString used as a $filter query parameter. (e.g. <see cref="FilterCondition{T}"/> or <see cref="StringCondition"/>)</param>
        /// <param name="allowDangerous">Allows to filter non-indexed fields. Might cause issues with large lists.</param>
        /// <param name="expandFields">Indicates if response should also fetch all field values, default true.</param>
        /// <returns>Collection of filtered list items.</returns>
        public async static Task<IEnumerable<ListItem>> GetListItems(this Shrex sp, string listId, IFilterString filter, bool allowDangerous = false, bool expandFields = true)
        {
            ArgumentNullException.ThrowIfNull(listId);
            ArgumentNullException.ThrowIfNull(filter);

            List<ListItem> result = [];

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
                        config.QueryParameters.Expand = DefaultExpandQuery.Instance.GetExpandQuery();
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
        /// Extension method for list item retreival using SharepointExtensions filters with custom $expand query.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of sharepoint a list.</param>
        /// <param name="filter">Instance of IFilterString used as a $filter query parameter. (e.g. <see cref="FilterCondition{T}"/> or <see cref="StringCondition"/>)</param>
        /// <param name="allowDangerous">Allows to filter non-indexed fields. Might cause issues with large lists.</param>
        /// <param name="expandQuery">Graph $expand query provider. (e.g. <see cref="DefaultExpandQuery.Instance"/> or <see cref="DynamicExpandQuery{T}"/>)</param>
        /// <returns>Collection of filtered list items.</returns>
        public async static Task<IEnumerable<ListItem>> GetListItems(this Shrex sp, string listId, IFilterString filter, IExpandQuery expandQuery, bool allowDangerous = false)
        {
            ArgumentNullException.ThrowIfNull(listId);
            ArgumentNullException.ThrowIfNull(filter);
            ArgumentNullException.ThrowIfNull(expandQuery);

            List<ListItem> result = [];

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
        /// Extension method for retreival of all list items from SharePoint list with custom $expand query.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of sharepoint a list.</param>
        /// <param name="expandQuery">Graph $expand query provider. (e.g. <see cref="DefaultExpandQuery.Instance"/> or <see cref="DynamicExpandQuery{T}"/>)</param>
        /// <returns>Collection of list items.</returns>
        public async static Task<IEnumerable<ListItem>> GetListItems(this Shrex sp, string listId, IExpandQuery expandQuery)
        {
            List<ListItem> result = [];

            var items = await sp.Client.Sites[sp.SiteId].Lists[listId]
                .Items
                .GetAsync(config =>
                {
                    config.QueryParameters.Expand = expandQuery.GetExpandQuery();
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
        /// Method for retreival all list items from paged response.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="items">Paged response.</param>
        /// <param name="result">Collection where to fill retrieved list items.</param>
        private static async Task GetPagedItems(this Shrex sp, ListItemCollectionResponse items, List<ListItem> result)
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
        /// Extension method for retreival of single list item using SharepointExtensions filters.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of a SharePoint list.</param>
        /// <param name="filter">Instance of IFilterString used as a $filter query parameter. (e.g. <see cref="FilterCondition{T}"/> or <see cref="StringCondition"/>)</param>
        /// <param name="allowDangerous">Allows to filter non-indexed fields. Might cause issues with large lists.</param>
        /// <param name="expandFields">Indicates if response should also fetch all field values, default true.</param>
        /// <returns>First filtered list item.</returns>
        public async static Task<ListItem?> GetListItemSingle(this Shrex sp, string listId, IFilterString filter, bool allowDangerous = false, bool expandFields = true)
        {
            var items = await sp.Client.Sites[sp.SiteId].Lists[listId]
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
        /// Auto mapping method for conversion between <see cref="ListItem"/> and instance of <see cref="IListItemDto"/>.
        /// </summary>
        /// <typeparam name="T">Type implementing <see cref="IListItemDto"/>.</typeparam>
        /// <param name="_">Instance of <see cref="Shrex"/>.</param>
        /// <param name="item">List item to be be mapped into dto.</param>
        /// <returns>Mapped entity.</returns>
        public static T Map<T>(this Shrex _, ListItem item) where T : IListItemDto, new()
        {
            if (item.Fields is null) throw new NullReferenceException(nameof(item.Fields));

            T result = new()
            {
                Id = item.Id
            };

            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                if (item.Fields.AdditionalData.TryGetValue(property.Name, out var value))
                {
                    property.SetValue(result, value);
                }
            }

            return result;
        }

        /// <summary>
        /// In current state it is better to use <see cref="Map{T}(Shrex, ListItem)"/>.
        /// Auto mapping method for conversion between <see cref="ListItem"/> and instance of <see cref="IListItemDto"/> using cached mapper.
        /// </summary>
        /// <typeparam name="T">Type implementing <see cref="IListItemDto"/>.</typeparam>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of sharepoint a list.</param>
        /// <param name="item">List item to be be mapped into dto.</param>
        /// <returns>Mapped entity.</returns>
        public static T Map<T>(this Shrex sp, string listId, ListItem item) where T : IListItemDto, new()
        {
            T result = new()
            {
                Id = item.Id
            };
            var mapper = DynamicListItemMapperLibrary.GetMapper<T>(sp, listId);
            mapper.FillObject(result, item);

            return result;
        }

        /// <summary>
        /// Auto mapping method for conversion between <see cref="ListItem"/> and instance of <see cref="IListItemDto"/> using cached mapper, that converts collection.
        /// </summary>
        /// <typeparam name="T">Implementation of <see cref="IListItemDto"/>.</typeparam>
        /// <param name="_">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listItems">Collection of <see cref="ListItem"/> to be mapped.</param>
        /// <returns>Collection of mapped entities.</returns>
        public static IEnumerable<T> Map<T>(this Shrex _, IEnumerable<ListItem?> listItems) where T : IListItemDto, new()
        {
            var properties = typeof(T).GetProperties();

            return listItems.Select(x => {
                var result = new T
                {
                    Id = x.Id
                };
                foreach (var property in properties)
                {
                    if (x?.Fields?.AdditionalData?.TryGetValue(property.Name, out var value) ?? false)
                    {
                        property.SetValue(result, value);
                    }
                }
                return result;
            });
        }

        /// <summary>
        /// Auto mapping method for conversion between <see cref="ListItem"/> and instance of <see cref="IListItemDto"/> using cached mapper, that converts collection.
        /// </summary>
        /// <typeparam name="T">Implementation of <see cref="IListItemDto"/>.</typeparam>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listItems">Collection of <see cref="ListItem"/> to be mapped.</param>
        /// <param name="listId">Id of SharePoint list.</param>
        /// <returns>Collection of mapped entities.</returns>
        public static IEnumerable<T> Map<T>(this Shrex sp, IEnumerable<ListItem?> listItems, string listId) where T : IListItemDto, new()
        {
            var mapper = DynamicListItemMapperLibrary.GetMapper<T>(sp, listId);

            return listItems.Select(x => {
                var result = new T
                {
                    Id = x.Id
                };
                mapper.FillObject(result, x);
                return result;
            });
        }

        /// <summary>
        /// Extension method that fetches all options of a choice column.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of a SharePoint list.</param>
        /// <param name="fieldName">Internal name of a column.</param>
        /// <returns>Collection of string values of choices.</returns>
        /// <exception cref="ArgumentException"></exception>
        public static async Task<IEnumerable<string>> GetChoices(this Shrex sp, string listId, string fieldName)
        {
            var columns = await sp.Client.Sites[sp.SiteId].Lists[listId].Columns.GetAsync();
            var column = columns?.Value?.FirstOrDefault(x => fieldName.Equals(x.Name)) ?? throw new NullReferenceException("Choice field was not found.");
            return column?.Choice?.Choices ?? throw new NullReferenceException("Choice field was not found.");
        }
    }
}
