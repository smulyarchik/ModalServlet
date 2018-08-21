namespace ModalHandler.Interfaces.Dialogs
{
    /// <summary>
    ///     Generic file upload dialog representation.
    /// </summary>
    public interface IFileUploadDialog : IDialog
    {
        /// <summary>
        /// Send the file name to the appropriate text box.
        /// </summary>
        /// <param name="fileName">Absolute path to the file.</param>
        /// <returns>Operation result.</returns>
        bool SetFileName(string fileName);
        /// <summary>
        /// Open the file by clicking the appropriate button.
        /// </summary>
        /// <returns>Operation result.</returns>
        bool Open();
    }
}