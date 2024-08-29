using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace PM.Controls
{
    /// <summary>
    /// Логика взаимодействия для CameraSettingsControl.xaml
    /// </summary>
    public partial class CameraSettingsControl : System.Windows.Controls.UserControl
    {
        public CameraSettingsControl()
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
