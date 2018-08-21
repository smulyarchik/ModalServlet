namespace ModalHandler.Interfaces.Dialogs
{
    /// <summary>
    ///     Generic representation of a top level dialog that has a chaining capacity.
    /// </summary>
    public interface ITopLevelDialog : IDialog
    {
        /// <summary>
        /// Get the next visible top level modal dialog.
        /// </summary>
        ITopLevelDialog NextTopLevel { get; }
    }
}