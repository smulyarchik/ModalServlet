using System;
using System.Linq.Expressions;
using ModalHandler.Handlers;
using ModalHandler.Interfaces;
using ModalHandler.Interfaces.Dialogs;
using NSubstitute;
using NUnit.Framework;

namespace ModalHandler.Test.Tests.Handler
{
    internal class BaseHandlerTests
    {
        private readonly IDialog _dialog = Substitute.For<IDialog>();
        private readonly ILogger _logger = Substitute.For<ILogger>();
        private BaseHandler<IDialog> _handler;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            // Parial mock.
            _handler = Substitute.ForPartsOf<BaseHandler<IDialog>>(_dialog, _logger, TimeSpan.Zero);
            _dialog.Name.Returns("Dummy Name");
            _dialog.Type.Returns("Dummy Type");
            // Make the logger invoke passing method.
            _logger.When(e => e.LogExec(Arg.Any<Expression<Func<bool>>>()))
                .Do(e => e.ArgAt<Expression<Func<bool>>>(0).Compile().Invoke());
        }

        [TearDown]
        public void Teardown()
        {
            _dialog.ClearReceivedCalls();
            _logger.ClearReceivedCalls();
            _handler.ClearReceivedCalls();
        }

        [Test]
        [Description("When dialog is not visible during handling, then log it and exit.")]
        public void Handle_NotVisible()
        {
            _dialog.IsVisible.Returns(false);

            _handler.Handle();

            _logger.Received(1).LogInfo(_handler.DialogIsNotVisibleErrMsg);
        }

        [Test]
        [Description("When dialog cannot be handled, then log it and exit.")]
        public void Handle_CannotHandle()
        {
            _dialog.InnerText.Returns("Test string");
            _dialog.IsVisible.Returns(true);
            // Need to make it clear we're mocking the property and not calling the base.
            // Partial mock would call the real property otherwise.
            _handler.When(e =>
                {
                    var canHandle = e.CanHandle;
                })
                .DoNotCallBase();
            _handler.CanHandle.Returns(false);

            _handler.Handle();

            _logger.Received(1).LogInfo(_handler.DialogCannotBeHandledErrMsg);
            _logger.Received(1).LogInfo(_handler.InnerTextMsg);
        }

        [Test]
        [Description("Handle - happy path. No info calls should occur.")]
        public void Handle_Success()
        {
            _dialog.IsVisible.Returns(true);
            _handler.CanHandle.Returns(true);

            _handler.Handle();

            _logger.DidNotReceive().LogInfo(Arg.Any<string>());
        }

        [Test]
        [Description("When disposing the handler, log the closing action and close the logger.")]
        public void Dispose_Logging()
        {
            _handler.Dispose();

            _logger.Received(1).LogExec(Arg.Any<Expression<Func<bool>>>());
            _logger.Received(1).Close();
        }

        [Test]
        [Description(
            "When disposing the handler and the dialog is not visible, do not attempt to close it - just exit.")]
        public void Dispose_NotVisible()
        {
            _dialog.IsVisible.Returns(false);

            _handler.Dispose();

            _dialog.DidNotReceive().Close();
            _logger.Received(1).Close();
        }

        [Test]
        [Description(
            "When disposing the handler and the dialog is visible, attempt to close it. The dialog stays closed - log the success.")]
        public void Dispose_CloseSuccess()
        {
            _dialog.IsVisible.Returns(true);
            _dialog.When(e => e.Close()).Do(e => _dialog.IsVisible.Returns(false));

            _handler.Dispose();

            _dialog.Received(1).Close();
            _logger.Received(1).LogInfo(_handler.DialogIsClosedMsg);
            _logger.Received(1).Close();
        }

        [Test]
        [Description(
            "When disposing the handler and the dialog is visible, attempt to close it. The dialog remains open - log the failure.")]
        public void Dispose_CloseFailure()
        {
            _dialog.IsVisible.Returns(true);

            _handler.Dispose();

            _dialog.Received(1).Close();
            _logger.Received(1).LogInfo(_handler.DialogFailedToCloseErrMsg);
            _logger.Received(1).Close();
        }
    }
}