using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Reflection;

namespace Shrex.Items.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public class DynamicListItemMapper
    {
        private Type _type;
        private PropertyInfo[] _properties;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shrex"></param>
        /// <param name="listId"></param>
        /// <param name="type"></param>
        public DynamicListItemMapper(Shrex shrex, string listId, Type type)
        {
            _type = type;
            _properties = type.GetProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="item"></param>
        public void FillObject<T>(T entity, ListItem item)
        {
            ArgumentNullException.ThrowIfNull(item.Fields);

            foreach (var property in _properties)
            {
                if (item.Fields.AdditionalData.TryGetValue(property.Name, out var value))
                {
                    property.SetValue(entity, value);
                }
            }
        }

    }
}
