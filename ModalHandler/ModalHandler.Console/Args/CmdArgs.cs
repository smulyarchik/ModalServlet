using CommandLine;
using CommandLine.Text;

namespace ModalHandler.Console.Args
{
    internal class CmdArgs
    {
        public const string UploadVerbName = "upload";
        public const string CleanUpVerbName = "cleanup";
        public const string AuthVerbName = "auth";

        [VerbOption(UploadVerbName, HelpText = "Handle native file upload dialog for a specified window.")]
        public UploadCmdArgs UploadVerb { get; set; }

        [VerbOption(CleanUpVerbName, HelpText = "Close all open modal dialogs.")]
        public CleanUpCmdArgs CleanUpVerb { get; set; }

        [VerbOption(AuthVerbName, HelpText = "Handle IE11 'Windows Security' dialog.")]
        public AuthCmdArgs AuthVerb { get; set; }

        [HelpVerbOption]
        public string GetUsage(string verb)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}