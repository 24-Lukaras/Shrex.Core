using Microsoft.Graph.Models;

namespace Shrex.Documents
{
    /// <summary>
    /// Interface defining how to create a folder structure in SharePoint document library.
    /// </summary>
    public interface IFolderStructure
    {
        /// <summary>
        /// Method that returns definitions of individual folders to be created.
        /// </summary>
        /// <returns>Collection of folder definitions to be created.</returns>
        public IReadOnlyList<FolderCreationResult> GetFolderStructure();
    }
}
