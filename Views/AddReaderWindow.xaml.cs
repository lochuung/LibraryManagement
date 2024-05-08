using System.Windows;
using System.Windows.Controls;

namespace LibraryManagement.Views
{
    public partial class AddReaderWindow : Window
    {
        public AddReaderWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            NameReader.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            Email.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            DobReader.GetBindingExpression(DatePicker.SelectedDateProperty).UpdateSource();
        }
    }
}