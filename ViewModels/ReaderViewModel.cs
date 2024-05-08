using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using LibraryManagement.Models;
using LibraryManagement.Utils.Paginations;

namespace LibraryManagement.ViewModels
{
    public class ReaderViewModel : BaseViewModel
    {
        private ReaderPaginatingCollection _List;
        public ReaderPaginatingCollection List { get => _List; set { _List = value; OnPropertyChanged(); } }

        private ObservableCollection<ReaderType> _ReaderType;
        public ObservableCollection<ReaderType> ReaderType { get => _ReaderType; set { _ReaderType = value; OnPropertyChanged(); } }
        private Reader _SelectedItem;
        public Reader SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged();
                if (SelectedItem != null)
                {
                    IdReader = SelectedItem.id;
                    NameReader = SelectedItem.name;
                    DobReader = SelectedItem.dob;
                    Email = SelectedItem.email;
                    CreatedAt = SelectedItem.created_date;
                    IdReaderType = SelectedItem.reader_type_id.Value;
                    SelectedReaderType = SelectedItem.ReaderType;
                }
            }
        }
        private ReaderType _SelectedReaderType;
        public ReaderType SelectedReaderType { get => _SelectedReaderType; set { _SelectedReaderType = value; OnPropertyChanged(); } }

        private long _id;
        public long IdReader { get => _id; set { _id = value; OnPropertyChanged(); } }

        private string _name;
        public string NameReader {get => _name; set { _name = value; OnPropertyChanged(); } }

        private string _email;
        public string Email { get => _email; set {_email = value;OnPropertyChanged();} }


        private string _address;
        public string AddressReader { get => _address; set { _address = value; OnPropertyChanged(); }}

        private long _reader_type_id;
        public long IdReaderType { get => _reader_type_id; set { _reader_type_id = value; OnPropertyChanged(); } }

        private DateTime? _created_date ;
        private DateTime? _dob;
        private DateTime? _latestExtended;

        public DateTime? CreatedAt {
            get => _created_date;
            set
            {   
                _created_date = value; OnPropertyChanged(); } }
        public DateTime? DobReader { get => _dob; set {  _dob = value; OnPropertyChanged(); } 
        }
        public DateTime? LatestExtended
        {
            get => _latestExtended;
            set
            {
                _latestExtended = value; OnPropertyChanged();
            }
        }
        private int _debt;
        public int Debt{ get => _debt; set { _debt = value; OnPropertyChanged();}}

        //Search Reader
        private string readerSearchKeyword;
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


        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand PrepareAddReaderCommand { get; set; }
        public ICommand CheckCommand { get; set;}
        public ICommand AddReaderTypeCommand { get; set;}
        public ICommand DeleteReaderTypeCommand { get; set;}
        public ICommand CancelCommand { get; set;}
        public ICommand CancelAddCommand { get; set; }
        public ICommand ReloadReaderTypeCommand { get; set;}
        public ICommand MoveToPreviousReadersPage { get; set; }
        public ICommand MoveToNextReadersPage { get; set; }
        public ICommand ExtendReaderCard { get; set; }

        public ReaderViewModel()
        {
            // Retrieve data from DB
            InitReaders();
           
            CancelCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedReaderType == null)
                    return false;
                return true;

            }, (p) =>
            {
                SelectedItem.name = NameReader;
                SelectedItem.dob = (DateTime)DobReader;
                SelectedItem.email = Email;
                SelectedItem.created_date = (DateTime)CreatedAt;
                SelectedItem.reader_type_id = IdReaderType;
                OnPropertyChanged("SelectedItem");
                ReaderSearchKeyword = null;
                InitReaders();
            });
            ReloadReaderTypeCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedReaderType == null)
                    return false;
                return true;

            }, (p) =>
            {
                ReaderType = new ObservableCollection<ReaderType>(DataSingleton.Instance.DB.ReaderTypes);
            });
            AddCommand = new AppCommand<object>((p) =>
            {
                if (NameReader == null || Email == null)
                    return false;
                return true;

            }, (p) =>
            {
                try
                {
                    var Reader = new Reader()
                    {
                        name = NameReader,
                        dob = (DateTime)DobReader,
                        email = Email,
                        created_date = DateTime.Today,
                        debt = 0,
                        reader_type_id = SelectedReaderType.id
                    };
                    DataSingleton.Instance.DB.Readers.Add(Reader);
                    DataSingleton.Instance.DB.SaveChanges();
                    List.MoveToLastPage();
                    SetSelectedItemToFirstItemOfPage(false);
                    MessageBox.Show("Bạn đã thêm người dùng thành công");
                    InitReaders();
                }
                catch(Exception)
                {
                    MessageBox.Show("Đã có lỗi xảy ra!");
                }
            });

            EditCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;

            }, (p) =>
            {
                var Reader = DataSingleton.Instance.DB.Readers.Where(x => x.id == SelectedItem.id).SingleOrDefault();
                Reader.name = SelectedItem.name;
                Reader.dob = (DateTime)SelectedItem.dob;
                Reader.email = SelectedItem.email;
                Reader.created_date = (DateTime)SelectedItem.created_date;
                Reader.reader_type_id = SelectedItem.ReaderType.id;
                DataSingleton.Instance.DB.SaveChanges();
                List.Refresh();
                OnPropertyChanged("SelectedItem");
                InitReaders();
                MessageBox.Show("Sửa thông tin độc giả thành công");
            });
            DeleteCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null)
                    return false;
                return true;

            }, (p) =>
            {
                var reader = DataSingleton.Instance.DB.Readers.Where(x => x.id == SelectedItem.id).SingleOrDefault();
                
                if (reader == null)
                {
                    MessageBox.Show("Độc giả không tồn tại");
                    return;
                }

                var bookReader = DataSingleton.Instance.DB.BookReaders
                    .Where(e => e.reader_id == reader.id && e.status == "đang mượn");
                
                if (bookReader.Count() > 0)
                {
                    MessageBox.Show("Không thể xóa độc giả đang mượn sách");
                    return;
                }
                
                var bookReaderHistory = DataSingleton.Instance.DB.BookReaders
                    .Where(e => e.reader_id == reader.id);
                DataSingleton.Instance.DB.BookReaders.RemoveRange(bookReaderHistory);
                
                var payments = DataSingleton.Instance.DB.Payments
                    .Where(e => e.reader_id == reader.id);

                var collected = DataSingleton.Instance.DB.Payments.Where(el => el.id == reader.id);
                DataSingleton.Instance.DB.Payments.RemoveRange(collected);

                DataSingleton.Instance.DB.Readers.Remove(reader);
                try
                {
                    DataSingleton.Instance.DB.SaveChanges();
                    MessageBox.Show("Xóa độc giả thành công");
                }
                catch(Exception)
                {
                    MessageBox.Show("Đã có lỗi xảy ra, không thể thực hiện thao tác xóa độc giả");
                }
                finally
                {
                    List.Refresh();
                    SetSelectedItemToFirstItemOfPage(true);
                }
            });
            PrepareAddReaderCommand = new AppCommand<object>(
                p => true,
                p =>
                {
                    NameReader = null;
                    AddressReader = null;
                    Email = null;
                    DobReader = new DateTime(2000, 1, 1);
                    CreatedAt = DateTime.Now;
                    Debt = 0;
                    SelectedReaderType = ReaderType.FirstOrDefault();
                });
            CheckCommand = new AppCommand<object>((p) =>
            {
                if (SelectedItem == null || SelectedReaderType == null)
                    return false;
                return true;

            }, (p) =>
            {
               
            });
            MoveToPreviousReadersPage = new AppCommand<object>(
               p =>
               {
                   return List.CurrentPage > 1;
               },
               p =>
               {
                   List.MoveToPreviousPage();
                   SetSelectedItemToFirstItemOfPage(true);
               });
            MoveToNextReadersPage = new AppCommand<object>(
                p =>
                {
                    return List.CurrentPage < List.PageCount;
                },
                p =>
                {
                    List.MoveToNextPage();
                    SetSelectedItemToFirstItemOfPage(true);
                });
            ExtendReaderCard = new AppCommand<object>(
                p => SelectedItem != null,
                p =>
                {
                    DataSingleton.Instance.DB.SaveChanges();
                    OnPropertyChanged("SelectedItem");
                    List.Refresh();
                    MessageBox.Show("Gia hạn độc giả thành công!");
                });
            CancelAddCommand = new AppCommand<object>(
                p => true,
                p =>
                {
                    if (SelectedItem != null)
                    {
                        IdReader = SelectedItem.id;
                        NameReader = SelectedItem.name;
                        DobReader = SelectedItem.dob;
                        Email = SelectedItem.email;
                        CreatedAt = SelectedItem.created_date;
                        IdReaderType = SelectedItem.reader_type_id.Value;
                        SelectedReaderType = SelectedItem.ReaderType;
                        SelectedItem = SelectedItem;
                    }
                });
        }

        private void InitReaders(string keyword = null)
        {
            ReaderType = new ObservableCollection<ReaderType>(DataSingleton.Instance.DB.ReaderTypes);
            if (keyword != null)
            {
                List = new ReaderPaginatingCollection(30, keyword);
            }
            else
            {
                List = new ReaderPaginatingCollection(30);
            }
            SetSelectedItemToFirstItemOfPage(true);
        }
        private void SetSelectedItemToFirstItemOfPage(bool isFirstItem)
        {
            if (List.Readers == null || List.Readers.Count == 0)
            {
                return;
            }
            if (isFirstItem)
            {
                SelectedItem = List.Readers.FirstOrDefault();
            }
            else
            {
                SelectedItem = List.Readers.LastOrDefault();
            }
        }
    }
}