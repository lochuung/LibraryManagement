using System.Windows;

namespace LibraryManagement.Views
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }
        
        private void currPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtCurr.Text = currPassword.Password;
        }

        private void newPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtNew.Text = newPassword.Password;
        }

        private void confirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtConfirm.Text = confirmPassword.Password;
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}