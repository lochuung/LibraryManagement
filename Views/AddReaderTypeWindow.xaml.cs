using System.Windows;

namespace LibraryManagement.Views
{
    public partial class AddReaderTypeWindow : Window
    {
        public AddReaderTypeWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}