using System;
using ModalHandler.Interfaces.Dialogs;

namespace ModalHandler.Dialogs.WinApi
{
    /// <summary>
    ///     WinApi implementation of a top level modal dialog.
    /// </summary>
    internal class TopLevelModalDialog : ModalDialog, ITopLevelDialog
    {
        public TopLevelModalDialog(IntPtr handle) : base(handle)
        {
        }

        public ITopLevelDialog NextTopLevel => new TopLevelModalDialog(
            Tools.WinApi.GetTopLevelDialog(Timeout)){ Timeout = Timeout};
    }
}