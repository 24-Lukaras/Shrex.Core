using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shrex.Documents
{

    public class FolderStructureBuilder
    {
        private readonly RootNode _root;
        public FolderStructureNode CurrentNode { get; set; }

        public FolderStructureBuilder(RootNode root)
        {
            _root = root;
            CurrentNode = _root;
        }

        public delegate void SubFolderBuilder(FolderStructureBuilder builder);

        public FolderStructureBuilder Folder(string folderName)
        {
            var folder = new FolderNode() { Name = folderName };
            CurrentNode.Nodes.Add(folder);
            return this;
        }

        public FolderStructureBuilder Folder(string folderName, SubFolderBuilder subFolderBuilderFunction)
        {
            var folder = new FolderNode() { Name = folderName };
            CurrentNode.Nodes.Add(folder);

            var subFolderBuilder = new FolderStructureBuilder(new RootNode() { Name = string.Empty }) { CurrentNode = folder };
            subFolderBuilderFunction(subFolderBuilder);

            return this;
        }

        public FolderStructureBuilder FolderIf(string folderName, Func<bool> createEntryFunction)
        {
            var folder = new FolderIfNode(createEntryFunction) { Name = folderName };
            CurrentNode.Nodes.Add(folder);
            return this;
        }

        public FolderStructureBuilder FolderIf(string folderName, Func<bool> createEntryFunction, SubFolderBuilder subFolderBuilderFunction)
        {
            var folder = new FolderIfNode(createEntryFunction) { Name = folderName };
            CurrentNode.Nodes.Add(folder);

            var subFolderBuilder = new FolderStructureBuilder(new RootNode() { Name = string.Empty }) { CurrentNode = folder };
            subFolderBuilderFunction(subFolderBuilder);

            return this;
        }

        public FolderStructureBuilder Navigate(string folderName)
        {
            var nav = new NavigateNode() { Name = folderName };
            CurrentNode.Nodes.Add(nav);
            CurrentNode = nav;
            return this;
        }

        public RootNode Exec()
        {
            return _root;
        }
    }

    public class RootNode : FolderStructureNode
    {
        public override bool CreatesEntry => false;

        public override bool GoDeeper => true;
    }

    public class FolderNode : FolderStructureNode
    {
        public override bool CreatesEntry => true;

        public override bool GoDeeper => true;
    }

    public class FolderIfNode : FolderStructureNode
    {
        public FolderIfNode(Func<bool> createsEntryFunction)
        {
            _createsEntry = createsEntryFunction;
        }

        private Func<bool> _createsEntry;
        public override bool CreatesEntry => _createsEntry.Invoke();

        public override bool GoDeeper => _createsEntry.Invoke();
    }

    public class NavigateNode : FolderStructureNode
    {
        public override bool CreatesEntry => false;

        public override bool GoDeeper => true;
    }

    public abstract class FolderStructureNode
    {
        public required string Name { get; init; }

        public abstract bool CreatesEntry { get; }

        public abstract bool GoDeeper { get; }

        public List<FolderStructureNode> Nodes { get; } = new List<FolderStructureNode>();

        public IReadOnlyCollection<string> GetFolderStructure(string? prevPath = null)
        {            
            List<string> folders = new List<string>();

            string currentPath = string.IsNullOrEmpty(prevPath) ? Name : $"{prevPath}/{Name}";

            if (CreatesEntry)
            {
                folders.Add(currentPath);
            }
            if (GoDeeper)
            {
                foreach (var node in Nodes)
                {
                    folders.AddRange(node.GetFolderStructure(currentPath));
                }
            }            

            return folders;
        }
    }
}
