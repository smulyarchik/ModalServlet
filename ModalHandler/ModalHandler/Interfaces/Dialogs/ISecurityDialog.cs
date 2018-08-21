namespace ModalHandler.Interfaces.Dialogs
{
    /// <summary>
    ///     Generic security dialog representation.
    /// </summary>
    public interface ISecurityDialog : IDialog
    {
        /// <summary>
        /// Check if the dialog is ready to accept credentials input.
        /// </summary>
        bool CanAcceptCredentials { get; }
        /// <summary>
        /// Send the credentials to the dialog.
        /// </summary>
        /// <param name="username">User name for the authentication.</param>
        /// <param name="password">Password for the authentication.</param>
        /// <returns>Operation result.</returns>
        bool EnterCredentials(string username, string password);
        /// <summary>
        /// Subbit the credentials by clicking the appropriate button.
        /// </summary>
        /// <returns>Operation result.</returns>
        bool Submit();
    }
}