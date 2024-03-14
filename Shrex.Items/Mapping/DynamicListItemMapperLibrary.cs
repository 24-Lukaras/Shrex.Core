using MappingKey = (string listId, System.Type type);

namespace Shrex.Items.Mapping
{
    /// <summary>
    /// Singleton class for caching instances of <see cref="DynamicListItemMapper"/> paired with list id and item type (instance of <see cref="IListItemDto"/>)
    /// </summary>
    public static class DynamicListItemMapperLibrary
    {
        private static readonly Dictionary<MappingKey, DynamicListItemMapper> _library = [];

        /// <summary>
        /// Fetches already existing or newly created mapper based on provided type and list id.
        /// </summary>
        /// <typeparam name="T">Implementation of <see cref="IListItemDto"/>.</typeparam>
        /// <param name="_">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listId">Id of a SharePoint list.</param>
        /// <returns>Found or newly created mapper.</returns>
        public static DynamicListItemMapper GetMapper<T>(Shrex _, string listId) where T : IListItemDto, new()
        {
            MappingKey key = new(listId, typeof(T));
            if (!_library.TryGetValue(key, out var mapper))
            {
                mapper = new DynamicListItemMapper(key.type);
            }
            return mapper;
        }
    }
}
