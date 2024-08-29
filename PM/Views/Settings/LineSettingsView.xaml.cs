using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PM.Views
{
    /// <summary>
    /// Логика взаимодействия для LineSettingsView.xaml
    /// </summary>
    public partial class LineSettingsView : UserControl
    {
        public LineSettingsView()
        {
            InitializeComponent();
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
        private void TextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox.Text.Length > 0 && textBox.Text[0] == '0')
            {
                textBox.Text = textBox.Text.Remove(0, 1);
            }

            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "1";
            }
        }
    }
}
