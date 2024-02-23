namespace Shrex.Items.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class StringCondition : FilterCondition<string>
    {
        /// <inheritdoc/>
        public override bool UseQuotationMarks => true;
    }
}
