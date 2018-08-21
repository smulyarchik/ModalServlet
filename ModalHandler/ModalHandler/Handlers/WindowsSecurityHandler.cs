using System;
using System.Windows.Automation;
using ModalHandler.Interfaces;
using ModalHandler.Interfaces.Dialogs;
using ModalHandler.Tools;

namespace ModalHandler.Handlers
{
    /// <summary>
    ///     UIAutomation implementation of a handler responsible for dealing with IE11 'Windows Security' dialog.
    /// </summary>
    public class WindowsSecurityHandler : BaseHandler<ISecurityDialog>
    {
        public const string UserPassExpectedErrMsg = "'Username' and 'Password' are expected.";

        public WindowsSecurityHandler(ISecurityDialog dialog, ILogger logger, TimeSpan timeout) : base(dialog, logger,
            timeout)
        {
        }

        public WindowsSecurityHandler(ILogger logger, TimeSpan timeout) : this(
            FindDialog(timeout), logger,
            timeout)
        {
        }

        private static ISecurityDialog FindDialog(TimeSpan timeout)
        {
            // Depending on IE11 version, this window may appear either top level or nested.
            // Iterating through the whole desktop tree with UIA might potentially affect performance by a great amount.
            // Use WinApi to traverse the tree instead as it's much more performant.
            var dialogPtr = WinApi.GetDesktopWindow().GetElement(timeout, e => e.GetText().Equals("Windows Security"));
            if (dialogPtr == IntPtr.Zero ||  dialogPtr.GetClassName().Equals(WinApi.ModalDialogClassName))
                return new Dialogs.WinApi.SecurityDialog(dialogPtr);
            return new Dialogs.Uia.SecurityDialog(AutomationElement.FromHandle(dialogPtr));
        }

        public override bool CanHandle => Dialog.CanAcceptCredentials;

        protected override bool DoHandle(params string[] args)
        {
            if (args.Length < 2)
            {
                Logger.LogInfo(UserPassExpectedErrMsg);
                return false;
            }
            var username = args[0];
            var password = args[1];
            return Logger.LogExec(() => Authenticate(username, password));
        }

        private bool Authenticate(string username, string password)
        {
            return Logger.LogExec(() => Dialog.EnterCredentials(username, password)) && Logger.LogExec(() => Dialog.Submit());
        }
    }
}