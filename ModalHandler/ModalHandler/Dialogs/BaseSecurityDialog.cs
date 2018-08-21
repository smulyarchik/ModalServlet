using System;
using System.Collections.Generic;
using ModalHandler.Interfaces.Dialogs;
using ModalHandler.Interfaces.Elements;
using ModalHandler.Tools;

namespace ModalHandler.Dialogs
{
    internal abstract class BaseSecurityDialog<TDialog> : ISecurityDialog where TDialog : IDialog
    {
        private readonly IDialog _dialog;

        protected abstract IButton OkButton { get; }
        protected abstract ITextBox UsernameBox { get; }
        protected abstract ITextBox PasswordBox { get; }

        protected BaseSecurityDialog(TDialog dialog)
        {
            _dialog = dialog;
        }

        public bool Submit()
        {
            OkButton.Click();
            var isClosed = false;
            Util.Wait(() => isClosed = !IsVisible, Timeout);
            return isClosed;
        }

        public bool CanAcceptCredentials => UsernameBox.IsVisible && PasswordBox.IsVisible && OkButton.IsVisible;

        public bool EnterCredentials(string username, string password)
        {
            var usernameBox = UsernameBox;
            usernameBox.SendKeys(username);
            PasswordBox.SendKeys(password);
            // Cannot compare password as it is masked.
            return usernameBox.Value.Equals(username);
        }

        public void Close()
        {
            _dialog.Close();
        }

        public bool IsVisible => _dialog.IsVisible;

        public string Name => _dialog.Name;

        public string Type => _dialog.Type;

        public string Class => _dialog.Class;
        public string InnerText => _dialog.InnerText;

        public TElement FindElement<TElement>(string name, string className, TimeSpan timeout) where TElement : IElement
        {
            return _dialog.FindElement<TElement>(name, className, timeout);
        }

        public IEnumerable<TElement> FindElements<TElement>(string name, string className, TimeSpan timeout) where TElement : IElement
        {
            return _dialog.FindElements<TElement>(name, className, timeout);
        }

        public TimeSpan Timeout
        {
            get { return _dialog.Timeout; }
            set { _dialog.Timeout = value; }
        }
    }
}
