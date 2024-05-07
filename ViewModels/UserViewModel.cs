using System;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using LibraryManagement.Models;

namespace LibraryManagement.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        private ObservableCollection<User> _List;
        public ObservableCollection<User> List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<Role> _Permission;
        public ObservableCollection<Role> Permission { get => _Permission; set { _Permission = value; OnPropertyChanged(); } }

        private User _SelectedItem;

        public User SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    NameUser = SelectedItem.name;
                    DobUser = SelectedItem.birthday;
                    AccountUser = SelectedItem.username;
                    IdPermission = SelectedItem.Roles.FirstOrDefault().id;
                    SelectedPermission = SelectedItem.Roles.FirstOrDefault();
                }
               
            }
        }

        private string _name;
        public string NameUser
        {
            get => _name;
            set
            {
                string str = value as string;
                _name = value; OnPropertyChanged();
            }
        }

        private string _address;
        public string AddressUser
        {
            get => _address;
            set
            {
                _address = value; OnPropertyChanged();
            }
        }

        private string _birthday;
        public string DobUser
        {
            get => _birthday;
            set
            {
                _birthday = value; OnPropertyChanged();
            }
        }

        private string _phoneNumberUser;
        public string PhoneNumberUser
        {
            get => _phoneNumberUser;
            set
            {
                _phoneNumberUser = value; OnPropertyChanged();
            }
        }

        private string _accountUser;
        public string AccountUser
        {
            get => _accountUser;
            set
            {
                _accountUser = value; OnPropertyChanged();
            }
        }

        private string _passwordUser;
        public string PasswordUser
        {
            get => _passwordUser;
            set
            {
                _passwordUser = value; OnPropertyChanged();
            }
        }

        private Role _SelectedPermission;
        public Role SelectedPermission
        {
            get => _SelectedPermission;
            set
            {
                _SelectedPermission = value;
                OnPropertyChanged();
            }
        }

        private long _idPermission;
        public long IdPermission
        {
            get => _idPermission;
            set
            {
                _idPermission = value;
                OnPropertyChanged();
            }
        }

        private string randomPassword()
        {
            Random rd = new Random();
            return rd.Next(1000, 9999).ToString();
        }

        //Search Reader
        private string staffSearchKeyword;
        public string UserSearchKeyword
        {
            get => staffSearchKeyword;
            set
            {
                staffSearchKeyword = value;
                OnPropertyChanged();
                SearchUser();
            }
        }

        private void SearchUser()
        {
            if (UserSearchKeyword == null || UserSearchKeyword.Trim() == "")
            {
                List = new ObservableCollection<User>(DataSingleton.Instance.DB.Users);
                return;
            }
            try
            {
                var result = DataSingleton.Instance.DB.Users.Where(
                                    staff => staff.name.ToLower().StartsWith(UserSearchKeyword.ToLower())
                                    );
                List = new ObservableCollection<User>(result);
            }
            catch (ArgumentNullException)
            {
                List = new ObservableCollection<User>(DataSingleton.Instance.DB.Users);
                MessageBox.Show("Từ khóa tìm kiếm rỗng!");
            }
        }

        public ICommand ResetPasswordCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand PrepareAddCommand { get; set; }
        public ICommand InitProperties { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CancelCommandAdd { get; set; }
        public ICommand NotifyOnSelectedItemChange { get; set; }

        public UserViewModel()
        {
            // Retrieve data from DB
            RetrieveData();
            CancelCommandAdd = new AppCommand<object>(
                p => true,
                p =>
                {
                    if (SelectedItem != null)
                    {
                        NameUser = SelectedItem.name;
                        DobUser = SelectedItem.birthday;
                        AccountUser = SelectedItem.username;
                        IdPermission = SelectedItem.Roles.FirstOrDefault().id;
                        SelectedPermission = SelectedItem.Roles.FirstOrDefault();
                    }
                });
            CancelCommand = new AppCommand<object>((p) =>
            {
                return true;

            }, (p) =>
            {
                SelectedItem.name = NameUser;
                SelectedItem.birthday = DobUser;
                SelectedItem.username = AccountUser;
                SelectedItem.Roles = new ObservableCollection<Role> { SelectedPermission };
                DataSingleton.Instance.DB.Users.AddOrUpdate(SelectedItem);
                
                DataSingleton.Instance.DB.SaveChanges();
                OnPropertyChanged("SelectedItem");
            });
            AddCommand = new AppCommand<object>((p) =>
            {
                if (NameUser == null || AccountUser == null || PasswordUser == null)
                    return false;
                return true;

            }, (p) =>
            {
                try
                {
                    var User = new User()
                    {
                        name = NameUser,
                        birthday = DobUser,
                        username = AccountUser,
                        password = EncryptSHA512Managed(PasswordUser),
                        Roles = new ObservableCollection<Role> { SelectedPermission }
                    };
                    DataSingleton.Instance.DB.Users.Add(User);
                    DataSingleton.Instance.DB.SaveChanges();
                    List.Add(User);
                    SelectedItem = User;
                    PasswordUser = null;
                    MessageBox.Show("Bạn đã thêm nhân viên thành công");
                }
                catch (Exception)
                {
                    MessageBox.Show("Bạn chưa chọn loại quyền!");
                }
            });

            EditCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedPermission == null)
                    return false;
                return true;

            }, (p) =>
            {

                var User = DataSingleton.Instance.DB.Users.Where(x => x.id == SelectedItem.id).SingleOrDefault();
                User.name = SelectedItem.name;
                User.birthday = SelectedItem.birthday;
                User.username = SelectedItem.username;
                User.Roles = new ObservableCollection<Role> { SelectedPermission };
                DataSingleton.Instance.DB.Users.AddOrUpdate(User);
                DataSingleton.Instance.DB.SaveChanges();
                System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(List);
                view.Refresh();
                MessageBox.Show("Bạn đã sửa thông tin nhân viên thành công");
            });
            DeleteCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedItem?.username == "admin")
                    return false;

                return true;

            }, (p) =>
            {
                var User = DataSingleton.Instance.DB.Users.Where(x => x.id == SelectedItem.id).SingleOrDefault();
                DataSingleton.Instance.DB.Users.Remove(User);
                DataSingleton.Instance.DB.SaveChanges();
                List.Remove(User);
                SelectedItem = List.FirstOrDefault();
                MessageBox.Show("Bạn đã xóa nhân viên thành công");
            });

            ResetPasswordCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedPermission == null)
                    return false;
                return true;

            }, (p) =>
            {

                var User = DataSingleton.Instance.DB.Users.Where(x => x.id == SelectedItem.id).SingleOrDefault();
                PasswordUser = randomPassword();
                User.password = EncryptSHA512Managed(PasswordUser);
                DataSingleton.Instance.DB.Users.AddOrUpdate(User);
                DataSingleton.Instance.DB.SaveChanges();
                System.ComponentModel.ICollectionView view = CollectionViewSource.GetDefaultView(List);
                view.Refresh();
                MessageBox.Show("Bạn đã reset mật khẩu cho tài khoản " + User.username + " thành công, mật khẩu mới là: " + PasswordUser + ", hãy đăng nhập và đổi mật khẩu mới!");
            });

            PrepareAddCommand = new AppCommand<object>(
                p => true,
                p =>
                {
                    NameUser = null;
                    DobUser = new DateTime(2004, 1, 1).ToString(CultureInfo.InvariantCulture);
                    PhoneNumberUser = null;
                    AddressUser = null;
                    AccountUser = null;
                    PasswordUser = null;
                    SelectedPermission = Permission.FirstOrDefault();
                });
            InitProperties = new AppCommand<object>(
                p => true,
                p =>
                {
                    SelectedItem = List.FirstOrDefault();
                });
            NotifyOnSelectedItemChange = new AppCommand<object>(
                p => true,
                p =>
                {
                    OnPropertyChanged("SelectedItem");
                });
        }

        public string EncryptSHA512Managed(string password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] bytPassword = uEncode.GetBytes(password);
            SHA512Managed sha = new SHA512Managed();
            byte[] hash = sha.ComputeHash(bytPassword);
            return Convert.ToBase64String(hash);
        }

        private void RetrieveData()
        {
            List = new ObservableCollection<User>(DataSingleton.Instance.DB.Users);
            Permission = new ObservableCollection<Role>(DataSingleton.Instance.DB.Roles);
        }

        private Role selectedRole;
    }
}