using System;
using System.IO;
using System.Linq.Expressions;
using ModalHandler.Handlers;
using ModalHandler.Interfaces;
using ModalHandler.Interfaces.Dialogs;
using ModalHandler.Test.Properties;
using NSubstitute;
using NUnit.Framework;

namespace ModalHandler.Test.Tests.Handler
{
    internal class FileUploadHandlerTests
    {
        private readonly IFileUploadDialog _dialog = Substitute.For<IFileUploadDialog>();
        private readonly ILogger _logger = Substitute.For<ILogger>();
        private FileUploadHandler _handler;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _handler = new FileUploadHandler(_dialog, _logger, TimeSpan.Zero);
            // Dialog is always open and can be handled.
            _dialog.IsVisible.Returns(true);
            // Make the logger invoke passing method.
            _logger.When(e => e.LogExec(Arg.Any<Expression<Func<bool>>>()))
                .Do(e => e.ArgAt<Expression<Func<bool>>>(0).Compile().Invoke());
        }

        [TearDown]
        public void Teardown()
        {
            // Clear calls after each test.
            _logger.ClearReceivedCalls();
        }

        [Test]
        [Description("When an empty file name is passed, log and return a failure.")]
        public void EmptyFileName()
        {
            _handler.Handle(string.Empty);

            // Call to Upload.
            _logger.Received(1).LogExec(Arg.Any<Expression<Func<bool>>>());
            // Error logging.
            _logger.Received(1).LogInfo(FileUploadHandler.EmptyFileNameErrMsg);
            Assert.That(_handler.IsHandled, Is.False);
        }

        [Test]
        [Description("When a non-existing name is passed, log and return a failure.")]
        public void NonExistingFileName()
        {
            const string fileName = "Some.file";
            _handler.Handle(fileName);

            // Call to Upload.
            _logger.Received(1).LogExec(Arg.Any<Expression<Func<bool>>>());
            // Error logging.
            _logger.Received(1).LogInfo(FileUploadHandler.FileCannotBeFoundErrMsg(fileName));
            Assert.That(_handler.IsHandled, Is.False);
        }

        [Test]
        [Description("When no parameters are passed, log and return a failure")]
        public void NoParams()
        {
            _handler.Handle();

            // Error logging.
            _logger.Received(1).LogInfo(FileUploadHandler.FileNameParamExpectedErrMsg);
            Assert.That(_handler.IsHandled, Is.False);
        }

        [Test]
        [Description("Upload happy path. No info calls should occur.")]
        public void Upload()
        {
            var fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Resources.TestFile);
            _logger.LogExec(Arg.Any<Expression<Func<bool>>>()).Returns(true);

            _handler.Handle(fileName);

            // Should receive 3 execution calls in total.
            _logger.Received(3).LogExec(Arg.Any<Expression<Func<bool>>>());
            // Including calls to Dialog.SetFileName and Dialog.Open methods.
            _dialog.Received(1).SetFileName(fileName);
            _dialog.Received(1).Open();
            Assert.That(_handler.IsHandled);
            _logger.DidNotReceive().LogInfo(Arg.Any<string>());
        }

        [Test]
        [Description("When failed to find the owner window, log the error and return.")]
        public void OwnerNotFound()
        {
            var handler = new FileUploadHandler("Non-existent window title", _logger, TimeSpan.Zero);

            Assert.That(handler.CanHandle, Is.False);
            _logger.Received(1).LogInfo(FileUploadHandler.OwnerCannotBeFoundErrMsg);
            // Means there were no calls to the dialog.
            _logger.DidNotReceive().LogInfo(handler.DialogIsNotVisibleErrMsg);
        }
    }
}