namespace Shrex.Items.Filters
{
    /// <summary>
    /// Class for filtration on "Date and time" columns.
    /// </summary>
    public class DateTimeCondition : BaseFilterCondition<DateTime>
    {
        /// <summary>
        /// Declares which <see cref="FilterOperation"/> should be applied in the filter query.
        /// </summary>
        public required FilterOperation Operation { get; init; }

        /// <inheritdoc />
        /// <exception cref="NotSupportedException">Thrown when unsupported <see cref="FilterOperation"/> is used.</exception>
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

        /// <inheritdoc />
        public override string GetFormattedValue()
        {
            return $"'{Value:O}'";
        }
    }
}
