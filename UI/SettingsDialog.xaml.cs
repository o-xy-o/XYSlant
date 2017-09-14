using System.Windows;
using System.Windows.Forms;

namespace SlantWPF.UI
{
    /// <summary>
    /// Interaktionslogik für SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {
        public SettingsDialog()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public int NumericWidth
        {
            get { return (int)((NumericUpDown)numericWidth.Child).Value; }
            set { ((NumericUpDown)numericWidth.Child).Value = value; }
        }

        public int NumericHeight
        {
            get { return (int)((NumericUpDown)numericHeight.Child).Value; }
            set { ((NumericUpDown)numericHeight.Child).Value = value; }
        }
    }
}
