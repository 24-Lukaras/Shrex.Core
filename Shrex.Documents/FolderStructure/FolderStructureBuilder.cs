using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Shrex.Documents
{
    /// <summary>
    /// Builder that creates definition of folder structure.
    /// </summary>
    public class FolderStructureBuilder
    {
        /// <summary>
        /// Node that signifies root of the builder or document library.
        /// </summary>
        public RootNode Root { get; init; }

        /// <summary>
        /// Node that is currently being processed.
        /// </summary>
        public FolderStructureNode CurrentNode { get; set; }

        /// <summary>
        /// Creates instance of <see cref="FolderStructureBuilder"/>. Please use <see cref="FolderStructure.Root"/> inside <see cref="FolderStructure"/> instead.
        /// </summary>
        /// <param name="root">Root node of the builder.</param>
        public FolderStructureBuilder(RootNode root)
        {
            Root = root;
            CurrentNode = Root;
        }

        /// <summary>
        /// Delegate method for creation of subfolder of created folder with <see cref="Folder(string)"/>.
        /// </summary>
        /// <param name="builder"><see cref="FolderStructureBuilder"/> with empty <see cref="Root" /> node and parent folder set as <see cref="CurrentNode"/>.</param>
        public delegate void SubFolderBuilder(FolderStructureBuilder builder);

        /// <summary>
        /// Creates a <see cref="FolderIfNode"/> in folder structure.
        /// </summary>
        /// <param name="folderName">Name of created folder.</param>
        /// <returns><see cref="FolderStructureBuilder"/> that is being currently used.</returns>
        public FolderStructureBuilder Folder(string folderName)
        {
            var folder = new FolderNode() { Name = folderName };
            CurrentNode.Nodes.Add(folder);
            return this;
        }

        /// <summary>
        /// Creates a <see cref="FolderIfNode"/> in folder structure.
        /// </summary>
        /// <param name="folderName">Name of created folder.</param>
        /// <param name="contentType">Content type id to be assigned to created folder.</param>
        /// <returns><see cref="FolderStructureBuilder"/> that is being currently used.</returns>
        public FolderStructureBuilder Folder(string folderName, string contentType)
        {
            var folder = new FolderNode() { Name = folderName, ContentTypeId = contentType };
            CurrentNode.Nodes.Add(folder);
            return this;
        }

        /// <summary>
        /// Creates a <see cref="FolderIfNode"/> in folder structure.
        /// </summary>
        /// <param name="folderName">Name of created folder.</param>
        /// <param name="subFolderBuilderFunction">Lambda function which defines subfolders of created folder. Example: (folder) => { folder.Folder("NewFolder2"); }</param>
        /// <returns><see cref="FolderStructureBuilder"/> that is being currently used.</returns>
        public FolderStructureBuilder Folder(string folderName, SubFolderBuilder subFolderBuilderFunction)
        {
            var folder = new FolderNode() { Name = folderName };
            CurrentNode.Nodes.Add(folder);

            var subFolderBuilder = new FolderStructureBuilder(new RootNode() { Name = string.Empty }) { CurrentNode = folder };
            subFolderBuilderFunction(subFolderBuilder);

            return this;
        }

        /// <summary>
        /// Creates a <see cref="FolderIfNode"/> in folder structure.
        /// </summary>
        /// <param name="folderName">Name of created folder.</param>
        /// <param name="contentType">Content type id to be assigned to created folder.</param>
        /// <param name="subFolderBuilderFunction">Lambda function which defines subfolders of created folder. Example: (folder) => { folder.Folder("NewFolder2"); }</param>
        /// <returns><see cref="FolderStructureBuilder"/> that is being currently used.</returns>
        public FolderStructureBuilder Folder(string folderName, string contentType, SubFolderBuilder subFolderBuilderFunction)
        {
            var folder = new FolderNode() { Name = folderName, ContentTypeId = contentType };
            CurrentNode.Nodes.Add(folder);

            var subFolderBuilder = new FolderStructureBuilder(new RootNode() { Name = string.Empty }) { CurrentNode = folder };
            subFolderBuilderFunction(subFolderBuilder);

            return this;
        }

        /// <summary>
        /// Creates a <see cref="FolderIfNode"/> in folder structure.
        /// </summary>
        /// <param name="folderName">Name of created folder.</param>
        /// <param name="createEntry">Defines if folder should be created.</param>
        /// <returns><see cref="FolderStructureBuilder"/> that is being currently used.</returns>
        public FolderStructureBuilder FolderIf(string folderName, bool createEntry)
        {
            var folder = new FolderIfNode(createEntry) { Name = folderName };
            CurrentNode.Nodes.Add(folder);
            return this;
        }

        /// <summary>
        /// Creates a <see cref="FolderIfNode"/> in folder structure.
        /// </summary>
        /// <param name="folderName">Name of created folder.</param>
        /// <param name="createEntry">Defines if folder should be created.</param>
        /// <param name="subFolderBuilderFunction">Lambda function which defines subfolders of created folder. Example: (folder) => { folder.Folder("NewFolder2"); }</param>
        /// <returns><see cref="FolderStructureBuilder"/> that is being currently used.</returns>
        public FolderStructureBuilder FolderIf(string folderName, bool createEntry, SubFolderBuilder subFolderBuilderFunction)
        {
            var folder = new FolderIfNode(createEntry) { Name = folderName };
            CurrentNode.Nodes.Add(folder);

            var subFolderBuilder = new FolderStructureBuilder(new RootNode() { Name = string.Empty }) { CurrentNode = folder };
            subFolderBuilderFunction(subFolderBuilder);

            return this;
        }

        /// <summary>
        /// Go into already existing folder.
        /// </summary>
        /// <param name="folderName">Name of existing folder.</param>
        /// <returns><see cref="FolderStructureBuilder"/> that is being currently used.</returns>
        public FolderStructureBuilder Navigate(string folderName)
        {
            var nav = new NavigateNode() { Name = folderName };
            CurrentNode.Nodes.Add(nav);
            CurrentNode = nav;
            return this;
        }
    }

    /// <summary>
    /// <see cref="FolderStructureNode"/> which signifies root of document library.
    /// </summary>
    public class RootNode : FolderStructureNode
    {
        /// <inheritdoc/>
        public override bool CreatesEntry => false;

        /// <inheritdoc/>
        public override bool GoDeeper => true;
    }

    /// <summary>
    /// <see cref="FolderStructureNode"/> which signifies new folder.
    /// </summary>
    public class FolderNode : FolderStructureNode
    {
        /// <inheritdoc/>
        public override bool CreatesEntry => true;

        /// <inheritdoc/>
        public override bool GoDeeper => true;
    }

    /// <summary>
    /// <see cref="FolderStructureNode"/> which signifies new folder, that is being created base of a condition.
    /// </summary>
    /// <remarks>
    /// Default <see cref="FolderIfNode"/> constructor.
    /// </remarks>
    /// <param name="createsEntry">Condition/boolean which signifies is a folder should be created.</param>
    public class FolderIfNode(bool createsEntry) : FolderStructureNode
    {
        private readonly bool _createsEntry = createsEntry;
        /// <inheritdoc/>
        public override bool CreatesEntry => _createsEntry;

        /// <inheritdoc/>
        public override bool GoDeeper => _createsEntry;
    }

    /// <summary>
    /// <see cref="FolderStructureNode"/> which signifies going into existing folder.
    /// </summary>
    public class NavigateNode : FolderStructureNode
    {
        /// <inheritdoc/>
        public override bool CreatesEntry => false;

        /// <inheritdoc/>
        public override bool GoDeeper => true;
    }

    /// <summary>
    /// Abstract class that contains properties of different node types, that can define folder structure.
    /// </summary>
    public abstract class FolderStructureNode
    {
        /// <summary>
        /// Name of node to be navigated to from parent node.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Content type id for cases when content type of folder needs to be altered after creation. For example conversion to document set.
        /// </summary>
        public string? ContentTypeId { get; init; }

        /// <summary>
        /// Signifies if this nodes creates a folder.
        /// </summary>
        public abstract bool CreatesEntry { get; }

        /// <summary>
        /// Structure creation ignores children nodes in case of false.
        /// </summary>
        public abstract bool GoDeeper { get; }

        /// <summary>
        /// Children nodes.
        /// </summary>
        public List<FolderStructureNode> Nodes { get; } = [];

        /// <summary>
        /// Method that creates collection of folders that needs to be created.
        /// </summary>
        /// <param name="prevPath">Path of parent node.</param>
        /// <returns>Collection of recursively created folders and subfolders.</returns>
        public IReadOnlyCollection<FolderCreationResult> GetFolderStructure(string? prevPath = null)
        {            
            List<FolderCreationResult> folders = [];

            string currentPath = string.IsNullOrEmpty(prevPath) ? Name : $"{prevPath}/{Name}";

            if (CreatesEntry)
            {
                folders.Add(new FolderCreationResult()
                {
                    Path = currentPath,
                    ContentType = ContentTypeId,
                });
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
