using System;
using System.IO;
using ModalHandler.Tools;
using NSubstitute;
using NUnit.Framework;

namespace ModalHandler.Test.Tests
{
    internal class LoggerTest
    {
        private static Exception _textError;
        private readonly TextWriter _writer = Substitute.For<TextWriter>();
        private Logger _logger;

        [OneTimeSetUp]
        public void ClassSetup()
        {
            _logger = new Logger(_writer);
        }

        [Test]
        [Description("When invoked, writes an info message to the underlying text writer.")]
        public void LogInfo()
        {
            const string testSt = "Test string";

            _logger.LogInfo(testSt);

            // Omit timestamp.
            _writer.Received(1).WriteLine(Arg.Is<string>(arg => arg.EndsWith(Logger.LoggerInfo + " " + testSt)));
        }

        [Test]
        [Description(
            "When invoked, logs execution beginning and end of the passed method. Returns method's return value.")]
        public void LogExec()
        {
            var arg1 = new Random().Next();
            const string str1 = "string1";
            const string str2 = "string2";
            string[] arg2 = {str1, str2};

            var result = _logger.LogExec(() => Dummy(arg1, arg2));

            Assert.That(result, Is.EqualTo(arg1));
            // Omit timestamps.
            _writer.Received(1)
                .WriteLine(Arg.Is<string>(arg => arg.EndsWith($"{Logger.LoggerExec} {nameof(LoggerTest)}.{nameof(Dummy)}: '{arg1}' '{str1} {str2}'")));
            _writer.Received(1).WriteLine(Arg.Is<string>(arg => arg.EndsWith($"{Logger.LoggerDone} {nameof(LoggerTest)}.{nameof(Dummy)}")));
        }


        [Test]
        [Description("Logs an unexpected exception without the info prefix.")]
        public void LogExecException()
        {
            const string errorMsg = "Test Message";

            _logger.LogExec(() => DummyError(errorMsg));

            _writer.Received(1).WriteLine(_textError);
        }

        [Test]
        [Description("When invoked, closes the underlying writer.")]
        public void Close()
        {
            _logger.Close();

            _writer.Received(1).Close();
        }

        [Test]
        [Description("When called, transforms the underlying writer's data into a string and returns it.")]
        public void Out()
        {
            var output = _logger.Out;

            _writer.Received(1).ToString();
        }

        private static int Dummy(int arg1, string[] arg2)
        {
            return arg1;
        }

        private static object DummyError(string message)
        {
            _textError = new Exception(message);
            throw _textError;
        }
    }
}