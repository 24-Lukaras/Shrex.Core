using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace Shrex.Documents
{
    /// <summary>
    /// 
    /// </summary>
    public static class ShrexDocumentExtensions
    {
        /// <summary>
        /// Create folder structure based on <see cref="IFolderStructure"/> implementation.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="folderStructure">Implementation of <see cref="IFolderStructure"/>. For better experience use own implementation of <see cref="FolderStructure"/>.</param>
        /// <param name="listOrDriveId">Id of either SharePoint list or its drive id.</param>
        /// <param name="isDrive">Indicates if provided id is drive id. Default true.</param>
        /// <returns>Task awaiter.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static async Task CreateFolderStructure(this Shrex sp, IFolderStructure folderStructure, string listOrDriveId, bool isDrive = true)
        {
            string driveId = listOrDriveId;
            if (!isDrive)
            {
                var drive = await sp.Client.Sites[sp.SiteId].Lists[listOrDriveId].Drive.GetAsync();
                driveId = drive?.Id ?? throw new NullReferenceException(nameof(drive));
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

                await driveRequest.Root.ItemWithPath(folder.Path).PatchAsync(folderItem);

                if (!string.IsNullOrEmpty(folder.ContentType))
                {
                    var listItemIds = await driveRequest.Root.ItemWithPath(folder.Path).GetAsync(config =>
                    {
                        config.QueryParameters.Select = ["sharepointIds"];
                    });
                    if (listItemIds is not null && listItemIds.SharepointIds is not null)
                    {
                        var listItem = new ListItem
                        {
                            ContentType = new ContentTypeInfo()
                            {
                                Id = folder.ContentType,
                            }
                        };
                        await sp.Client.Sites[listItemIds.SharepointIds.SiteId].Lists[listItemIds.SharepointIds.ListId].Items[listItemIds.SharepointIds.ListItemId].PatchAsync(listItem);
                    }
                }
            }
        }

        /// <summary>
        /// Downloads stream of a file from a SharePoint document library by its path.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="path">Full path to file.</param>
        /// <param name="listOrDriveId">Id of either SharePoint list or its drive id.</param>
        /// <param name="isDrive">Indicates if provided id is drive id. Default true.</param>
        /// <returns>Downloaded stream of targeted file.</returns>
        /// <exception cref="NullReferenceException">Thrown when drive or drive item cannot be found.</exception>
        public static async Task<Stream> DownloadDriveItem(this Shrex sp, string path, string listOrDriveId, bool isDrive = true)
        {
            string driveId = listOrDriveId;
            if (!isDrive)
            {
                var drive = await sp.Client.Sites[sp.SiteId].Lists[listOrDriveId].Drive.GetAsync();
                driveId = drive?.Id ?? throw new NullReferenceException(nameof(drive));
            }

            ArgumentNullException.ThrowIfNull(driveId, nameof(driveId));

            var driveRequest = sp.Client.Drives[driveId];
            var driveItem = await driveRequest.Root.ItemWithPath(path).Content.GetAsync();
            return driveItem ?? throw new NullReferenceException(nameof(driveItem));
        }

        /// <summary>
        /// Uploads a file to folder in a SharePoint document library.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="listOrDriveId">Id of either SharePoint list or its drive id.</param>
        /// <param name="content"><see cref="Stream"/> of binary data to be uploaded.</param>
        /// <param name="path">Full path to folder of a SharePoint document library.</param>
        /// <param name="filename">Filename of a created file including extension.</param>
        /// <param name="isDrive">Indicates if provided id is drive id. Default true.</param>
        /// <returns>Task awaiter.</returns>
        /// <exception cref="NullReferenceException">Thrown when drive or folder cannot be found.</exception>
        public static async Task UploadDriveItem(this Shrex sp, string listOrDriveId, Stream content, string path, string filename, bool isDrive)
        {
            string driveId = listOrDriveId;
            if (!isDrive)
            {
                var drive = await sp.Client.Sites[sp.SiteId].Lists[listOrDriveId].Drive.GetAsync();
                driveId = drive?.Id ?? throw new NullReferenceException(nameof(drive));
            }

            ArgumentNullException.ThrowIfNull(driveId, nameof(driveId));

            var driveRequest = sp.Client.Drives[driveId];
            await driveRequest.Root.ItemWithPath($"{path}/{filename}").Content.PutAsync(content);
        }

        /// <summary>
        /// Extension method that fetches contents of a folder in a SharePoint document library based on its path.
        /// </summary>
        /// <param name="sp">Instance of <see cref="Shrex"/>.</param>
        /// <param name="path">Full path to folder of a SharePoint document library.</param>
        /// <param name="listOrDriveId">Id of either SharePoint list or its drive id.</param>
        /// <param name="isDrive">Indicates if provided id is drive id. Default true.</param>
        /// <returns>Collection of found drive items.</returns>
        /// <exception cref="NullReferenceException">Thrown when drive or folder cannot be found.</exception>
        public static async Task<IEnumerable<DriveItem>> GetDriveItems(this Shrex sp, string path, string listOrDriveId, bool isDrive = true)
        {
            string driveId = listOrDriveId;
            if (!isDrive)
            {
                var drive = await sp.Client.Sites[sp.SiteId].Lists[listOrDriveId].Drive.GetAsync();
                driveId = drive?.Id ?? throw new NullReferenceException(nameof(drive));
            }

            ArgumentNullException.ThrowIfNull(driveId, nameof(driveId));

            var driveRequest = sp.Client.Drives[driveId];
            return (await driveRequest.Root.ItemWithPath(path).Children.GetAsync())?.Value ?? throw new NullReferenceException();
        }
    }
}
