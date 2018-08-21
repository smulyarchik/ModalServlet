using System;
using System.Linq.Expressions;
using ModalHandler.Handlers;
using ModalHandler.Interfaces;
using ModalHandler.Interfaces.Dialogs;
using NSubstitute;
using NUnit.Framework;

namespace ModalHandler.Test.Tests.Handler
{
    internal class CleanUpHandlerTests
    {
        private readonly ITopLevelDialog _dialog = Substitute.For<ITopLevelDialog>();
        private readonly ILogger _logger = Substitute.For<ILogger>();
        private CleanUpHandler _handler;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _handler = new CleanUpHandler(_dialog, _logger, TimeSpan.Zero);
            // Make the logger invoke passing method.
            _logger.When(e => e.LogExec(Arg.Any<Expression<Func<bool>>>()))
                .Do(e => e.ArgAt<Expression<Func<bool>>>(0).Compile().Invoke());
        }

        [TearDown]
        public void Teardown()
        {
            _logger.ClearReceivedCalls();
            _dialog.ClearReceivedCalls();
        }

        [Test]
        [Description("Clean up - happy path")]
        public void CleanUp()
        {
            var topLevel1 = Substitute.For<ITopLevelDialog>();
            var topLevel2 = Substitute.For<ITopLevelDialog>();
            var topLevel3 = Substitute.For<ITopLevelDialog>();
            // Original dialog(the desktop) calls for the first top level modal.
            _dialog.IsVisible.Returns(true);
            _dialog.NextTopLevel.Returns(topLevel1);
            // First modal is visible and closed successfully.
            topLevel1.IsVisible.Returns(true);
            topLevel1.When(e => e.Close()).Do(e => topLevel1.IsVisible.Returns(false));
            // It then calls for the next top level dialog.
            topLevel1.NextTopLevel.Returns(topLevel2);
            // Which is visible and closed as well.
            topLevel2.IsVisible.Returns(true);
            topLevel2.When(e => e.Close()).Do(e => topLevel2.IsVisible.Returns(false));
            // CleanUp keeps calling for open top level dialogs until there are none.
            topLevel2.NextTopLevel.Returns(topLevel3);
            // The third dialog is not visible. End of cycle.
            topLevel3.IsVisible.Returns(false);

            // Do the clean up.
            _handler.Handle();

            // Main dialog should return next top level.
            dynamic received = _dialog.Received(1).NextTopLevel;
            // That should successfully be closed.
            topLevel1.Received(1).Close();
            // Calls for the next top level.
            received = topLevel1.Received(1).NextTopLevel;
            // This one gets closed as well.
            topLevel2.Received(1).Close();
            // Then calls the third one.
            received = topLevel2.Received(1).NextTopLevel;
            // Which is not closed.
            topLevel3.DidNotReceive().Close();
            // And doesn't call for the next dialog.
            received = topLevel3.DidNotReceive().NextTopLevel;
            // LogExec should receive 3 calls: 1 for CleanUp and 2 for CloseModal.
            _logger.Received(3).LogExec(Arg.Any<Expression<Func<bool>>>());
            // There should be 2 logged messages for dialogs closure.
            _logger.Received(2).LogInfo(Arg.Any<string>());
        }

        [Test]
        [Description("If the clean up is triggered with arguments, ignore them and log it.")]
        public void CleanUpWithArgs()
        {
            const string arg = "some random string value";
            _dialog.IsVisible.Returns(true);

            _handler.Handle(arg);

            _logger.Received(1).LogInfo(CleanUpHandler.ArgumentsIgnoredInfoMsg);
        }

        [Test]
        [Description("When executed with no modal visible dialogs, no info messages should be displayed")]
        public void NoTopLevelDialogs()
        {
            var topLevelDialog = Substitute.For<ITopLevelDialog>();
            topLevelDialog.IsVisible.Returns(false);
            _dialog.IsVisible.Returns(true);
            _dialog.NextTopLevel.Returns(topLevelDialog);

            _handler.Handle();

            var received =_dialog.Received(1).NextTopLevel;
            topLevelDialog.DidNotReceive().Close();
            _logger.DidNotReceive().LogInfo(Arg.Any<string>());
        }
    }
}