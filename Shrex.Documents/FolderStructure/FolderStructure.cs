using Microsoft.Graph.Models;
using System.Text;

namespace Shrex.Documents
{
    /// <summary>
    /// Generic class to be inherited by folder structure implementations.
    /// </summary>
    public abstract class FolderStructure : IFolderStructure
    {
        /// <summary>
        /// Definition of folder structure. Please use <see cref="Root"/> method as initial entry point.
        /// </summary>
        public abstract FolderStructureBuilder Structure { get; }

        /// <inheritdoc/>
        public IReadOnlyList<FolderCreationResult> GetFolderStructure()
        {
            List<FolderCreationResult> folders = [];
            foreach (var node in Structure.Root.Nodes)
            {
                folders.AddRange(node.GetFolderStructure());
            }
            return folders;
        }

        /// <summary>
        /// Method to be used as entry point of <see cref="Structure"/>, which indicates root of document library.
        /// </summary>
        /// <returns><see cref="FolderStructureBuilder"/> located at root of document library.</returns>
        protected FolderStructureBuilder Root()
        {
            return new FolderStructureBuilder(new RootNode() { Name = string.Empty });
        }
    }
}
