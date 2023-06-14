using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dietapp
{
    /// <summary>
    /// Interaction logic for MissingFeatureWindow.xaml
    /// </summary>
    public partial class MissingFeatureWindow : Window
    {
        public MissingFeatureWindow()
        {
            InitializeComponent();

            btnOkay.Click += BtnOkay_Click;
            btnClose.Click += BtnClose_Click;
        }

        private void BtnOkay_Click(object sender, RoutedEventArgs e)
        {
            string setupFilePath = Path.GetFullPath(@".\SqlLocalDB.msi");
            Process process = new Process();
            process.StartInfo.FileName = "msiexec";
            process.StartInfo.Arguments = String.Format(" /i \"{0}\"", setupFilePath);
            process.Start();
            process.WaitForExit(60000);
            this.Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
