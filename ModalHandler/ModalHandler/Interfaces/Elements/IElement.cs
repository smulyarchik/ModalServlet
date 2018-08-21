using System;
using System.Collections.Generic;

namespace ModalHandler.Interfaces.Elements
{
    /// <summary>
    ///     Generic element container representation.
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Check if the element is displayed on the screen.
        /// </summary>
        bool IsVisible { get; }
        /// <summary>
        /// Element's name.(title, caption etc.)
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Element's type;
        /// </summary>
        string Type { get; }

        /// <summary>
        /// Element's class name.
        /// </summary>
        string Class { get; }

        /// <summary>
        /// Text of the element and all its descendants.
        /// </summary>
        string InnerText { get; }

        /// <summary>
        /// Find a child element by name / class combination.
        /// </summary>
        /// <typeparam name="TElement">Element implementation type.</typeparam>
        /// <param name="name">Name of the element.</param>
        /// <param name="className">Class name of the element.</param>
        /// <param name="timeout">Maximum amount of time for element search.</param>
        /// <returns>Found element instance.</returns>
        TElement FindElement<TElement>(string name, string className, TimeSpan timeout) where TElement : IElement;

        /// <summary>
        /// Find all child elements by name / class combination.
        /// </summary>
        /// <typeparam name="TElement">Element implementation type.</typeparam>
        /// <param name="name">Name of the element. If null then any element name is considered.</param>
        /// <param name="className">Class name of the element. If null then any element class name is considered.</param>
        /// <param name="timeout">Maximum amount of time for element search.</param>
        /// <returns>Collection of found elements cast to the specified type.</returns>
        IEnumerable<TElement> FindElements<TElement>(string name, string className, TimeSpan timeout) where TElement : IElement;
    }
}