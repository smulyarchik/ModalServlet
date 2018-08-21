using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace ModalHandler.Tools
{
    /// <summary>
    /// Helper class for working with UIAutomation elements.
    /// </summary>
    internal static class UiAutomation
    {
        /// <summary>
        /// Search for an element throughout the subtree.
        /// </summary>
        /// <param name="root">Parent element.</param>
        /// <param name="name">Searched element's name.</param>
        /// <param name="classType">Searched element's class name or type.</param>
        /// <param name="timeout">Maximum amount of time for element search.</param>
        /// <returns>Found element's instance or null if no match occurred.</returns>
        public static AutomationElement FindDescendant(this AutomationElement root, string name, object classType, TimeSpan timeout)
        {
            return root.FindElement(TreeScope.Descendants, name, classType, timeout);
        }

        /// <summary>
        /// Fetch all descendant of the provided parent element that match name/class constraint.
        /// </summary>
        /// <param name="root">Parent element.</param>
        /// <param name="name">Searched elements' name. If null, any name is considered.</param>
        /// <param name="classType">Searched elements' class name or type. If null, any class is considered.</param>
        /// <param name="timeout">Maximum amount of time for element search.</param>
        /// <returns>Found elements or an empty collection if no match occurred.</returns>
        public static IEnumerable<AutomationElement> FindDescendants(this AutomationElement root, string name,
            object classType, TimeSpan timeout) => root.FindElements(TreeScope.Descendants, name, classType, timeout);

        /// <summary>
        /// Search for a child element only. (one nesting level)
        /// </summary>
        /// <param name="root">Parent element.</param>
        /// <param name="name">Searched element's name.</param>
        /// <param name="classType">Searched element's class name or type.</param>
        /// <param name="timeout">Maximum amount of time for element search.</param>
        /// <returns>Found element's instance or null if no match occurred.</returns>
        public static AutomationElement FindChild(this AutomationElement root, string name, object classType, TimeSpan timeout)
        {
            return root.FindElement(TreeScope.Children, name, classType, timeout);
        }

        private static AutomationElement FindElement(this AutomationElement root, TreeScope scope, string name,
            object classType, TimeSpan timeout) => root.FindElements(scope, name, classType, timeout).FirstOrDefault();

        private static IEnumerable<AutomationElement> FindElements(this AutomationElement root, TreeScope scope, string name, object classType, TimeSpan timeout)
        {
            var condition1 = string.IsNullOrEmpty(name)
                ? Condition.TrueCondition
                : new PropertyCondition(AutomationElement.NameProperty, name);
            var condition2 = classType == null
                ? Condition.TrueCondition
                : classType is ControlType
                    ? new PropertyCondition(AutomationElement.ControlTypeProperty, (ControlType) classType)
                    : new PropertyCondition(AutomationElement.ClassNameProperty, classType);
            return root.FindElements(scope, new AndCondition(condition1, condition2), timeout);
        }

        private static IEnumerable<AutomationElement> FindElements(this AutomationElement root, TreeScope scope, Condition andCondition, TimeSpan timeout)
        {
            var elements = default(IEnumerable<AutomationElement>);
            Util.Wait(() => (elements = root.FindAll(scope, andCondition).Cast<AutomationElement>()).Any(), timeout);
            return elements;
        }

        public static string GetInnerText(this AutomationElement node) => node
            .FindDescendants(null, null, TimeSpan.Zero)
            .Select(e => e.Current.Name)
            .Where(e => !string.IsNullOrEmpty(e)).Aggregate('\r', '\n');
    }
}