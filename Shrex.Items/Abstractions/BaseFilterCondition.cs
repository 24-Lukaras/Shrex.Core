namespace Shrex.Items.Filters
{
    /// <summary>
    /// Abstract class which serves as base for filtration classes;
    /// </summary>
    /// <typeparam name="T">Type of filtered value.</typeparam>
    public abstract class BaseFilterCondition<T> : IFilterString
    {
        /// <summary>
        /// Name of field/column on SharePoint list against which is filter applied.
        /// </summary>
        public required string FieldName { get; init; }

        /// <summary>
        /// Value used in filter.
        /// </summary>
        public required T Value { get; set; }

        /// <summary>
        /// Defines how the <see cref="Value"/> should be formatted in case <see cref="object.ToString()"/> is not enough.
        /// </summary>
        /// <returns>Formatted string of <see cref="Value"/>.</returns>
        public virtual string GetFormattedValue()
        {
            return Value?.ToString() ?? "null";
        }

        /// <inheritdoc />
        public abstract string GetFilterString();
    }
}
