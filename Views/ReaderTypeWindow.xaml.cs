using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LibraryManagement.Views
{
    public partial class ReaderTypeWindow : Window
    {
        public ReaderTypeWindow()
        {
            InitializeComponent();
        }
        
        private void btnAddTypeReader_Click(object sender, RoutedEventArgs e)
        {
            AddReaderTypeWindow w = new AddReaderTypeWindow();
            w.ShowDialog();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}