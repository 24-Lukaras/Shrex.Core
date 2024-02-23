
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace Shrex.Documents
{
    /// <summary>
    /// 
    /// </summary>
    public static class ShrexDocumentExtensions
    {
        public static async Task CreateFolderStructure(this Shrex sp, IFolderStructure folderStructure, string listOrDriveId, bool isDrive = true)
        {
            if (isDrive)
            {
                await sp.CreateFolderStructureDrive(folderStructure,listOrDriveId);
            }
            await sp.CreateFolderStructureList(folderStructure, listOrDriveId);            
        }

        private static async Task CreateFolderStructureDrive(this Shrex shrex, IFolderStructure folderStucture, string? driveId)
        {
            ArgumentNullException.ThrowIfNull(driveId, nameof(driveId));

            var drive = shrex.Client.Drives[driveId];

            var folders = folderStucture.GetFolderStructure();
            foreach (var folder in folders)
            {
                var folderItem = new DriveItem()
                {
                    Folder = new Folder(),
                };
                await drive.Root.ItemWithPath(folder).PatchAsync(folderItem);
            }
        }

        private static async Task CreateFolderStructureList(this Shrex shrex, IFolderStructure folderStucture, string listId)
        {
            var drive = await shrex.Client.Sites[shrex.SiteId].Lists[listId].Drive.GetAsync();

            await CreateFolderStructureDrive(shrex, folderStucture, drive?.Id);
        }
    }
}
