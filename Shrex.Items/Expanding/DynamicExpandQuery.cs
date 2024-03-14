using Shrex.Items.Abstractions;
using Shrex.Items.Mapping;

namespace Shrex.Items
{
    /// <summary>
    /// Expand query which includes all fields of list item based on properties of provided type.
    /// </summary>
    /// <typeparam name="T">Type inheriting <see cref="IListItemDto"/> that defines which columns should be returned base on properties.</typeparam>
    public class DynamicExpandQuery<T> : IExpandQuery where T : IListItemDto
    {
        /// <inheritdoc/>
        public string[] GetExpandQuery()
        {
            var properties = typeof(T).GetProperties();

            return [ $"fields($select={string.Join(",", properties.Select(x => x.Name))})"];
        }
    }
}
