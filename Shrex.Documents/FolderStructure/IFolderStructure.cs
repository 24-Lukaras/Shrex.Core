using Microsoft.Graph.Models;

namespace Shrex.Documents
{
    public interface IFolderStructure
    {
        public IReadOnlyList<string> GetFolderStructure();
    }
}
