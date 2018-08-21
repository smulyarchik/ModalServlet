using System;
using System.Linq.Expressions;
using ModalHandler.Handlers;
using ModalHandler.Interfaces;
using ModalHandler.Interfaces.Dialogs;
using NSubstitute;
using NUnit.Framework;

namespace ModalHandler.Test.Tests.Handler
{
    internal class WindowsSecurityHandlerTest
    {
        private readonly ISecurityDialog _dialog = Substitute.For<ISecurityDialog>();
        private readonly ILogger _logger = Substitute.For<ILogger>();
        private WindowsSecurityHandler _handler;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _handler = new WindowsSecurityHandler(_dialog, _logger, TimeSpan.Zero);
            // Make the logger invoke passing method and return its result.
            _logger.LogExec(Arg.Any<Expression<Func<bool>>>())
                .Returns(e => e.ArgAt<Expression<Func<bool>>>(0).Compile().Invoke());
            _dialog.IsVisible.Returns(true);
            _dialog.CanAcceptCredentials.Returns(true);
        }

        [TearDown]
        public void Teardown()
        {
            _logger.ClearReceivedCalls();
        }

        [Test]
        [Description("Authenticate - happy path.")]
        public void Authenticate()
        {
            const string username = "user";
            const string password = "pass";
            _dialog.EnterCredentials(Arg.Is(username), Arg.Is(password)).Returns(true);
            _dialog.Submit().Returns(true);

            _handler.Handle(username, password);

            Assert.That(_handler.IsHandled);
            // Calls: Authenticate, Dialog.EnterCredentials, Dialog.Submit.
            _logger.Received(3).LogExec(Arg.Any<Expression<Func<bool>>>());
            _dialog.Received(1).EnterCredentials(username, password);
            _dialog.Received(1).Submit();
            // No info messages.
            _logger.DidNotReceive().LogInfo(Arg.Any<string>());
        }

        [Test]
        [Description("when Incorrect number of parameters is passed, log it and return a failure.")]
        public void ParamsMismatch()
        {
            _handler.Handle("onlyOneParam");

            Assert.That(_handler.IsHandled, Is.False);
            _logger.Received(1).LogInfo(WindowsSecurityHandler.UserPassExpectedErrMsg);
            _logger.DidNotReceive().LogExec(Arg.Any<Expression<Func<bool>>>());
        }
    }
}