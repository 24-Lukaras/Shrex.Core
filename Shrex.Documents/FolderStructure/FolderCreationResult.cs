namespace Shrex.Documents
{
    /// <summary>
    /// Class for folder structure creation.
    /// </summary>
    public class FolderCreationResult
    {
        /// <summary>
        /// Full path of folder to be created.
        /// </summary>
        public required string Path { get; set; }

        /// <summary>
        /// Name of folder to be created.
        /// </summary>
        public string FolderName => Path.Split('/').Last();

        /// <summary>
        /// Content type id, in case there is need to alter folder to document set or else.
        /// </summary>
        public string? ContentType { get; set; }

    }
}
