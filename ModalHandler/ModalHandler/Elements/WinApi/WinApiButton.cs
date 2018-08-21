using System;
using ModalHandler.Interfaces.Elements;

namespace ModalHandler.Elements.WinApi
{
    /// <summary>
    ///     WinApi implementation of a button.
    /// </summary>
    internal class WinApiButton : WinApiElement, IButton
    {
        public WinApiButton(IntPtr handle) : base(handle)
        {
        }

        public void Click()
        {
            const int BM_CLICK = 0x00F5;
            Tools.WinApi.SendMessage(Handle, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        }
    }
}