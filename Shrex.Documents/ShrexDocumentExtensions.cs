
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shrex.Documents
{
    /// <summary>
    /// 
    /// </summary>
    public static class ShrexDocumentExtensions
    {
        public static async Task CreateFolderStructure(this Shrex sp, IFolderStructure folderStructure, string listOrDriveId, bool isDrive = true)
        {
            string driveId = listOrDriveId;
            if (!isDrive)
            {
                var drive = await sp.Client.Sites[sp.SiteId].Lists[listOrDriveId].Drive.GetAsync();
                driveId = drive?.Id;
            }

            ArgumentNullException.ThrowIfNull(driveId, nameof(driveId));

            var driveRequest = sp.Client.Drives[driveId];

            var folders = folderStructure.GetFolderStructure();
            foreach (var folder in folders)
            {
                var folderItem = new DriveItem()
                {
                    Folder = new Folder(),
                };
                await driveRequest.Root.ItemWithPath(folder).PatchAsync(folderItem);
            }
        }

        public static async Task<Stream> DownloadDriveItem(this Shrex sp, string path, string listOrDriveId, bool isDrive = true)
        {
            string driveId = listOrDriveId;
            if (!isDrive)
            {
                var drive = await sp.Client.Sites[sp.SiteId].Lists[listOrDriveId].Drive.GetAsync();
                driveId = drive?.Id;
            }

            ArgumentNullException.ThrowIfNull(driveId, nameof(driveId));

            var driveRequest = sp.Client.Drives[driveId];
            return await driveRequest.Root.ItemWithPath(path).Content.GetAsync();
        }

        public static async Task<IEnumerable<DriveItem>> GetDriveItems(this Shrex sp, string path, string listOrDriveId, bool isDrive = true)
        {
            string driveId = listOrDriveId;
            if (!isDrive)
            {
                var drive = await sp.Client.Sites[sp.SiteId].Lists[listOrDriveId].Drive.GetAsync();
                driveId = drive?.Id;
            }

            ArgumentNullException.ThrowIfNull(driveId, nameof(driveId));

            var driveRequest = sp.Client.Drives[driveId];
            return (await driveRequest.Root.ItemWithPath(path).Children.GetAsync()).Value;
        }
    }
}
