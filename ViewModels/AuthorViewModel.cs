using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagement.Models;
using LibraryManagement.Views;

namespace LibraryManagement.ViewModels
{
    public class AuthorViewModel : BaseViewModel
    {private ObservableCollection<Models.Author> _ListAuthor;
        public ObservableCollection<Models.Author> ListAuthor { get => _ListAuthor; set { _ListAuthor = value; OnPropertyChanged(); } }

        private Author _SelectedItem;
        public Author SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    IdAuthor = SelectedItem.id;
                    NameAuthor = SelectedItem.name;
                }
            }
        }

        private long _id;
        public long IdAuthor { get => _id; set { _id = value; OnPropertyChanged(); } }

        private string _name;
        public string NameAuthor { get => _name; set { _name = value; OnPropertyChanged(); } }

        public ICommand AddAuthorCommand { get; set; }

        public ICommand AddAuthorToDBCommand { get; set; }
        public ICommand DeleteAuthortoDBCommand { get; set; }

        public AuthorViewModel()
        {
            ListAuthor = new ObservableCollection<Author>(DataSingleton.Instance.DB.Authors);

            AddAuthorCommand = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                AddAuthorWindow wd = new AddAuthorWindow();
                NameAuthor = null;
                wd.ShowDialog();
            });

            //AddAuthor
            AddAuthorToDBCommand = new AppCommand<object>((p) =>
            {
                if (NameAuthor == null || NameAuthor == "")
                    return false;
                return true;
            }, (p) =>
            {
                if (NameAuthor == null)
                {
                    MessageBox.Show("Tên tác giả không được bỏ trống");
                    return;
                }
                var displayList = DataSingleton.Instance.DB.Authors.Where(x => x.name.ToLower() == NameAuthor.ToLower());
                if (displayList.Count() != 0)
                {
                    MessageBox.Show("Tên tác giả bị trùng");
                    NameAuthor = null;
                    return;
                }
                var author = new Author()
                {
                    name = NameAuthor
                };

                DataSingleton.Instance.DB.Authors.Add(author);
                DataSingleton.Instance.DB.SaveChanges();

                ListAuthor.Add(author);
                MessageBox.Show("Thêm tác giả thành công");
            });

            //Delete Author
            DeleteAuthortoDBCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                    var author = DataSingleton.Instance.DB.Authors.Where(x => x.id == SelectedItem.id).SingleOrDefault();
                    foreach(var el in DataSingleton.Instance.DB.Books)
                    {
                        if (el.Authors.Where(a => a.id == author.id).Count() > 0)
                        {
                            MessageBox.Show("Không thể xóa tác giả do tác giả còn được tham chiếu trong sách");
                            return;
                        }
                    }
                    DataSingleton.Instance.DB.Authors.Remove(author);
                    DataSingleton.Instance.DB.SaveChanges();
                    ListAuthor.Remove(author);
                    MessageBox.Show("Xóa tác giả thành công");
            });
        }
    }
}