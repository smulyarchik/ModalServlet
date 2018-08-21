using CommandLine;

namespace ModalHandler.Console.Args
{
    internal class AuthCmdArgs : CleanUpCmdArgs
    {
        [Option('u', "username", HelpText = "Username for authentication.", Required = true)]
        public string Username { get; set; }

        [Option('p', "password", HelpText = "Password for authentication.", Required = true)]
        public string Password { get; set; }
    }
}