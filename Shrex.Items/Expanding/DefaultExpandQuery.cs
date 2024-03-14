using Shrex.Items.Abstractions;

namespace Shrex.Items
{
    /// <summary>
    /// Expand query which includes all field of list item excluding lookup values
    /// </summary>
    public class DefaultExpandQuery : IExpandQuery
    {
        /// <summary>
        /// Singleton instance of <see cref="DefaultExpandQuery"/>.
        /// </summary>
        public static DefaultExpandQuery Instance { get; } = new DefaultExpandQuery();

        /// <inheritdoc/>>
        public string[] GetExpandQuery() => ["fields"];
    }
}
