using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace ModalHandler.Test.App
{
    internal class WindowsSecurityDialog
    {
        private WinApi._CREDUI_INFO _credui;
        private readonly StringBuilder _usernameBuffer = new StringBuilder();
        private readonly StringBuilder _passwordBuffer = new StringBuilder();
        private int _maxLength = 100;
        private bool _save;

        internal WindowsSecurityDialog()
        {
            _credui = new WinApi._CREDUI_INFO
            {
                pszCaptionText = "Windows Security",
                pszMessageText = "Message",
                cbSize = Marshal.SizeOf(_credui)
            };
        }

        public string Username { get; private set; }
        public string Password { get; private set; }
        public bool IsSuccess { get; private set; }

        public void ShowXaml()
        {
            // http://www.pinvoke.net/default.aspx/credui.CredUIPromptForWindowsCredentials
            const int CREDUIWIN_GENERIC = 0x1;
            uint authPackage = 0;
            IntPtr outCredBuffer;
            uint outCredSize;
            // Show the dialog.
            IsSuccess = WinApi.CredUIPromptForWindowsCredentials(ref _credui,
                            0,
                            ref authPackage,
                            IntPtr.Zero,
                            0,
                            out outCredBuffer,
                            out outCredSize,
                            ref _save,
                            CREDUIWIN_GENERIC) == 0;
            var maxDomain = 100;
            var domainBuf = new StringBuilder(maxDomain);
            if (!IsSuccess) return;
            // Try unpack credentials.
            IsSuccess = WinApi.CredUnPackAuthenticationBuffer(0, outCredBuffer, outCredSize, _usernameBuffer,
                ref _maxLength, domainBuf, ref maxDomain, _passwordBuffer, ref _maxLength);
            if (!IsSuccess) return;
            //clear the memory allocated by CredUIPromptForWindowsCredentials 
            WinApi.CoTaskMemFree(outCredBuffer);
            var networkCredential = new NetworkCredential
            {
                UserName = _usernameBuffer.ToString(),
                Password = _passwordBuffer.ToString(),
                Domain = domainBuf.ToString()
            };
            Username = networkCredential.UserName;
            Password = networkCredential.Password;
        }

        public void ShowWin32()
        {
            // http://www.pinvoke.net/default.aspx/credui.CredUIPromptForCredentialsW
            const string domain = "Domain";
            // Show the dialog.
            IsSuccess = WinApi.CredUIPromptForCredentialsW(ref _credui, domain, IntPtr.Zero, 0,
                _usernameBuffer, 100, _passwordBuffer,
                10, ref _save, 0) == 0;
            if (!IsSuccess) return;
            // Remove the domain.
            Username = _usernameBuffer.ToString().Replace(domain, string.Empty).Trim('\\');
            Password = _passwordBuffer.ToString();
        }
    }
}
