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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string target)
            {
                await Switcher(target);
            }
        }

        private async Task Switcher(string target)
        {
            var startArguments = $"SetActiveInput {target}";
            var programFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
            if (string.IsNullOrEmpty(programFilesX86))
            {
                MessageBox.Show("Die Umgebungsvariable 'ProgramFiles(x86)' ist nicht gesetzt.");
                return;
            }
            var processPath = System.IO.Path.Combine(programFilesX86, "Dell", "Dell Display Manager", "ddm.exe");

            if (!File.Exists(processPath))
            {
                MessageBox.Show($"Pfad {processPath} wurde nicht gefunden.");
                return;
            }

            try
            {
                await Task.Run(() =>
                {
                    var ddm = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            UseShellExecute = false,
                            Arguments = startArguments,
                            FileName = processPath,
                            CreateNoWindow = true
                        }
                    };
                    ddm.Start();
                });

                await Dispatcher.InvokeAsync(() => lblSwitchedTo.Content = $"Switched to: {target}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Umschalten: {ex.Message}");
                lblSwitchedTo.Content = "Fehler beim Umschalten";
            }
        }
    }
}
