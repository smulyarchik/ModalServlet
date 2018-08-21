using System;
using System.Windows.Automation;
using ModalHandler.Interfaces.Elements;

namespace ModalHandler.Elements.Uia
{
    /// <summary>
    ///     UIAutomation implementation of a generic text box.
    /// </summary>
    internal class UiaTextBox : UiaElement, ITextBox
    {
        private readonly ValuePattern _pattern;

        public UiaTextBox(AutomationElement element) : base(element)
        {
            _pattern = (ValuePattern) Element?.GetCurrentPattern(ValuePattern.Pattern);
        }


        public void SendKeys(string text)
        {
            _pattern.SetValue(text);
        }

        public string Value
        {
            get
            {
                try
                {
                    // Some text boxes support TextPattern, others - ValuePattern.
                    object pattern;
                    var textPatternIsSupported = Element.TryGetCurrentPattern(TextPattern.Pattern, out pattern);
                    return textPatternIsSupported
                        ? ((TextPattern) pattern).DocumentRange.GetText(256)
                        : _pattern.Current.Value;
                }
                catch (InvalidOperationException)
                {
                    return string.Empty;
                }
            }
        }
    }
}