namespace Shrex.Items.Mapping
{
    /// <summary>
    /// Interface meant to be used by classes automatically mapped with <see cref="Microsoft.Graph.Models.ListItem"/>
    /// </summary>
    public interface IListItemDto
    {
        /// <summary>
        /// Property for storing the Id of <see cref="Microsoft.Graph.Models.ListItem"/>.
        /// </summary>
        public string? Id { get; set; }
    }
}
