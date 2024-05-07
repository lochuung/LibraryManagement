using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using LibraryManagement.ViewModels;

namespace LibraryManagement.Views
{
    public partial class BookUC : UserControl
    {
        public BookUC()
        {
            InitializeComponent();
        }private void UpdateBook_Click(object sender, RoutedEventArgs e)
        {
            // Set source binding to BookImageCover
            Binding imgBinding = new Binding("BookImageCover");

            SaveBook.Visibility = Visibility.Visible;
            CancelUpdate.Visibility = Visibility.Visible;
            UpdateBook.Visibility = Visibility.Hidden;
            NameBook.IsReadOnly = false;
            Publisher.IsEnabled = true;
            DateManufacture.IsEnabled = true;
            Price.IsReadOnly = false;
            AddBook.IsEnabled = false;

            tbAuthors.Visibility = Visibility.Hidden;
            tbCategories.Visibility = Visibility.Hidden;
            changeAuthors.Visibility = Visibility.Visible;
            changeCategories.Visibility = Visibility.Visible;
            lbAuthor.VerticalAlignment = VerticalAlignment.Top;
            inforGrid.RowDefinitions[1].Height = new GridLength(3, GridUnitType.Star);
            inforGrid.RowDefinitions[2].Height = new GridLength(3, GridUnitType.Star);

            SearchBox.IsEnabled = false;
            operation.Visibility = Visibility.Hidden;
            //Tắt hết các tính năng không liên quan

            DeleteBook.Visibility = Visibility.Hidden;
            //StopBorrowBook.Visibility = Visibility.Hidden;
            ListDisplayBook.IsEnabled = false;
            paginating.IsEnabled = false;
            SearchBook.IsEnabled = false;

            //UpdateBook.Command.Execute(null);
        }

        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            // Set source binding back to SelectedItem.image
            Binding imgBinding = new Binding("SelectedItem.image");
            imgBinding.Converter = new ImageToSource();

            SaveBook.Visibility = Visibility.Hidden;
            UpdateBook.Visibility = Visibility.Visible;
            NameBook.IsReadOnly = false;
            Publisher.IsEnabled = false;
            DateManufacture.IsEnabled = false;
            Price.IsReadOnly = true;
            AddBook.IsEnabled = true;

            tbAuthors.Visibility = Visibility.Visible;
            tbCategories.Visibility = Visibility.Visible;
            changeAuthors.Visibility = Visibility.Hidden;
            changeCategories.Visibility = Visibility.Hidden;
            lbAuthor.VerticalAlignment = VerticalAlignment.Center;
            inforGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            inforGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);

            SearchBox.IsEnabled = true;

            operation.Visibility = Visibility.Visible;

            //Mở lại các tính năng đã khóa
            DeleteBook.Visibility = Visibility.Visible;

            ListDisplayBook.IsEnabled = true;
            paginating.IsEnabled = true;
            SearchBook.IsEnabled = true;
        }

        private void CancelUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Set source binding back to SelectedItem.image
            Binding imgBinding = new Binding("SelectedItem.image");
            imgBinding.Converter = new ImageToSource();

            CancelUpdate.Visibility = Visibility.Hidden;
            UpdateBook.Visibility = Visibility.Hidden;
            SaveBook.Visibility = Visibility.Hidden;
            operation.Visibility = Visibility.Visible;
            AddBook.IsEnabled = true;

            UpdateBook.Visibility = Visibility.Visible;

            tbAuthors.Visibility = Visibility.Visible;
            tbCategories.Visibility = Visibility.Visible;
            changeAuthors.Visibility = Visibility.Hidden;
            changeCategories.Visibility = Visibility.Hidden;
            lbAuthor.VerticalAlignment = VerticalAlignment.Center;
            inforGrid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            inforGrid.RowDefinitions[2].Height = new GridLength(1, GridUnitType.Star);

            
            ListDisplayBook.IsEnabled = true;

            SearchBox.IsEnabled = true;

            SaveBook.Visibility = Visibility.Hidden;

            NameBook.IsReadOnly = true;
            tbCategories.IsEnabled = false;
            Publisher.IsEnabled = false;
            DateManufacture.IsEnabled = false;
            Price.IsReadOnly = true;
            //Tắt hết các tính năng không liên quan
            DeleteBook.Visibility = Visibility.Visible;
            paginating.IsEnabled = true;
            SearchBook.IsEnabled = true;
        }

        private void ListDisplayBook_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                ListDisplayBook.UnselectAll();
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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}