namespace Shrex.Filters
{
    public class DateTimeFilterCondition : BaseFilterCondition<DateTime>
    {
        public required FilterOperation Operation { get; init; }

        public override string GetFilterString()
        {
            string format = Operation switch
            {
                FilterOperation.Equals => "fields/{0}/datetime eq {1}",
                FilterOperation.NotEqual => "fields/{0}/datetime ne {1}",

                FilterOperation.LessThan => "fields/{0}/datetime lt {1}",
                FilterOperation.LessOrEqual => "fields/{0}/datetime le {1}",
                FilterOperation.GreaterThan => "fields/{0}/datetime gt {1}",
                FilterOperation.GreaterOrEqual => "fields/{0}/datetime ge {1}",

                FilterOperation.IsNull => "fields/{0} eq null",
                FilterOperation.IsNotNull => "fields/{0} ne null",
                _ => throw new NotSupportedException()
            };

            return string.Format(format, FieldName, GetFormattedValue());
        }

        public override string GetFormattedValue()
        {
            return $"'{Value:O}'";
        }
    }
}
