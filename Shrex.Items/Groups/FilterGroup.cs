using Shrex.Items.Filters;

namespace Shrex.Items
{
    /// <summary>
    /// Class used for grouping filters.
    /// </summary>
    public class FilterGroup : IFilterString
    {
        /// <summary>
        /// Logical operation applied to group of filters.
        /// </summary>
        public readonly FilterGroupOperation Operation;

        /// <summary>
        /// Collection of <see cref="IFilterString"/>.
        /// </summary>
        public IEnumerable<IFilterString> Filters;

        /// <summary>
        /// Default constructor for <see cref="FilterGroup"/>
        /// </summary>
        /// <param name="operation">Logical operation applied to group of filters.</param>
        /// <param name="filters">Collection of <see cref="IFilterString"/>. Can be instantiated as multiple parameters.</param>
        public FilterGroup(FilterGroupOperation operation, params IFilterString[] filters)
        {
            Operation = operation;
            Filters = filters;
        }

        /// <inheritdoc/>
        /// <exception cref="ArgumentException"></exception>
        public string GetOperationString()
        {
            switch (Operation)
            {
                default:
                    throw new ArgumentException();

                case FilterGroupOperation.Or:
                    return "or";

                case FilterGroupOperation.And:
                    return "and";
            }
        }

        /// <inheritdoc/>
        public string GetFilterString()
        {
            return $"({string.Join($" {GetOperationString()} ", Filters.Select(x => x.GetFilterString()))})";
        }
    }
}
