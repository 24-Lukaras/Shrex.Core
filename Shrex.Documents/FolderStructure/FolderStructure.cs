using Microsoft.Graph.Models;
using System.Text;

namespace Shrex.Documents
{
    public abstract class FolderStructure : IFolderStructure
    {
        public abstract RootNode Structure { get; }

        public IReadOnlyList<string> GetFolderStructure()
        {
            List<string> folders = new List<string>();
            foreach (var node in Structure.Nodes)
            {
                folders.AddRange(node.GetFolderStructure());
            }
            return folders;
        }

        protected FolderStructureBuilder Root()
        {
            return new FolderStructureBuilder(new RootNode() { Name = string.Empty });
        }
    }
}
