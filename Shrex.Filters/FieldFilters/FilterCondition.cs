namespace Shrex.Filters
{
    public class FilterCondition<T> : BaseFilterCondition<T>
    {
        public required FilterOperation Operation { get; init; }
        public virtual bool UseQuotationMarks { get; init; }

        public override string GetFilterString()
        {
            string format = Operation switch
            {
                FilterOperation.Equals => "fields/{0} eq {1}",
                FilterOperation.NotEqual => "fields/{0} ne {1}",

                FilterOperation.LessThan => "fields/{0} lt {1}",
                FilterOperation.LessOrEqual => "fields/{0} le {1}",
                FilterOperation.GreaterThan => "fields/{0} gt {1}",
                FilterOperation.GreaterOrEqual => "fields/{0} ge {1}",

                FilterOperation.StartsWith => "startswith(fields/{0},{1})",

                FilterOperation.IsNull => "fields/{0} eq null",
                FilterOperation.IsNotNull => "fields/{0} ne null",
                _ => throw new NotSupportedException()
            };

            return string.Format(format, FieldName, GetFormattedValue());
        }

        public override string GetFormattedValue()
        {
            return UseQuotationMarks ? $"'{Value}'" : $"{Value}";
        }
    }
}
