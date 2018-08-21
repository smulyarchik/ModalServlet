namespace ModalHandler.Interfaces.Elements
{
    /// <summary>
    ///     Generic text field / edit representation.
    /// </summary>
    public interface ITextBox : IElement
    {
        /// <summary>
        /// Current value stored in the text box.
        /// </summary>
        string Value { get; }
        /// <summary>
        /// Send the specified string input to the text box.
        /// </summary>
        /// <param name="keys">Text to send.</param>
        void SendKeys(string keys);
    }
}