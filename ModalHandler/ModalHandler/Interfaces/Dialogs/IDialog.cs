using System;
using ModalHandler.Interfaces.Elements;

namespace ModalHandler.Interfaces.Dialogs
{
    /// <summary>
    /// Representation of a generic dialog.
    /// </summary>
    public interface IDialog : IElement
    {
        /// <summary>
        /// Kill dialog window.
        /// </summary>
        void Close();

        /// <summary>
        /// Internal timeout for interacting with elements.
        /// </summary>
        TimeSpan Timeout { get; set; }
    }
}