using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace LibraryManagement.Views
{
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PhoneNumberStaff_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void mPasswordStaff_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordStaff.Text = mPasswordStaff.Password;
        }
    }
}