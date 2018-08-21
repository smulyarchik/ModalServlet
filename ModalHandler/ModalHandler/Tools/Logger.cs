using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ModalHandler.Interfaces;

namespace ModalHandler.Tools
{
    public class Logger : ILogger
    {
        public const string LoggerExec = "[EXECUTING]:";
        public const string LoggerInfo = "[INFO]:";
        public const string LoggerDone = "[DONE]:";
        private readonly TextWriter _writer;

        public Logger(TextWriter writer)
        {
            _writer = writer;
        }

        public void LogInfo(string message)
        {
            _writer.WriteLine($"{Timestamp} {LoggerInfo} {message}");
        }

        public T LogExec<T>(Expression<Func<T>> method)
        {
            var expBody = (MethodCallExpression) method.Body;
            var methodInfo = expBody.Method;
            var methodName = methodInfo.Name;
            var caller = methodInfo.DeclaringType?.Name;
            try
            {
                string argsLine = null;
                // Aggregate arguments into a string.
                if (expBody.Arguments.Count > 0)
                    argsLine = ": " + expBody.Arguments.Aggregate<Expression, string>(null,
                                       (cur, a) =>
                                       {
                                           var arg = Expression.Lambda(a).Compile().DynamicInvoke();
                                           // If an argument is a collection, aggregate all its members.
                                           if (arg is IEnumerable<string>)
                                               arg = ((IEnumerable<string>) arg).Aggregate();
                                           return $"{cur} '{arg}'";
                                       })
                                   .Trim();
                _writer.WriteLine($"{Timestamp} {LoggerExec} {caller}.{methodName}{argsLine}");
                return method.Compile().Invoke();
            }
            catch (Exception e)
            {
                _writer.WriteLine(e);
                return default(T);
            }
            finally
            {
                _writer.WriteLine($"{Timestamp} {LoggerDone} {caller}.{methodName}");
            }
        }

        public void Close()
        {
            _writer.Close();
        }

        public string Out => _writer.ToString().Trim();

        private static string Timestamp => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss,fff");
    }
}