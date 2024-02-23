namespace Shrex.Items.Filters
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Type of filtered value.</typeparam>
    public abstract class BaseFilterCondition<T> : IFilterString
    {
        /// <summary>
        /// 
        /// </summary>
        public required string FieldName { get; init; }

        /// <summary>
        /// 
        /// </summary>
        public required T Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract string GetFormattedValue();

        /// <inheritdoc />
        public abstract string GetFilterString();
    }
}
