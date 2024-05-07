using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagement.Models;
using LibraryManagement.Utils.Paginations;
using LibraryManagement.Views;

namespace LibraryManagement.ViewModels
{
    public class BookViewModel : BaseViewModel
    {
        private BookPaginatingCollection list;
        public BookPaginatingCollection List { get => list; set { list = value; OnPropertyChanged(); } }

        private BookPaginatingCollection _ListLatestBooks;
        public BookPaginatingCollection ListLatestBooks { get => _ListLatestBooks; set { _ListLatestBooks = value; OnPropertyChanged(); } }

        private ObservableCollection<Author> _ListAuthors;
        public ObservableCollection<Author> ListAuthors { get => _ListAuthors; set { _ListAuthors = value; OnPropertyChanged(); } }

        private Book _SelectedItem;
        public Book SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    title = SelectedItem.title;
                    SelectedPublisher = SelectedItem.Publisher;
                    SelectedCategories = new ObservableCollection<Category>(SelectedItem.Categories);
                    added_date = SelectedItem.added_date.Value;
                    manufacture_date = SelectedItem.manufacture_date.Value;
                    price = SelectedItem.price.Value;
                    ListAuthors = new ObservableCollection<Author>(SelectedItem.Authors);
                }
            }
        }

        private ObservableCollection<Category> _selectedCategories;
        public ObservableCollection<Category> SelectedCategories
        {
            get => _selectedCategories;
            set
            {
                _selectedCategories = value;
                OnPropertyChanged();
            }
        }
        
        private Category _SelectedCategory;
        public Category SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                OnPropertyChanged();
            }
        }

        private Publisher _SelectedPublisher;
        public Publisher SelectedPublisher
        {
            get => _SelectedPublisher;
            set
            {
                _SelectedPublisher = value;
                OnPropertyChanged();
            }
        }

        private Author _SelectedAuthor;
        public Author SelectedAuthor
        {
            get => _SelectedAuthor;
            set
            {
                _SelectedAuthor = value;
                OnPropertyChanged();
            }
        }

        private string _title;
        public string title { get => _title; set { _title = value; OnPropertyChanged(); } }

        private ObservableCollection<Category> _category;
        public ObservableCollection<Category> category { get => _category; set { _category = value; OnPropertyChanged(); } }

        private ObservableCollection<Publisher> _publisher;
        public ObservableCollection<Publisher> publisher { get => _publisher; set { _publisher = value; OnPropertyChanged(); } }

        private ObservableCollection<Author> _author;
        public ObservableCollection<Author> author { get => _author; set { _author = value; OnPropertyChanged(); } }

        private DateTime _manufacture_date;
        public DateTime manufacture_date { get => _manufacture_date; set { _manufacture_date = value; OnPropertyChanged(); } }

        private DateTime _added_date;
        public DateTime added_date
        {
            get
            {
                return _added_date;
            }
            set { _added_date = value; OnPropertyChanged(); }
        }

        private decimal _price;
        public decimal price { get => _price; set { _price = value; OnPropertyChanged(); } }

        private string _statusBook;
        public string statusBook { get => _statusBook; set { _statusBook = value; OnPropertyChanged(); } }

        public string SourceImageFile { get; set; }

        //Search Book
        private string bookSearchKeyword;
        public string BookSearchKeyword
        {
            get => bookSearchKeyword;
            set
            {
                bookSearchKeyword = value;
                OnPropertyChanged();
                InitBooks(bookSearchKeyword);
            }
        }

        public ICommand AddBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand CancelAddCommand { get; set; }

        public ICommand AddBookToDBCommand { get; set; }
        public ICommand EditBookToDBCommand { get; set; }

        public ICommand AddBookFromFileCommand { get; set; }
        public ICommand ExportBooksCommand { get; set; }

        public ICommand MoveToPreviousBooksPage { get; set; }
        public ICommand MoveToNextBooksPage { get; set; }
        public ICommand PrevBooks { get; set; }
        public ICommand NextBooks { get; set; }

        //Add Authors
        public ICommand AddAuthors { get; set; }

        //Delete Authors
        public ICommand UnSelectedAuthor { get; set; }
        
        // Add Category
        public ICommand AddCategoryCommand { get; set; }
        
        // Delete Category
        public ICommand UnSelectedCategoryCommand { get; set; }
        

        public BookViewModel()
        {
            SourceImageFile = null;
            category = new ObservableCollection<Category>(DataSingleton.Instance.DB.Categories);
            publisher = new ObservableCollection<Publisher>(DataSingleton.Instance.DB.Publishers);
            author = new ObservableCollection<Author>(DataSingleton.Instance.DB.Authors);
            ListAuthors = new ObservableCollection<Author>();
            SelectedCategories = new ObservableCollection<Category>();

            InitBooks();
            GetLastestBooks();

            CancelCommand = new AppCommand<object>((p) =>
            {
                return true;

            }, (p) =>
            {
                if (SelectedItem != null)
                {
                    SelectedItem.title = title;
                    SelectedItem.Categories = SelectedCategories;
                    SelectedItem.Publisher = SelectedPublisher;
                    SelectedItem.manufacture_date = manufacture_date;
                    SelectedItem.added_date = added_date;
                    SelectedItem.price = price;
                    OnPropertyChanged("SelectedItem");
                }
                BookSearchKeyword = null;
            });

            AddBookCommand = new AppCommand<object>((p) => 
            { 
                return true; 
            }, (p) => 
            { 
                AddBookWindow wd = new AddBookWindow(); 
                title = "";
                SelectedCategories = null;
                SelectedPublisher = null;
                SelectedAuthor = null;
                ListAuthors = new ObservableCollection<Author>();
                SelectedCategories = new ObservableCollection<Category>();
                added_date = DateTime.Now;
                manufacture_date = new DateTime(2000, 1, 1);
                price = 0;
                wd.ShowDialog();

            });

            CancelAddCommand = new AppCommand<object>(
                p => true,
                p =>
                {
                    if (SelectedItem != null)
                    {
                        title = SelectedItem.title;
                        SelectedPublisher = SelectedItem.Publisher;
                        SelectedCategories = new ObservableCollection<Category>(SelectedItem.Categories);
                        added_date = SelectedItem.added_date.Value;
                        manufacture_date = SelectedItem.manufacture_date.Value;
                        price = SelectedItem.price.Value;
                        ListAuthors = new ObservableCollection<Author>(SelectedItem.Authors);
                    }
                    InitBooks();
                });

            AddBookToDBCommand = new AppCommand<object>((p) =>
            {
                //Kiểm tra điều kiện
                if (string.IsNullOrEmpty(title))
                    return false;
                if (price <= 0 || SelectedCategories == null || SelectedPublisher == null || ListAuthors.Count == 0)
                    return false;
                return true;
            }, (p) =>
            {
                var book = new Book()
                {
                    title = title,
                    manufacture_date = manufacture_date,
                    added_date = added_date,
                    price = price,
                    publisher_id = SelectedPublisher.id,
                    status = "có sẵn"
                };
                
                var authors = new List<Author>();
                foreach (var author in ListAuthors)
                {
                    authors.Add(author);
                }
                var categories = new List<Category>();
                foreach (var category in SelectedCategories)
                {
                    categories.Add(category);
                }
                book.Authors = authors;
                book.Categories = categories;
                
                // save changes
                DataSingleton.Instance.DB.Books.Add(book);
                DataSingleton.Instance.DB.SaveChanges();

                SourceImageFile = null;
                List.MoveToLastPage();
                SetSelectedItemToFirstItemOfPage(false);
                GetLastestBooks();
                MessageBox.Show("Thêm sách thành công!");
            });

            //Edit Book Information
            EditBookToDBCommand = new AppCommand<object>((p) =>
            {
                //Kiểm tra điều kiện
                if (string.IsNullOrEmpty(title) || SelectedItem == null)
                    return false;
                if (price <= 0 || SelectedCategories == null || SelectedPublisher == null || ListAuthors.Count == 0)
                    return false;

                return true;
            }, (p) =>
            {
                var book = DataSingleton.Instance.DB.Books.Where(x => x.id == SelectedItem.id).SingleOrDefault();
                book.title = SelectedItem.title;
                book.manufacture_date = SelectedItem.manufacture_date;
                book.added_date = SelectedItem.added_date;
                book.price = SelectedItem.price;
                book.publisher_id = SelectedItem.publisher_id;
                // Save authors
                book.Authors.Clear();
                foreach (var author in ListAuthors)
                {
                    book.Authors.Add(author);
                }
                // Save categories
                book.Categories.Clear();
                foreach (var category in SelectedCategories)
                {
                    book.Categories.Add(category);
                }
               
                DataSingleton.Instance.DB.SaveChanges();
                SourceImageFile = null;
                List.Refresh();
                OnPropertyChanged("SelectedItem");
                InitBooks();
                MessageBox.Show("Sửa thông tin sách thành công");
                
            });

            //Delete Book
            DeleteBookCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;
            }, (p) =>
            {
                var book = DataSingleton.Instance.DB.Books.Where(x => x.id == SelectedItem.id)
                    .SingleOrDefault();
                if (book == null)
                {
                    MessageBox.Show("Không tìm thấy sách cần xóa");
                    return;
                }

                // Check if book is being borrowed? if it is, do not delete it.
                var isBeingBorrowed = DataSingleton.Instance.DB
                    .BookReaders.Any(el => el.book_id == book.id && el.return_date == null);
                if (isBeingBorrowed)
                {
                    MessageBox.Show("Không thể xóa sách đang được mượn");
                    return;
                }
                
                var bookReaders = DataSingleton.Instance.DB.BookReaders.Where(el => el.book_id == book.id);
                DataSingleton.Instance.DB.BookReaders.RemoveRange(bookReaders);
                DataSingleton.Instance.DB.Books.Remove(book);

                // save changes to DB
                try
                {
                    DataSingleton.Instance.DB.SaveChanges();
                    MessageBox.Show("Xóa sách thành công");
                }
                catch (Exception)
                {
                    MessageBox.Show("Đã có lỗi xảy ra, không thể thực hiện thao tác xóa sách");
                }
                finally
                {
                    List.Refresh();
                    SetSelectedItemToFirstItemOfPage(true);
                    GetLastestBooks();
                }
                
            });

            //Command Add Authors
            AddAuthors = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (!ListAuthors.Contains(SelectedAuthor))
                {
                    ListAuthors.Add(SelectedAuthor);
                }
            });

            //Delete Author in List Author
            UnSelectedAuthor = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                Author removeAuthor = p as Author;
                ListAuthors.Remove(removeAuthor);
            });
            
            // Add Category
            AddCategoryCommand = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (!SelectedCategories.Contains(SelectedCategory))
                {
                    SelectedCategories.Add(SelectedCategory);
                }
            });
            
            // Delete Category
            UnSelectedCategoryCommand = new AppCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                Category removeCategory = p as Category;
                SelectedCategories.Remove(removeCategory);
            });

            MoveToPreviousBooksPage = new AppCommand<object>(
                p =>
                {
                    return this.List.CurrentPage > 1;
                },
                p =>
                {
                    List.MoveToPreviousPage();
                    SetSelectedItemToFirstItemOfPage(true);
                });
            MoveToNextBooksPage = new AppCommand<object>(
                p =>
                {
                    return this.List.CurrentPage < this.List.PageCount;
                },
                p =>
                {
                    List.MoveToNextPage();
                    SetSelectedItemToFirstItemOfPage(true);
                });

            PrevBooks = new AppCommand<object>(
                p =>
                {
                    return ListLatestBooks.CurrentPage > 1;
                },
                p =>
                {
                    ListLatestBooks.MoveToPreviousPage();
                    OnPropertyChanged("ListLatestBooks");
                });
            NextBooks = new AppCommand<object>(
                p =>
                {
                    return ListLatestBooks.CurrentPage < List.PageCount;
                },
                p =>
                {
                    ListLatestBooks.MoveToNextPage();
                    OnPropertyChanged("ListLatestBooks");
                });
        }


        private void InitBooks(string keyword = null)
        {
            if (keyword != null)
            {
                List = new BookPaginatingCollection(30, keyword);
            }
            else
            {
                List = new BookPaginatingCollection(30);
            }
            SetSelectedItemToFirstItemOfPage(true);
        }
        private void SetSelectedItemToFirstItemOfPage(bool isFirstItem)
        {
            if (List.Books == null || List.Books.Count == 0)
            {
               return;
            }
            if (isFirstItem)
            {
                SelectedItem = List.Books.FirstOrDefault();
            }
            else
            {
                SelectedItem = List.Books.LastOrDefault();
            }
        }

        private void GetLastestBooks()
        {
            ListLatestBooks = new LatestBookPaginationCollection(9, 18);
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show("Exception Occured while releasing object " + ex.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}