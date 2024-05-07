using System.Windows;
using System.Windows.Input;
using static LibraryManagement.Utils.CommonUtils;

namespace LibraryManagement
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            this.Icon = ConvertHBitmapSource(Properties.Resources.icon);
            LoginBackground.ImageSource = ConvertHBitmapSource(Properties.Resources.backgroundLogin);
        }
        
        private void matKhau_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passWord.Text = matKhau.Password;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (btnLogin.IsEnabled)
            {
                if (e.Key == Key.Enter)
                {
                    btnLogin.Command.Execute(null);
                }    
            }    
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}