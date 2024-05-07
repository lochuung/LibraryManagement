using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagement.Models;
using LibraryManagement.Utils.Paginations;

namespace LibraryManagement.ViewModels
{
    public class ReturnBookViewModel : BaseViewModel
    { private string readerSearchKeyword;
        private ReaderPaginatingCollection listReader;
        private Reader readerSelected;
        private ObservableCollection<BookReader> listDetailBorrowCorresponding;
        private ObservableCollection<BookReader> listDetailBorrowSelected;
        private BookReader billReturn;
        private DateTime dateReturn;

        public string ReaderSearchKeyword
        {
            get => readerSearchKeyword;
            set
            {
                readerSearchKeyword = value;
                OnPropertyChanged();
                InitReaders(readerSearchKeyword);
            }
        }
        public ReaderPaginatingCollection ListReader {
            get => listReader;
            set { listReader = value; OnPropertyChanged(); }
        }
        public Reader ReaderSelected {
            get => readerSelected;
            set {
                readerSelected = value;
                OnPropertyChanged();
                RetrieveDetailBorrow();
                if (readerSelected != null)
                {
                    BookReader.id = readerSelected.id;
                    OnPropertyChanged("BookReader");
                }
            }
        }
        public ObservableCollection<BookReader> ListDetailBorrowCorresponding {
            get => listDetailBorrowCorresponding;
            set { listDetailBorrowCorresponding = value; OnPropertyChanged(); }
        }
        public ObservableCollection<BookReader> ListDetailBorrowSelected {
            get => listDetailBorrowSelected;
            set { listDetailBorrowSelected = value; OnPropertyChanged(); }
        }
        public BookReader BookReader {
            get => billReturn;
            set
            {
                billReturn = value; OnPropertyChanged();
            }
        }
        public DateTime DateReturn { get => dateReturn; set { dateReturn = value; OnPropertyChanged(); } }

        public ICommand SelectBook { get; set; }
        public ICommand UnSelectBook { get; set; }
        public ICommand ReturnBook { get; set; }
        public ICommand MoveToPreviousReadersPage { get; set; }
        public ICommand MoveToNextReadersPage { get; set; }




        /// <summary>
        /// Constructor
        /// </summary>

        public ReturnBookViewModel()
        {
            DefineCommands();
            RetrieveDataAndClearInput();
        }

        /// <summary>
        /// Definition for commands
        /// </summary>
        private void DefineCommands()
        {
            SelectBook = new AppCommand<object>(
                p =>
                {
                    return true;
                },
                p =>
                {
                    BookReader detailSelected = p as BookReader;
                    ListDetailBorrowCorresponding.Remove(detailSelected);
                    ListDetailBorrowSelected.Add(detailSelected);
                });
            UnSelectBook = new AppCommand<object>(
                p =>
                {
                    return true;
                },
                p =>
                {
                    BookReader detailSelected = p as BookReader;
                    ListDetailBorrowSelected.Remove(detailSelected);
                    ListDetailBorrowCorresponding.Add(detailSelected);
                });
            ReturnBook = new AppCommand<object>(
                p =>
                {
                    if (ListDetailBorrowSelected == null || ListDetailBorrowSelected.Count == 0)
                    {
                        return false;
                    }
                    return true;
                },
                p =>
                {
                    BookReader.return_date = DateReturn;
                    DataSingleton.Instance.DB.BookReaders.Add(BookReader);
                    try
                    {
                        foreach (var detailBorrow in ListDetailBorrowSelected)
                        {
                            detailBorrow.return_date = DateReturn;
                            detailBorrow.status = "đã trả";
                            detailBorrow.Book.status = "có sẵn";
                        }
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
                        // Change Application state to intialize state
                        RetrieveDataAndClearInput();
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
                        // Change Application state to intialize state
                        RetrieveDataAndClearInput();
                        MessageBox.Show("Trả sách thành công!");
                    }
                });
            MoveToPreviousReadersPage = new AppCommand<object>(
               p =>
               {
                   return ListReader.CurrentPage > 1;
               },
               p =>
               {
                   ListReader.MoveToPreviousPage();
               });
            MoveToNextReadersPage = new AppCommand<object>(
                p =>
                {
                    return ListReader.CurrentPage < ListReader.PageCount;
                },
                p =>
                {
                    ListReader.MoveToNextPage();
                });
        }
        /// <summary>
        /// Retrieve default data from DB and initialize properties
        /// </summary>
        private void RetrieveDataAndClearInput()
        {

            BookReader = new BookReader();
            DateReturn = DateTime.Now;
            InitReaders();
            ReaderSearchKeyword = null;
            ListDetailBorrowSelected = new ObservableCollection<BookReader>();
            RetrieveDetailBorrow();
        }

        /// <summary>
        /// Retrieve detail borrow data with corresponding reader
        /// </summary>
        private void RetrieveDetailBorrow()
        {
            if (ReaderSelected != null)
            {
                try
                {
                    ListDetailBorrowCorresponding = new ObservableCollection<BookReader>(ReaderSelected.BookReaders
                        .Where(br => br.status == "đang mượn"));
                }
                catch(ArgumentNullException)
                {
                    MessageBox.Show("Đã có lỗi xảy ra khi đọc dữ liệu!");
                    RetrieveDataAndClearInput();
                }
               
            }
            else
            {
                ListDetailBorrowCorresponding = new ObservableCollection<BookReader>();
            }
            ListDetailBorrowSelected = new ObservableCollection<BookReader>();
        }


        // Init data for pagination
        private void InitReaders(string keyword = null)
        {
            if (keyword != null)
            {
                ListReader = new ReaderPaginatingCollection(10, keyword);
            }
            else
            {
                ListReader = new ReaderPaginatingCollection(10);
            }
            SetSelectedItemToFirstItemOfPage(true);
        }

        private void SetSelectedItemToFirstItemOfPage(bool isFirstItem)
        {
            if (ListReader.Readers == null || ListReader.Readers.Count == 0)
            {
                return;
            }
            if (isFirstItem)
            {
                ReaderSelected = ListReader.Readers.FirstOrDefault();
            }
            else
            {
                ReaderSelected = ListReader.Readers.LastOrDefault();
            }
        }
    }
}