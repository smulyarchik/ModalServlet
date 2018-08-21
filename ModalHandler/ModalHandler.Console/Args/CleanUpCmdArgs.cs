using CommandLine;

namespace ModalHandler.Console.Args
{
    internal class CleanUpCmdArgs
    {
        [Option('t', "timeout", DefaultValue = 10,
            HelpText = "Maximum amount of time in seconds allotted for internal waits. (e.g. modal window detection)",
            Required = false)]
        public int Timeout { get; set; }
    }
}