using System;
using System.Windows.Automation;
using ModalHandler.Elements.Uia;
using ModalHandler.Interfaces.Dialogs;

namespace ModalHandler.Dialogs.Uia
{
    internal class ModalDialog : UiaElement, IDialog
    {
        public ModalDialog(AutomationElement element) : base(element)
        {
        }

        public void Close()
        {
            (Element.GetCurrentPattern(WindowPattern.Pattern) as WindowPattern)?.Close();
        }

        public TimeSpan Timeout { get; set; }
    }
}
