using System.Windows.Automation;
using ModalHandler.Elements.Uia;
using ModalHandler.Interfaces.Elements;

namespace ModalHandler.Dialogs.Uia
{
    /// <summary>
    ///     UIAutomation implementation of 'Windows Security' dialog.
    /// </summary>
    internal class SecurityDialog : BaseSecurityDialog<ModalDialog>
    {
        public SecurityDialog(AutomationElement element) : base(new ModalDialog(element))
        {
        }

        protected override IButton OkButton => FindElement<UiaButton>("OK", "Button", Timeout);
        protected override ITextBox UsernameBox => FindElement<UiaTextBox>("User name", "TextBox", Timeout);
        protected override ITextBox PasswordBox => FindElement<UiaTextBox>("Password", "PasswordBox", Timeout);
    }
}