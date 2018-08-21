using System;
using System.Text;
using ModalHandler.Interfaces.Elements;
using ModalHandler.Tools;

namespace ModalHandler.Elements.WinApi
{
    /// <summary>
    ///     WinApi implementation of a generic text box.
    /// </summary>
    internal class WinApiTextBox : WinApiElement, ITextBox
    {
        public WinApiTextBox(IntPtr handle) : base(handle)
        {
        }

        public void SendKeys(string keys)
        {
            var sb = new StringBuilder(keys);
            const int WM_SETTEXT = 0x000C;
            Tools.WinApi.SendMessage(Handle, WM_SETTEXT, (IntPtr) sb.Length, sb.ToString());
        }

        public string Value => Handle.GetText();
    }
}