using System;
using System.Linq;
using ModalHandler.Dialogs.WinApi;
using ModalHandler.Interfaces;
using ModalHandler.Interfaces.Dialogs;
using ModalHandler.Tools;

namespace ModalHandler.Handlers
{
    /// <summary>
    ///     WinApi implementation of a handler responsible for top level modals clean up.
    /// </summary>
    public class CleanUpHandler : BaseHandler<ITopLevelDialog>
    {
        internal const string ArgumentsIgnoredInfoMsg = "No arguments are required for clean up. Ignored.";

        public CleanUpHandler(ITopLevelDialog dialog, ILogger logger, TimeSpan timeout) : base(dialog, logger, timeout)
        {
        }

        public CleanUpHandler(ILogger logger, TimeSpan timeout) : this(
            // Start with the desktop as it is always visible.
            new TopLevelModalDialog(WinApi.GetDesktopWindow()), logger, timeout)
        {
        }

        protected override bool DoHandle(params string[] args)
        {
            if (args.Any())
                Logger.LogInfo(ArgumentsIgnoredInfoMsg);
            return Logger.LogExec(() => CleanUp());
        }

        private bool CleanUp()
        {
            var handler = this;
            while (true)
            {
                handler = new CleanUpHandler(handler.Dialog.NextTopLevel, handler.Logger, handler.Timeout);
                if (!handler.CanHandle)
                    break;
                Logger.LogExec(() => handler.CloseModal());
            }
            return true;
        }

        public override void Dispose()
        {
            Logger.Close();
        }
    }
}