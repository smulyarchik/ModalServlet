using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Automation;
using ModalHandler.Elements.Uia;
using ModalHandler.Handlers;
using ModalHandler.Interfaces;
using ModalHandler.Test.Properties;
using ModalHandler.Tools;
using NUnit.Framework;

namespace ModalHandler.Test.Tests
{
    internal class UiTests
    {
        private const string AppMainWindowName = "Main Window";
        private const string AppStatusBoxName = "Status Box";
        private readonly ILogger _logger = new Logger(Console.Out);
        private readonly TimeSpan _timeout = TimeSpan.FromSeconds(1);

        [TestCase("Xaml")]
        [TestCase("Win32")]
        [Description("Handle IE11 'Windows Security' dialog by simulating it in the test WPF app.")]
        public void WindowsSecurity(string type)
        {
            HandlerTest($"Windows Security({type})", () => new WindowsSecurityHandler(_logger, _timeout), "user", "pass");
        }

        [Test]
        [Description("Handle Windows file upload dialog by simulating it in the test WPF app.")]
        public void FileUpload()
        {
            // Remove a part of the name to check partial matching.
            var ownerName = AppMainWindowName.Remove(AppMainWindowName.Length - 3);
            HandlerTest("File Upload", () => new FileUploadHandler(ownerName, _logger, _timeout),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.TestFile));
        }

        [Test]
        [Description("Handle modal dialogs cleanup by simulating it in the test WPF app.")]
        public void CleanUp()
        {
            HandlerTest("File Upload", () => new CleanUpHandler(_logger, _timeout));
        }

        private void HandlerTest(string testWindowBtnName, Func<IHandler> getHandler, params string[] args)
        {
            var appPath = Path.Combine(TestContext.CurrentContext.TestDirectory, typeof(App.App).Module.Name);

            var proc = Process.Start(appPath);
            try
            {
                proc?.WaitForInputIdle();

                var mainWindow =
                    AutomationElement.RootElement.FindChild(AppMainWindowName, ControlType.Window, _timeout);
                var btn = new UiaButton(mainWindow.FindDescendant(testWindowBtnName, ControlType.Button, _timeout));
                btn.Click();

                using (var handler = getHandler())
                {
                    handler.Handle(args);

                    Assert.That(handler.IsHandled);
                    var tBox = new UiaTextBox(mainWindow.FindDescendant(AppStatusBoxName, ControlType.Edit, _timeout));
                    Assert.That(tBox.Value, Is.EqualTo(args.Aggregate()));
                }
            }
            finally
            {
                proc?.Kill();
            }
        }
    }
}