using CommandLine;

namespace ModalHandler.Console.Args
{
    internal class UploadCmdArgs : CleanUpCmdArgs
    {
        [Option('o', "owner",
            HelpText =
                "Name of a browser that owns the modal dialog. Fully qualified title containing a browser type is expected.",
            Required = true)]
        public string Owner { get; set; }

        [Option('p', "path", HelpText = "Absolute path to a file to upload.", Required = true)]
        public string Path { get; set; }
    }
}