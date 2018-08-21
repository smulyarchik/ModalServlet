using System.Windows.Automation;
using ModalHandler.Interfaces.Elements;

namespace ModalHandler.Elements.Uia
{
    /// <summary>
    ///     UIAutomation implementation of a generic button.
    /// </summary>
    internal class UiaButton : UiaElement, IButton
    {
        public UiaButton(AutomationElement element) : base(element)
        {
        }

        public void Click()
        {
            ((InvokePattern) Element.GetCurrentPattern(InvokePattern.Pattern)).Invoke();
        }
    }
}