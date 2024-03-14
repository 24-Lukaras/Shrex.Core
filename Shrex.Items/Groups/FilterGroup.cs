using Shrex.Items.Filters;

namespace Shrex.Items
{
    /// <summary>
    /// Class used for grouping filters.
    /// </summary>
    /// <remarks>
    /// Default constructor for <see cref="FilterGroup"/>
    /// </remarks>
    /// <param name="operation">Logical operation applied to group of filters.</param>
    /// <param name="filters">Collection of <see cref="IFilterString"/>. Can be instantiated as multiple parameters.</param>
    public class FilterGroup(FilterGroupOperation operation, params IFilterString[] filters) : IFilterString
    {
        /// <summary>
        /// Logical operation applied to group of filters.
        /// </summary>
        public readonly FilterGroupOperation Operation = operation;

        /// <summary>
        /// Collection of <see cref="IFilterString"/>.
        /// </summary>
        public readonly IEnumerable<IFilterString> Filters = filters;

        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Thrown when unsupported <see cref="FilterGroupOperation"/> is used.</exception>
        public string GetOperationString()
        {
            return Operation switch
            {
                FilterGroupOperation.Or => "or",
                FilterGroupOperation.And => "and",
                _ => throw new NotSupportedException(),
            };
        }

        /// <inheritdoc/>
        public string GetFilterString()
        {
            return $"({string.Join($" {GetOperationString()} ", Filters.Select(x => x.GetFilterString()))})";
        }
    }
}
