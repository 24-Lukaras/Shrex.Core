namespace Shrex.Filters
{
    public abstract class BaseFilterCondition<T> : IFilterString
    {
        public required string FieldName { get; init; }

        public required T Value { get; set; }

        public abstract string GetFormattedValue();

        public abstract string GetFilterString();
    }
}
