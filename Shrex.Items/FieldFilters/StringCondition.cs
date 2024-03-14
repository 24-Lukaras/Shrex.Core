namespace Shrex.Items.Filters
{
    /// <summary>
    /// Class for filtration on "Text" columns.
    /// </summary>
    public class StringCondition : BaseFilterCondition<string>
    {
        /// <summary>
        /// Declares which <see cref="FilterOperation"/> should be applied in the filter query.
        /// </summary>
        public required FilterOperation Operation { get; init; }

        /// <inheritdoc/>>
        /// <exception cref="NotSupportedException">Thrown when unsupported <see cref="FilterOperation"/> is used.</exception>
        public override string GetFilterString()
        {
            string format = Operation switch
            {
                FilterOperation.Equals => "fields/{0}LookupId eq {1}",
                FilterOperation.NotEqual => "fields/{0}LookupId ne {1}",

                FilterOperation.StartsWith => "startswith(fields/{0},{1})",

                FilterOperation.IsNull => "fields/{0} eq null",
                FilterOperation.IsNotNull => "fields/{0} ne null",
                _ => throw new NotSupportedException()
            };

            return string.Format(format, FieldName, GetFormattedValue());
        }

        /// <inheritdoc/>
        public override string GetFormattedValue()
        {
            return $"'{Value}'";
        }
    }
}
