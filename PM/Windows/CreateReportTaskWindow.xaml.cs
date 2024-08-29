using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PM.Windows
{
    public partial class CreateReportTaskWindow : Window
    {
        public CreateReportTaskWindow()
        {
            InitializeComponent();

            Width = SystemParameters.VirtualScreenWidth / 2.5;
            Height = SystemParameters.VirtualScreenHeight / 1.5;
        }


        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(CreateReportTaskWindow))
                {
                    window.DragMove();
                }
            }
        }
        private void gtinComboBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(gtinComboBox.Text))
                return;

            gtinComboBox.IsDropDownOpen = true;

            TextBox textBox = (TextBox)e.OriginalSource;
            textBox.Select(textBox.SelectionStart + textBox.SelectionLength, 0);
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(gtinComboBox.ItemsSource);
            if (cv != null)
            {
                cv.Filter = s =>
                (s.ToString().ToUpper()).Contains(gtinComboBox.Text.ToUpper(), StringComparison.CurrentCultureIgnoreCase);
            }
        }
        private void gtinComboBox_DropDownClosed(object sender, EventArgs e)
        {
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(gtinComboBox.ItemsSource);
            if (cv != null)
            {
                cv.Filter = s => true;
            }
        }
        private void gtinComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            CollectionView cv = (CollectionView)CollectionViewSource.GetDefaultView(gtinComboBox.ItemsSource);
            if (e.Key == Key.Back)
            {
                gtinComboBox.SelectedIndex = -2;
                gtinComboBox.SelectedItem = null;
            }
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
            }
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
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
        }
        private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ".")
            {
                e.Handled = true;
            }
        }
    }
}
