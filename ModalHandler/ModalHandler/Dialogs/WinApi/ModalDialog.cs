using System;
using ModalHandler.Elements.WinApi;
using ModalHandler.Interfaces.Dialogs;

namespace ModalHandler.Dialogs.WinApi
{
    /// <summary>
    ///     WinApi implementaion of a standard modal dialog.
    /// </summary>
    internal class ModalDialog : WinApiElement, IDialog
    {
        public ModalDialog(IntPtr handle) : base(handle)
        {
        }

        public void Close()
        {
            const int WM_CLOSE = 0x0010;
            Tools.WinApi.SendMessage(Handle, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
        }

        public TimeSpan Timeout { get; set; }
    }
}