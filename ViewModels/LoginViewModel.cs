using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using LibraryManagement.Models;
using LibraryManagement.Views;

namespace LibraryManagement.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users { get => _users; set { _users = value; OnPropertyChanged(); } }

        private ObservableCollection<Role> _role;
        public ObservableCollection<Role> Role { get => _role; set { _role = value; OnPropertyChanged(); } }

        public Window main;

        private User _currentUser;
        public User CurrentUser { get => _currentUser; set { _currentUser = value; OnPropertyChanged(); } }

        private long _idStaff;
        public long IdStaff 
        { get => _idStaff; set { _idStaff = value; OnPropertyChanged(); } }

        private string _account;
        public string Account
        {
            get => _account;
            set
            {
                _account = value; OnPropertyChanged();
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value; OnPropertyChanged();
            }
        }

        private string _namePermission;
        public string NamePermission
        {
            get => _namePermission;
            set
            {
                _namePermission = value;
                OnPropertyChanged();
            }
        }

        private int _idPermission;
        public int IdPermission
        {
            get => _idPermission;
            set
            {
                _idPermission = value;
                OnPropertyChanged();
            }
        }

        private string _nameStaff;
        public string NameStaff
        {
            get => _nameStaff;
            set
            {
                _nameStaff = value; 
                OnPropertyChanged();
            }
        }

        private bool _canChangeRegualtion;
        public bool CanChangeRegulation
        {
            get => _canChangeRegualtion;
            set
            {
                _canChangeRegualtion = value;
                OnPropertyChanged();
            }
        }

        private bool _canChangePermission;
        public bool CanChangePermission
        {
            get => _canChangePermission;
            set
            {
                _canChangePermission = value;
                OnPropertyChanged();
            }
        }
        public AppCommand<object> EditPasswordCommand { get; }
        public AppCommand<object> ChangePasswordCommand { get; }
        public AppCommand<object> LogoutCommand { get; }
        public AppCommand<object> LoginCommand { get; }
        public LoginViewModel()
        {
            RetrieveData();
            LoginCommand = new AppCommand<object>((p) =>
            {
                if (Password == null || Account == null)
                    return false;
                return true;

            }, (p) =>
            {
                
                for (int i = 0; i < Users.Count; i++)
                {
                    string username = Users[i].username;
                    string password = Users[i].password;
                    if (username == Account && password == EncryptSha512Managed(Password))
                    {
                        CurrentUser = Users[i];
                        NameStaff = CurrentUser.name;
                        IdStaff = CurrentUser.id;
                        CanChangeRegulation = CurrentUser.Roles
                            .Select(x => x.name)
                            .Contains("ADMIN");
                        CanChangePermission = CurrentUser.Roles
                            .Select(x => x.name)
                            .Contains("ADMIN");
                        break;
                    }
                    else CurrentUser = null;
                }    
                if (CurrentUser == null)
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!", 
                        "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    main = new MainWindow();
                    if (Application.Current.MainWindow != null)
                        Application.Current.MainWindow.Visibility = Visibility.Hidden;
                    main.ShowDialog();
                }
            });

            ChangePasswordCommand = new AppCommand<object>((p) =>
            {
                if (CurrentUser == null)
                    return false;
                return true;
            }, (p) =>
            {
                Window changePasswordWindow = new ChangePasswordWindow();
                changePasswordWindow.ShowDialog();
            });

            LogoutCommand = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                main.Close();
                RetrieveData();
            });

            EditPasswordCommand = new AppCommand<object>((p) =>
            {
                if (CurrentUser == null || Password == null || NewPassword == null || ConfirmPassword == null)
                    return false;
                return true;

            }, (p) =>
            {
                var user = DataSingleton.Instance.DB.Users.FirstOrDefault(x => x.id == CurrentUser.id);
                if (user == null)
                {
                    MessageBox.Show("Tài khoản không tồn tại");
                    return;
                }
                if (user.password != EncryptSha512Managed(Password))
                {
                    MessageBox.Show("Mật khẩu cũ không chính xác!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (NewPassword != ConfirmPassword)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                user.password = EncryptSha512Managed(NewPassword);
                DataSingleton.Instance.DB.Users.AddOrUpdate(user);
                DataSingleton.Instance.DB.SaveChanges();
                MessageBox.Show("Bạn đã đổi mật khẩu thành công");
            });
        }

        public string EncryptSha512Managed(string password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytPassword = uEncode.GetBytes(password);
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(bytPassword);
            return Convert.ToBase64String(hash);
        }
        private void RetrieveData()
        {
            Users = new ObservableCollection<User>(DataSingleton.Instance.DB.Users);
            Role = new ObservableCollection<Role>(DataSingleton.Instance.DB.Roles);
        }
    }
}