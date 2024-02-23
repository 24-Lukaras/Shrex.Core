using MappingKey = (string listId, System.Type type);

namespace Shrex.Items.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public static class DynamicListItemMapperLibrary
    {
        private static IDictionary<MappingKey, DynamicListItemMapper> _library = new Dictionary<MappingKey, DynamicListItemMapper>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <returns></returns>
        public static DynamicListItemMapper GetMapper<T>(Shrex shrex, string listId)
        {
            MappingKey key = new MappingKey(listId, typeof(T));
            if (!_library.TryGetValue(key, out var mapper))
            {
                mapper = new DynamicListItemMapper(shrex, listId, key.type);
            }
            return mapper;
        }
    }
}
