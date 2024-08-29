using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PM.Views
{
    /// <summary>
    /// Логика взаимодействия для HandleAggregationAddPalletView.xaml
    /// </summary>
    public partial class HandleAggregationAddPalletView : UserControl
    {
        public HandleAggregationAddPalletView()
        {
            InitializeComponent();
        }


        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (e.Key == Key.Enter)
            {
                textBox.Text += "\0";
                return;

            }
        }
    }
}
