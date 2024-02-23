namespace Shrex.Items.Filters
{
    /// <summary>
    /// Enumerator with all operations that can be applied to graph $filter query.
    /// Intended for usage in filtration classes (e.g. <see cref="FilterCondition{T}"/>).
    /// Filtration throws <see cref="NotSupportedException"/> in case of an operation that isn't applicable.
    /// </summary>
    public enum FilterOperation
    {
        /// <summary>
        /// 'lt' used for filtering items with lower value than was set in filter
        /// </summary>
        LessThan,

        /// <summary>
        /// 'le' used for filtering items with lower or same value than was set in filter
        /// </summary>
        LessOrEqual,

        /// <summary>
        /// 'gt' used for filtering items with higher value than was set in filter
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 'ge' used for filtering items with higher or same value than was set in filter
        /// </summary>
        GreaterOrEqual,


        /// <summary>
        /// 'eq' used for filtering items with same value than was set in filter
        /// </summary>
        Equals,

        /// <summary>
        /// 'ne' used for filtering items with different value than was set in filter
        /// </summary>
        NotEqual,

        /// <summary>
        /// 
        /// </summary>
        In,

        /// <summary>
        /// 
        /// </summary>
        Has,

        /// <summary>
        /// 
        /// </summary>
        Any,

        /// <summary>
        /// 
        /// </summary>
        All,

        /// <summary>
        /// 'startswith(fields/{field},{value})' used for filtering items where filtered column begins with filter value
        /// </summary>
        StartsWith,

        /// <summary>
        /// 
        /// </summary>
        EndsWith,

        /// <summary>
        /// 
        /// </summary>
        Contains,


        /// <summary>
        /// 'fields/{field} ne null' used for filtering items that have some value in filtered column
        /// </summary>
        IsNotNull,

        /// <summary>
        /// 'fields/{field} eq null' used for filtering items that do not value in filtered column
        /// </summary>
        IsNull
    }
}
