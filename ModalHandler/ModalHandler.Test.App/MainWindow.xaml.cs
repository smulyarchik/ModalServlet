using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace ModalHandler.Test.App
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowsSecurity_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new WindowsSecurityDialog();
            if (((Button) sender).Content.ToString().Contains("Win32"))
                dialog.ShowWin32();
            else
                dialog.ShowXaml();
            if (dialog.IsSuccess)
                StatusBox.Text = $"{dialog.Username} {dialog.Password}";
        }

        private void FileUpload_Click(object sender, RoutedEventArgs e)
        {
            var fileUploadDlg = new OpenFileDialog();
            var result = fileUploadDlg.ShowDialog();
            if (result == true)
                StatusBox.Text = fileUploadDlg.FileName;
        }
    }
}