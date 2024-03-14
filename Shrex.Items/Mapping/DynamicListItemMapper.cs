using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Reflection;

namespace Shrex.Items.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="type"></param>
    public class DynamicListItemMapper(Type type)
    {
        private readonly Type _type = type;
        private readonly PropertyInfo[] _properties = type.GetProperties();

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
