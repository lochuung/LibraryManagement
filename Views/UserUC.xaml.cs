using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace LibraryManagement.Views
{
    public partial class UserUC : UserControl
    {
        public UserUC()
        {
            InitializeComponent();
        }
        
         private void btnAddStaff_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Command.CanExecute(null))
            {
                button.Command.Execute(null);
            }
            AddUserWindow wd = new AddUserWindow();
            wd.ShowDialog();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            btnSua.Visibility = Visibility.Hidden;
            btnLuu.Visibility = Visibility.Visible;
            NameStaff.IsReadOnly = false;
            DobStaff.IsEnabled = true;
            UserName.IsReadOnly = false;
            btnAddStaff.IsEnabled = false;
            btnDeleStaff.IsEnabled = false;
            SearchBox.IsEnabled = false;
            lvNhanVien.IsEnabled = false;
            btnCancel.Visibility = Visibility.Visible;
        }

        private void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            btnLuu.Visibility = Visibility.Hidden;
            btnSua.Visibility = Visibility.Visible;
            NameStaff.IsReadOnly = true;
            DobStaff.IsEnabled = false;
            UserName.IsReadOnly = true;
            btnAddStaff.IsEnabled = true;
            btnDeleStaff.IsEnabled = true;
            SearchBox.IsEnabled = true;
            lvNhanVien.IsEnabled = true;
            btnCancel.Visibility = Visibility.Hidden;
        }

        private void lvNhanVien_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvNhanVien.UnselectAll();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            btnLuu.Visibility = Visibility.Hidden;
            btnSua.Visibility = Visibility.Visible;
            NameStaff.IsReadOnly = true;
            DobStaff.IsEnabled = false;
            UserName.IsReadOnly = true;
            btnAddStaff.IsEnabled = true;
            btnDeleStaff.IsEnabled = true;
            SearchBox.IsEnabled = true;
            lvNhanVien.IsEnabled = true;
            btnCancel.Visibility = Visibility.Hidden;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }
    }
}