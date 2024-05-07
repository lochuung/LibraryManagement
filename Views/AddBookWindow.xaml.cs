using System.Windows;
using System.Windows.Controls;

namespace LibraryManagement.Views
{
    public partial class AddBookWindow : Window
    {
        public AddBookWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DateManufacture.GetBindingExpression(DatePicker.SelectedDateProperty)?.UpdateSource();
            NameBook.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            NameBook.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            TbPrice.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
            CbCategory.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
            CbAuthor.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
        }
    }
}