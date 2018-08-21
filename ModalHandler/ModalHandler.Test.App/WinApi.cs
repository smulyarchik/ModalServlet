using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ModalHandler.Test.App
{
    internal static class WinApi
    {
        [DllImport("credui", CharSet = CharSet.Unicode)]
        public static extern int CredUIPromptForCredentialsW(ref _CREDUI_INFO creditUR, 
            string targetName,
            IntPtr reserved1,
            int iError,
            StringBuilder userName,
            int maxUserName,
            StringBuilder password,
            int maxPassword,
            [MarshalAs(UnmanagedType.Bool)] ref bool pfSave,
            int flags);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct _CREDUI_INFO
        {
            public int cbSize;
            public IntPtr hwndParent;
            public string pszMessageText;
            public string pszCaptionText;
            public IntPtr hbmBanner;
        }

        [DllImport("credui.dll", CharSet = CharSet.Unicode)]
        public static extern uint CredUIPromptForWindowsCredentials(
            ref _CREDUI_INFO notUsedHere,
            int authError,
            ref uint authPackage,
            IntPtr InAuthBuffer,
            uint InAuthBufferSize,
            out IntPtr refOutAuthBuffer,
            out uint refOutAuthBufferSize,
            ref bool fSave,
            int flags);

        [DllImport("credui.dll", CharSet = CharSet.Auto)]
        public static extern bool CredUnPackAuthenticationBuffer(int dwFlags, IntPtr pAuthBuffer, uint cbAuthBuffer, StringBuilder pszUserName, ref int pcchMaxUserName, StringBuilder pszDomainName, ref int pcchMaxDomainame, StringBuilder pszPassword, ref int pcchMaxPassword);

        [DllImport("ole32.dll")]
        public static extern void CoTaskMemFree(IntPtr ptr);
    }
}
