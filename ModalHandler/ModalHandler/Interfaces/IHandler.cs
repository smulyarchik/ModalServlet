using System;

namespace ModalHandler.Interfaces
{
    /// <summary>
    ///     Generic disposable handler representation.
    /// </summary>
    public interface IHandler : IDisposable
    {
        /// <summary>
        ///     Check if the current handler can handle the found dialog.
        /// </summary>
        bool CanHandle { get; }

        /// <summary>
        ///     Check if the found dialog is successfully handled.
        /// </summary>
        bool IsHandled { get; }

        /// <summary>
        ///     Handle action. Number of arguments is defined by each handler implementation.
        /// </summary>
        /// <param name="args"></param>
        void Handle(params string[] args);
    }
}