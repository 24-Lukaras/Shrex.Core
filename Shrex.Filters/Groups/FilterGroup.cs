namespace Shrex.Filters
{
    public class FilterGroup : IFilterString
    {
        public readonly FilterGroupOperation Operation;
        public IEnumerable<IFilterString> Filters;

        public FilterGroup(FilterGroupOperation operation, params IFilterString[] filters)
        {
            Operation = operation;
            Filters = filters;
        }

        public string GetOperationString()
        {
            switch (Operation)
            {
                default:
                    throw new ArgumentException("");

                case FilterGroupOperation.Or:
                    return "or";

                case FilterGroupOperation.And:
                    return "and";
            }
        }

        public string GetFilterString()
        {
            return $"({string.Join($" {GetOperationString()} ", Filters.Select(x => x.GetFilterString()))})";
        }
    }
}
