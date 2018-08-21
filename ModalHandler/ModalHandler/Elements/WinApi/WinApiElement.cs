using System;
using System.Collections.Generic;
using System.Linq;
using ModalHandler.Interfaces.Elements;
using ModalHandler.Tools;

namespace ModalHandler.Elements.WinApi
{
    /// <summary>
    ///     WinApi implementation of a generic element.
    /// </summary>
    internal class WinApiElement : IElement
    {
        protected WinApiElement(IntPtr handle)
        {
            Handle = handle;
            Type = GetType().Name;
            Class = handle.GetClassName();
            Name = Handle.GetText();
        }

        protected IntPtr Handle { get; }

        public virtual bool IsVisible => Tools.WinApi.IsWindowVisible(Handle);

        public string Name { get; }

        public string Type { get; }

        public string Class { get; }
        public string InnerText => Handle.GetInnerText();

        public TElement FindElement<TElement>(string name, string className, TimeSpan timeout)
            where TElement : IElement => FindElements<TElement>(name, className, timeout).FirstOrDefault();

        public IEnumerable<TElement> FindElements<TElement>(string name, string className, TimeSpan timeout) where TElement : IElement
        {
            return Handle.GetElements(timeout, e =>
                {
                    var namePredicate = string.IsNullOrEmpty(name) || e.GetText().Equals(name);
                    var classNamePredicate = string.IsNullOrEmpty(className) || e.GetClassName().Equals(className);
                    return namePredicate && classNamePredicate;
                }).Select(e => (TElement) Activator.CreateInstance(typeof(TElement), e));
        }
    }
}