using Shrex.Items.Abstractions;

namespace Shrex.Items
{
    public class DefaultExpandQuery : IExpandQuery
    {
        public static DefaultExpandQuery Instance { get; } = new DefaultExpandQuery();

        public string[] GetExpandQuery() => ["fields"];
    }
}
