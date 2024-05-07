using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagement.Models;
using LibraryManagement.Views;

namespace LibraryManagement.ViewModels
{
    public class CategoryViewModel : BaseViewModel
    {
         private ObservableCollection<Models.Category> _ListCategory;
        public ObservableCollection<Models.Category> ListCategory { get => _ListCategory; set { _ListCategory = value; OnPropertyChanged(); } }

        private Category _SelectedItem;
        public Category SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    IdCategory = SelectedItem.id;
                    NameCategory = SelectedItem.name;
                }
            }
        }

        private long _id;
        public long IdCategory { get => _id; set { _id = value; OnPropertyChanged(); } }

        private string _name;
        public string NameCategory { get => _name; set { _name = value; OnPropertyChanged(); } }

        public ICommand AddCategoryCommand { get; set; }

        public ICommand AddCategoryToDBCommand { get; set; }
        public ICommand DeleteCategorytoDBCommand { get; set; }

        public CategoryViewModel()
        {
            ListCategory = new ObservableCollection<Category>(DataSingleton.Instance.DB.Categories);

            AddCategoryCommand = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                AddCategoryWindow wd = new AddCategoryWindow();
                NameCategory = null;
                wd.ShowDialog();
            });

            //Add Category
            AddCategoryToDBCommand = new AppCommand<object>((p) =>
            {
                if (NameCategory == null || NameCategory == "")
                    return false;
                return true;

            }, (p) =>
            {
                if (NameCategory == null)
                {
                    MessageBox.Show("Thể loại không được bỏ trống");
                    return; 
                }
                var displayList = DataSingleton.Instance.DB.Categories.Where(x => x.name.ToLower() == NameCategory.ToLower());
                if (displayList.Count() != 0)
                {
                    MessageBox.Show("Thể loại bị trùng");
                    NameCategory = null;
                    return;
                }
                var category = new Category()
                {
                    name = NameCategory
                };

                DataSingleton.Instance.DB.Categories.Add(category);
                DataSingleton.Instance.DB.SaveChanges();

                ListCategory.Add(category);
                MessageBox.Show("Thêm thể loại thành công");
            });

            //Delete Category
            DeleteCategorytoDBCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var category = DataSingleton.Instance.DB.Categories.Where(x => x.id == SelectedItem.id).SingleOrDefault();

                if (DataSingleton.Instance.DB.Books.Where(b => b.id == category.id).Count() > 0) {
                    MessageBox.Show("Không thể xóa thể loại do thể loại còn được tham chiếu trong sách");
                }
                else
                {
                    DataSingleton.Instance.DB.Categories.Remove(category);
                    DataSingleton.Instance.DB.SaveChanges();
                    ListCategory.Remove(category);
                    MessageBox.Show("Xóa thể loại thành công");
                }
            });
        }
    }
}