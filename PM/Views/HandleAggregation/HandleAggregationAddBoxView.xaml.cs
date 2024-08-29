using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PM.Views
{
    /// <summary>
    /// Логика взаимодействия для HandleAggregationAddBoxView.xaml
    /// </summary>
    public partial class HandleAggregationAddBoxView : UserControl
    {
        public HandleAggregationAddBoxView()
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
