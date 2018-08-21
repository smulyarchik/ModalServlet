using System;
using CommandLine;
using ModalHandler.Console.Args;
using ModalHandler.Handlers;
using ModalHandler.Interfaces;
using ModalHandler.Tools;

namespace ModalHandler.Console
{
    internal class Program
    {
        internal static int Main(string[] args)
        {
            string option = null;
            object subOpts = null;

            var supportedArgs = new CmdArgs();

            var parser = new Parser(settings => settings.HelpWriter = System.Console.Out);
            var logger = new Logger(System.Console.Out);
            try
            {
                bool isParsed;
                isParsed = parser.ParseArgumentsStrict(args, supportedArgs, (verb, opts) =>
                {
                    option = verb;
                    subOpts = opts;
                }, () => isParsed = false);

                if (!isParsed)
                    return (int) ExitCodes.ErrorBadArguments;

                bool isHandled;
                if (option.Equals(CmdArgs.UploadVerbName))
                    isHandled = Upload((UploadCmdArgs) subOpts, logger);
                else if (option.Equals(CmdArgs.AuthVerbName))
                    isHandled = Authenticate((AuthCmdArgs) subOpts, logger);
                else
                    isHandled = CleanUp((CleanUpCmdArgs) subOpts, logger);
                return (int) (isHandled ? ExitCodes.ErrorSuccess : ExitCodes.ErrorException);
            }
            catch (Exception e)
            {
                logger.LogInfo(e.ToString());
                Environment.Exit((int) ExitCodes.ErrorException);
                // ReSharper disable once HeuristicUnreachableCode
                return (int) ExitCodes.ErrorException;
            }
            finally
            {
                logger.Close();
            }
        }

        private static bool Authenticate(AuthCmdArgs args, ILogger logger)
        {
            using (var handler = new WindowsSecurityHandler(logger, TimeSpan.FromSeconds(args.Timeout)))
            {
                handler.Handle(args.Username, args.Password);
                return handler.IsHandled;
            }
        }


        private static bool CleanUp(CleanUpCmdArgs args, ILogger logger)
        {
            using (var handler = new CleanUpHandler(logger, TimeSpan.FromSeconds(args.Timeout)))
            {
                handler.Handle();
                return handler.IsHandled;
            }
        }

        private static bool Upload(UploadCmdArgs args, ILogger logger)
        {
            using (var handler = new FileUploadHandler(args.Owner, logger, TimeSpan.FromSeconds(args.Timeout)))
            {
                handler.Handle(args.Path);
                return handler.IsHandled;
            }
        }

        internal enum ExitCodes
        {
            ErrorSuccess = 0,
            ErrorException = 1,
            ErrorBadArguments = 160
        }
    }
}