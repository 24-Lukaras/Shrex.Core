namespace Shrex.Filters
{
    public enum FilterOperation
    {
        LessThan,
        LessOrEqual,
        GreaterThan,
        GreaterOrEqual,

        Equals,
        NotEqual,
        In,
        Has,

        StartsWith,
        EndsWith,
        Contains,

        IsNotNull,
        IsNull
    }
}
