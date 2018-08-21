using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;
using ModalHandler.Interfaces.Elements;
using ModalHandler.Tools;

namespace ModalHandler.Elements.Uia
{
    /// <summary>
    ///     UIAutomation implementation of a generic element.
    /// </summary>
    internal abstract class UiaElement : IElement
    {
        protected UiaElement(AutomationElement element)
        {
            Element = element;
            Type = GetType().Name;
            Name = element?.Current.Name;
            Class = element?.Current.LocalizedControlType;
        }

        protected AutomationElement Element { get; }

        public virtual bool IsVisible
        {
            get
            {
                try
                {
                    return !Element.Current.IsOffscreen && Element.Current.IsEnabled;
                }
                catch (Exception)
                {
                    // Should handle multiple cases, such as when the element is not available or null, etc.
                    return false;
                }
            }
        }

        public string Name { get; }

        public string Type { get; }

        public string Class { get; }

        public string InnerText => Element.GetInnerText();

        public TElement FindElement<TElement>(string name, string className, TimeSpan timeout)
            where TElement : IElement => FindElements<TElement>(name, className, timeout).FirstOrDefault();

        public IEnumerable<TElement> FindElements<TElement>(string name, string className, TimeSpan timeout) where TElement : IElement
        {
            return Element.FindDescendants(name, className, timeout)
                .Select(e => (TElement) Activator.CreateInstance(typeof(TElement), e));
        }
    }
}