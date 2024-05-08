using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryManagement.Views
{
    public partial class ReaderUC : UserControl
    {
        public ReaderUC()
        {
            InitializeComponent();
        }
        
         private void ButtonAddReader_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Command.CanExecute(null))
            {
                button.Command.Execute(null);
            }
            AddReaderWindow wd = new AddReaderWindow();
            wd.ShowDialog();
        }

        private void UpdateReader_Click(object sender, RoutedEventArgs e)
        {
            SaveReader.Visibility = Visibility.Visible;
            UpdateReader.Visibility = Visibility.Hidden;
            NameReader.IsReadOnly = false;
            Email.IsReadOnly = false;
            DobReader.IsEnabled = true;
            CreatAt.IsEnabled = true;
            ListDisplayReader.IsEnabled = false;
            TypeReader.IsEnabled = true;
            btnAddReader.IsEnabled = false;
            btnDeleteReader.Visibility = Visibility.Hidden;
            btnCancel.Visibility = Visibility.Visible;
            btnPrev.IsEnabled = false;
            btnNext.IsEnabled = false;
            SearchBox.IsEnabled = false;
        }

        private void SaveReader_Click(object sender, RoutedEventArgs e)
        {
            SaveReader.Visibility = Visibility.Hidden;
            UpdateReader.Visibility = Visibility.Visible;
            NameReader.IsReadOnly = true;
            Email.IsReadOnly = true;
            DobReader.IsEnabled = false;
            CreatAt.IsEnabled = false;
            TypeReader.IsEnabled = false;
            ListDisplayReader.IsEnabled = true;
            btnDeleteReader.IsEnabled = true;
            btnAddReader.IsEnabled = true;

            btnDeleteReader.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Hidden;

            //btnTypeReader.IsEnabled = true;
            SearchBox.IsEnabled = true;
            //  btnCancel.IsEnabled = false;
            btnPrev.IsEnabled = true;
            btnNext.IsEnabled = true;
        }

        private void ButtonTypeReader_Click(object sender, RoutedEventArgs e)
        {
            ReaderTypeWindow w = new ReaderTypeWindow();
            w.ShowDialog();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            SaveReader.Visibility = Visibility.Hidden;
            UpdateReader.Visibility = Visibility.Visible;
            NameReader.IsReadOnly = true;
            Email.IsReadOnly = true;
            DobReader.IsEnabled = false;
            CreatAt.IsEnabled = false;
            TypeReader.IsEnabled = false;
            ListDisplayReader.IsEnabled = true;
            btnDeleteReader.IsEnabled = true;
            btnAddReader.IsEnabled = true;

            btnDeleteReader.Visibility = Visibility.Visible;
            btnCancel.Visibility = Visibility.Hidden;

            SearchBox.IsEnabled = true;
            btnPrev.IsEnabled = true;
            btnNext.IsEnabled = true;
        }

        private void ListDisplayReader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                ListDisplayReader.UnselectAll();
        }

        private void btnAuthors_Click(object sender, RoutedEventArgs e)
        {
            ReaderTypeWindow wd = new ReaderTypeWindow();
            wd.ShowDialog();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}