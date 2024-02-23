namespace Shrex.Items.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class DateTimeFilterCondition : BaseFilterCondition<DateTime>
    {
        /// <summary>
        /// 
        /// </summary>
        public required FilterOperation Operation { get; init; }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException"></exception>
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetFormattedValue()
        {
            return $"'{Value:O}'";
        }
    }
}
