using System;
using System.Linq.Expressions;

namespace ModalHandler.Interfaces
{
    /// <summary>
    ///     Generic logger representation. Key feature is to log a wrapped method's execution beginning / end.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        ///     Logging output.
        /// </summary>
        string Out { get; }

        /// <summary>
        ///     Log a diagnostic message to provide more details during execution.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void LogInfo(string message);

        /// <summary>
        ///     Log a message before and after the passed method's execution.
        /// </summary>
        /// <typeparam name="T">Wrapped method's initial return value.</typeparam>
        /// <param name="method">Method wrapped in a function.</param>
        /// <returns></returns>
        T LogExec<T>(Expression<Func<T>> method);

        /// <summary>
        ///     Close the logger disposing of the underlying writer.
        /// </summary>
        void Close();
    }
}