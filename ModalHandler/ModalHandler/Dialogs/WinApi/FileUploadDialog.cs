using System;
using ModalHandler.Elements.WinApi;
using ModalHandler.Interfaces.Dialogs;
using ModalHandler.Interfaces.Elements;

namespace ModalHandler.Dialogs.WinApi
{
    /// <summary>
    ///     WinApi file upload dialog implementation.
    /// </summary>
    internal class FileUploadDialog : ModalDialog, IFileUploadDialog
    {
        public FileUploadDialog(IntPtr handle) : base(handle)
        {
        }

        private IButton OpenButton => FindElement<WinApiButton>("&Open", "Button", Timeout);
        private ITextBox FileNameTextBox => FindElement<WinApiTextBox>(string.Empty, "Edit", Timeout);

        public bool SetFileName(string fileName)
        {
            FileNameTextBox.SendKeys(fileName);
            return FileNameTextBox.Value.Equals(fileName);
        }

        public bool Open()
        {
            OpenButton.Click();
            return !IsVisible;
        }

        public override bool IsVisible => base.IsVisible && OpenButton.IsVisible && FileNameTextBox.IsVisible;
    }
}