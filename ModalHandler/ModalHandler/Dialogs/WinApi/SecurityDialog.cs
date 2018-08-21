using System;
using System.Linq;
using ModalHandler.Elements.WinApi;
using ModalHandler.Interfaces.Elements;

namespace ModalHandler.Dialogs.WinApi
{
    /// <summary>
    ///     WinApi implementation of 'Windows Security' dialog.
    /// </summary
    internal class SecurityDialog : BaseSecurityDialog<ModalDialog>
    {
        public SecurityDialog(IntPtr handle) : base(new ModalDialog(handle))
        {
        }

        protected override IButton OkButton => FindElement<WinApiButton>("OK", "Button", Timeout);

        protected override ITextBox UsernameBox => FindElements<WinApiTextBox>(null, "Edit", Timeout)
            .First(e => string.IsNullOrEmpty(e.Value));
        protected override ITextBox PasswordBox => FindElements<WinApiTextBox>(null, "Edit", Timeout).Last();
    }
}
