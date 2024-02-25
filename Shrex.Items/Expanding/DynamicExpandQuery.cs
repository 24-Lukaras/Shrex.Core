using Shrex.Items.Abstractions;

namespace Shrex.Items
{
    public class DynamicExpandQuery<T> : IExpandQuery
    {
        public string[] GetExpandQuery()
        {
            var properties = typeof(T).GetProperties();

            return [ $"fields($select={string.Join(",", properties.Select(x => x.Name))})"];
        }
    }
}
