using System;
using System.IO;
using ModalHandler.Dialogs.WinApi;
using ModalHandler.Interfaces;
using ModalHandler.Interfaces.Dialogs;
using ModalHandler.Tools;

namespace ModalHandler.Handlers
{
    /// <summary>
    ///     WinApi implementation of handler responsible for file upload.
    /// </summary>
    public class FileUploadHandler : BaseHandler<IFileUploadDialog>
    {
        internal const string EmptyFileNameErrMsg = "File Name cannot be empty or null";

        public const string OwnerCannotBeFoundErrMsg = "Owner window cannot be found";

        internal const string FileNameParamExpectedErrMsg = "File name parameter is expected.";

        internal static string FileCannotBeFoundErrMsg(string fileName) =>  $"File cannot be found: '{fileName}'";

        public FileUploadHandler(IFileUploadDialog dialog, ILogger logger, TimeSpan timeout) : base(dialog, logger,
            timeout)
        {
        }

        public FileUploadHandler(string ownerTitle, ILogger logger, TimeSpan timeout) : this(
            new FileUploadDialog(new Func<IntPtr>(() =>
            {
                // Account for postfixes in browser name.
                var parentHandle = WinApi.GetDesktopWindow().GetElement(timeout, e => e.GetText().StartsWith(ownerTitle));
                if (parentHandle == IntPtr.Zero)
                {
                    logger.LogInfo(OwnerCannotBeFoundErrMsg);
                    return parentHandle;
                }
                return WinApi.GetEnabledPopup(ownerTitle, timeout);
            }).Invoke()),
            logger,
            timeout)
        {
        }

        private bool Upload(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                Logger.LogInfo(EmptyFileNameErrMsg);
                return false;
            }
            if (!File.Exists(fileName))
            {
                Logger.LogInfo(FileCannotBeFoundErrMsg(fileName));
                return false;
            }
            return Logger.LogExec(() => Dialog.SetFileName(fileName))
                   && Logger.LogExec(() => Dialog.Open());
        }

        protected override bool DoHandle(params string[] args)
        {
            if (args.Length < 1)
            {
                Logger.LogInfo(FileNameParamExpectedErrMsg);
                return false;
            }
            var fileName = args[0];
            return Logger.LogExec(() => Upload(fileName));
        }
    }
}