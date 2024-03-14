namespace Shrex.Items.Filters
{
    /// <summary>
    /// Class for filtration on "Lookup" columns. Provide lookup id in the Value property.
    /// </summary>
    /// <typeparam name="T">Type of filtered value. Either <see cref="int"/> or <see cref="string"/>.</typeparam>
    public class LookupCondition<T> : BaseFilterCondition<T>
    {
        /// <summary>
        /// Declares which <see cref="FilterOperation"/> should be applied in the filter query.
        /// </summary>
        public required FilterOperation Operation { get; init; }

        /// <summary>
        /// Class for filtration on "Lookup" columns. Provide lookup id in the Value property.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when type not suitable for LookupId value is provided.</exception>
        public LookupCondition()
        {
            if (typeof(T) != typeof(string) && typeof(T) != typeof(int))
            {
                throw new NotSupportedException($"type {typeof(T).FullName} is not supported for LookupFilter. please use string or int");
            }
        }

        /// <inheritdoc/>
        /// <exception cref="NotSupportedException">Thrown when unsupported <see cref="FilterOperation"/> is used.</exception>
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
    }
}
