namespace Shrex.Filters
{
    public class LookupFilterCondition<T> : BaseFilterCondition<T>
    {
        public required FilterOperation Operation { get; init; }

        public LookupFilterCondition()
        {
            if (typeof(T) != typeof(string) && typeof(T) != typeof(int))
            {
                throw new NotSupportedException($"type {typeof(T).FullName} is not supported for LookupFilter. please use string or int");
            }
        }

        public override string GetFilterString()
        {
            string format = Operation switch
            {
                FilterOperation.Equals => "fields/{0}LookupId eq {1}",
                FilterOperation.NotEqual => "fields/{0}LookupId ne {1}",

                FilterOperation.LessThan => "fields/{0}LookupId lt {1}",
                FilterOperation.LessOrEqual => "fields/{0}LookupId le {1}",
                FilterOperation.GreaterThan => "fields/{0}LookupId gt {1}",
                FilterOperation.GreaterOrEqual => "fields/{0}LookupId ge {1}",

                FilterOperation.IsNull => "fields/{0} eq null",
                FilterOperation.IsNotNull => "fields/{0} ne null",
                _ => throw new NotSupportedException()
            };

            return string.Format(format, FieldName, GetFormattedValue());
        }

        public override string GetFormattedValue()
        {
            return Value.ToString();
        }
    }
}
