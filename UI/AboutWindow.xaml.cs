using System.Reflection;
using System.Windows;

namespace SlantWPF.UI
{
    /// <summary>
    /// Interaktionslogik für AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public string Version { get; }
        public string BuildDate { get; }

        public AboutWindow()
        {
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            BuildDate = Properties.Resources.BuildDate.TrimEnd(' ', '\n', '\r');
            InitializeComponent();
        }
    }
}
