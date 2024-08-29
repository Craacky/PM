using System.ComponentModel;
using System.Windows;

namespace PM.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Height = SystemParameters.VirtualScreenHeight - 45;
            Width = SystemParameters.VirtualScreenWidth;
        }
        private void TitleBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

    }
}
