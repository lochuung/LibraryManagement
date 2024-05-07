using System.Windows;

namespace LibraryManagement.Views
{
    public partial class AddAuthorWindow : Window
    {
        public AddAuthorWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}