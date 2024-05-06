using System.Windows;
using System.Windows.Controls;

namespace LibraryManagement.Views
{
    public partial class HomeUC : UserControl
    {
        public HomeUC()
        {
            InitializeComponent();
        }
        
        private void ButtonBorrowBook_Click(object sender, RoutedEventArgs e)
        {
            Window window = new BorrowBookWindow();
            window.ShowDialog();
        }
        private void ButtonReturnBook_Click(object sender, RoutedEventArgs e)
        {
            Window window = new ReturnBookWindow();
            window.ShowDialog();
        }
        private void ButtonCollectFine_Click(object sender, RoutedEventArgs e)
        {
            Window window = new CollectFineWindow();
            window.ShowDialog();
        }

        private void ButtonAuthor_Click(object sender, RoutedEventArgs e)
        {
            Window window = new AuthorWindow();
            window.ShowDialog();
        }

        private void ButtonCategory_Click(object sender, RoutedEventArgs e)
        {
            Window window = new CategoryWindow();
            window.ShowDialog();
        }
    }
}