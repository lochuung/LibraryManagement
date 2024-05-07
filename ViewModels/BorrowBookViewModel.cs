using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagement.Models;
using LibraryManagement.Utils.Paginations;

namespace LibraryManagement.ViewModels
{
    public class BorrowBookViewModel : BaseViewModel
    {private BookPaginatingCollection books;
        private ObservableCollection<Book> listBooksSelected;
        private ReaderPaginatingCollection readers;
        private Reader readerSelected;
        private string bookKeyword;
        private string readerKeyword;
        public ObservableCollection<Book> ListBooksSelected { 
            get => listBooksSelected; 
            set { 
                listBooksSelected = value; 
                OnPropertyChanged(); 
            } }
        public BookPaginatingCollection Books
        {
            get => books;
            set
            {
                books = value;
                OnPropertyChanged();
            }
        }
        public ReaderPaginatingCollection Readers { 
            get => readers; 
            set { 
                readers = value;
                OnPropertyChanged();
            } 
        }
        public Reader ReaderSelected {
            get => readerSelected; 
            set {
                readerSelected = value; 
                OnPropertyChanged();
            }
        }
        public string BookKeyword { get => bookKeyword; set { bookKeyword = value; OnPropertyChanged(); InitBooks(bookKeyword); } }
        public string ReaderKeyword { get => readerKeyword; set { readerKeyword = value; OnPropertyChanged(); InitReaders(readerKeyword); } }

        public ICommand BorrowCommand { get; set; }
        public ICommand SelectBook { get; set; }
        public ICommand UnselectBook { get; set; }
        public ICommand UnselectAllBooks { get; set; }
        public ICommand MoveToPreviousBooksPage { get; set; }
        public ICommand MoveToNextBooksPage { get; set; }
        public ICommand MoveToPreviousReadersPage { get; set; }
        public ICommand MoveToNextReadersPage { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public BorrowBookViewModel()
        {
            RetrieveDataAndClearInput();
            DefineCommands();
        }

        /// <summary>
        /// Get default data from DB and init some properties
        /// </summary>
        private void RetrieveDataAndClearInput()
        {
            if (ListBooksSelected == null)
            {
                ListBooksSelected = new ObservableCollection<Book>();
            }
            else
            {
                ListBooksSelected.Clear();
            }

            InitBooks();
            InitReaders();
        }
        /// <summary>
        /// Definitions for commands
        /// </summary>
        private void DefineCommands()
        {
            BorrowCommand = new AppCommand<object>(
                p =>
                {
                    if (ReaderSelected == null || ListBooksSelected.Count == 0)
                    {
                        return false;
                    }
                    return true;
                },
                p =>
                {
                    // Add borrow record to DB
                    foreach (var book in ListBooksSelected)
                    {
                        
                        var borrow = new BookReader() { borrow_date = DateTime.Now, reader_id = ReaderSelected.id };
                        borrow.book_id = book.id;
                        borrow.status = "đang mượn";
                        book.status = "đã mượn";
                        DataSingleton.Instance.DB.BookReaders.Add(borrow);
                    }

                    try
                    {
                        DataSingleton.Instance.DB.SaveChanges();
                    }
                    catch (DbUpdateException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    catch (DbEntityValidationException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    catch (NotSupportedException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    catch (ObjectDisposedException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    catch (InvalidOperationException)
                    {
                        MessageBox.Show("Không thể thao tác vì lỗi cơ sở dữ liệu!");
                    }
                    finally
                    {
 
                        RetrieveDataAndClearInput();
                        MessageBox.Show("Mượn sách thành công!");
                    }
                });
            SelectBook = new AppCommand<object>(
                p =>
                {
                    return true;
                },
                p => {
                    if (ReaderSelected == null)
                    {
                        MessageBox.Show("Vui lòng chọn độc giả trước tiên!");
                        return;
                    }

                    Book bookSelected = p as Book;
                    if (ListBooksSelected.Contains(bookSelected))
                    {
                        MessageBox.Show("Sách này được chọn rồi!");
                        return;
                    }
                    else
                    {
                        if (CheckBookExpiryOfReader(ReaderSelected))
                        {
                            MessageBox.Show("Độc giả đang có sách quá hạn không thể mượn thêm sách!");
                            return;
                        }
                    }
                    
                    if (bookSelected.status == "có sẵn")
                    {
                        ListBooksSelected.Add(bookSelected);
                    }
                    else
                    {
                        MessageBox.Show("Sách đã được mượn rồi");
                    }
                });
            UnselectBook = new AppCommand<object>(
                p =>
                {
                    return true;
                },
                p =>
                {
                    Book bookSelected = p as Book;
                    ListBooksSelected.Remove(bookSelected);
                });
            UnselectAllBooks = new AppCommand<object>(
                p =>
                {
                    return ListBooksSelected?.Count > 0;
                },
                p =>
                {
                    ListBooksSelected = new ObservableCollection<Book>();
                });
            MoveToPreviousBooksPage = new AppCommand<object>(
                p =>
                {
                    return Books.CurrentPage > 1;
                },
                p =>
                {
                    Books.MoveToPreviousPage();
                });
            MoveToNextBooksPage = new AppCommand<object>(
                p =>
                {
                    return Books.CurrentPage < Books.PageCount;
                },
                p =>
                {
                    Books.MoveToNextPage();
                });
            MoveToPreviousReadersPage = new AppCommand<object>(
                p =>
                {
                    return Readers.CurrentPage > 1;
                },
                p =>
                {
                    Readers.MoveToPreviousPage();
                });
            MoveToNextReadersPage = new AppCommand<object>(
                p =>
                {
                    return Readers.CurrentPage < Readers.PageCount;
                },
                p =>
                {
                    Readers.MoveToNextPage();
                    
                });
        }

        private bool CheckBookExpiryOfReader(Reader reader)
        {
            int maxBorrowDays = 30;
            IQueryable<long> borrowExpired = from br in DataSingleton.Instance.DB.BookReaders
                                where br.reader_id == reader.id && br.status == "đang mượn" 
                                                                && DbFunctions.DiffDays(DateTime.Now, br.borrow_date)
                                                                    .Value > maxBorrowDays
                                select br.id;
            
            return borrowExpired.Any() ? true : false;
        }

        private void SetSelectedItemToFirstItemOfPage(bool isFirstItem)
        {
            if (Readers.Readers == null || Readers.Readers.Count == 0)
            {
                return;
            }
            if (isFirstItem)
            {
                ReaderSelected = Readers.Readers.FirstOrDefault();
            }
            else
            {
                ReaderSelected = Readers.Readers.LastOrDefault();
            }
        }

        private void InitReaders(string keyword = null)
        {
            if (keyword != null)
            {
                Readers = new ReaderPaginatingCollection(10, keyword);
            }
            else
            {
                Readers = new ReaderPaginatingCollection(10);
            }
            SetSelectedItemToFirstItemOfPage(true);
        }

        private void InitBooks(string keyword = null)
        {
            if (keyword != null)
            {
                Books = new BookPaginatingCollection(10, keyword);
            }
            else
            {
                Books = new BookPaginatingCollection(10);
            }
        }
    }
}