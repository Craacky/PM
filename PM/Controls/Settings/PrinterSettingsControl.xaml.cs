using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace PM.Controls
{
    /// <summary>
    /// Логика взаимодействия для PrinterSettingsControl.xaml
    /// </summary>
    public partial class PrinterSettingsControl : System.Windows.Controls.UserControl
    {
        public PrinterSettingsControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                pathTextBlock.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}
