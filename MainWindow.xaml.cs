using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.ComponentModel;
using System.IO;

namespace DesktopSwitcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_HDMI1 (object sender, RoutedEventArgs e)
        {
            string Target = "HDMI1";
            Switcher(Target);
        }
        private void Button_Click_HDMI2(object sender, RoutedEventArgs e)
        {
            string Target = "HDMI2";
            Switcher(Target);
        }
        private void Button_Click_USBC(object sender, RoutedEventArgs e)
        {
            string Target = "USB-C";
            Switcher(Target);
        }

        private void Switcher(string Target)
        {
            bool isError = false;
            
            string StartArguments = "SetActiveInput " + Target;
            string ProcessPath = Environment.GetEnvironmentVariable("ProgramFiles(x86)") + "\\Dell\\Dell Display Manager\\ddm.exe";
            
            if(!File.Exists(ProcessPath))
            {
                string MsgBoxText = "Path " + ProcessPath + " ist not found";
                MessageBox.Show(MsgBoxText);

            }

            Process ddm = new Process();

            try
            {
                // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.useshellexecute?view=netframework-4.8
                ddm.StartInfo.UseShellExecute = false;

                ddm.StartInfo.Arguments = StartArguments;

                ddm.StartInfo.FileName = ProcessPath;

                ddm.StartInfo.CreateNoWindow = true;

                ddm.Start();

            }

            catch (Exception e)
            {
                isError = true;
                Console.WriteLine(e.Message);
            }

            if(isError == false)
            {
                lblSwitchtedTo.Content = "Switched to: " + Target;
            }
        
        }
    }
}
