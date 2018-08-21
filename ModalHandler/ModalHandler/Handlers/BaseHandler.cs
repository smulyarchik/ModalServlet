using System;
using ModalHandler.Interfaces;
using ModalHandler.Interfaces.Dialogs;
using ModalHandler.Tools;

namespace ModalHandler.Handlers
{
    /// <summary>
    ///     Generic implementation of a base handler's core logic.
    /// </summary>
    /// <typeparam name="TDialog">Dialog to be handled.</typeparam>
    public abstract class BaseHandler<TDialog> : IHandler where TDialog : class, IDialog
    {
        private string DialogNameClass => $" (name: '{Dialog.Name}', class: '{Dialog.Class}')";

        internal string DialogIsNotVisibleErrMsg => $"Failed to detect {Dialog.Type}.";

        internal string DialogCannotBeHandledErrMsg => $"Cannot handle {Dialog.Type}." + DialogNameClass;

        internal string DialogIsClosedMsg => $"Closed {Dialog.Type}." + DialogNameClass;

        internal string DialogFailedToCloseErrMsg => $"Failed to close {Dialog.Type}." + DialogNameClass;

        internal string InnerTextMsg => $"Inner text: {Dialog.InnerText}";

        protected BaseHandler(TDialog dialog, ILogger logger, TimeSpan timeout)
        {
            Dialog = dialog;
            Logger = logger;
            Timeout = timeout;
        }

        protected TDialog Dialog { get; }

        protected TimeSpan Timeout { get; }

        protected ILogger Logger { get; }

        public void Handle(params string[] args)
        {
            // Use 20% of the dialog timeout for internal element waits.
            Dialog.Timeout = TimeSpan.FromTicks(Timeout.Ticks / 5);
            if (!Dialog.IsVisible)
            {
                Logger.LogInfo(DialogIsNotVisibleErrMsg);
                return;
            }
            if (!CanHandle)
            {
                Logger.LogInfo(DialogCannotBeHandledErrMsg);
                Logger.LogInfo(InnerTextMsg);
                return;
            }
            IsHandled = DoHandle(args);
        }

        public virtual bool CanHandle => Dialog.IsVisible;

        public bool IsHandled { get; private set; }

        public virtual void Dispose()
        {
            Logger.LogExec(() => CloseModal());
            Logger.Close();
        }

        protected abstract bool DoHandle(params string[] args);

        protected bool CloseModal()
        {
            if (!Dialog.IsVisible)
                return false;
            Dialog.Close();
            Util.Wait(() => !Dialog.IsVisible, Timeout);
            Logger.LogInfo(Dialog.IsVisible ? DialogFailedToCloseErrMsg : DialogIsClosedMsg);
            return false;
        }
    }
}