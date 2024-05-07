using LibraryManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using LibraryManagement.Utils.Paginations;

namespace LibraryManagement.ViewModels
{

    /// <summary>
    /// Convert idpermisson for button Regulation UI control 
    /// </summary>
    class PermissonToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return false;
                int idPermission = Int32.Parse(value.ToString());
                if (idPermission == 1)
                {
                    return "Visible";
                }
                else
                {
                    return "Hidden";
                }
            }
            catch (Exception)
            {
                return "Hidden";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Convert idpermisson for textbox Regulation UI control 
    /// </summary>
    class PermissonToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return false;
                int idPermission = Int32.Parse(value.ToString());
                if (idPermission == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
    }

    /// <summary>
    /// Convert List to boolean (for paginating buttons' visibility)
    /// </summary>
    class ListToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return false;
                LatestBookPaginationCollection list = (LatestBookPaginationCollection)value;
                int button = Int32.Parse(paramater.ToString());
                if (button == 1)
                {
                    return list.CurrentPage > 1 ? "Visible" : "Hidden";
                }
                else
                {
                    return list.CurrentPage < list.PageCount ? "Visible" : "Hidden";
                }
            }
            catch (Exception)
            {
                return "Hidden";
            }
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
    }

    /// <summary>
    /// Convert selected item to boolean (for enable/disable context menu item)
    /// </summary>
    class SelectedItemToBoolean : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return false;
            return true;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }
    }

    /// <summary>
    /// Convert the bill return to total amount of fine of the reader
    /// </summary>
    class ImageToSource : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return 0;
            string fileName = (string)value;

            string destPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string destinationFile = Path.Combine($"{destPath}\\BookImageCover", fileName);
            return destinationFile;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Convert the date a book was borrowed to the number of days borrowed. 
    /// </summary>
    class DateBorrowedToDaysBorrowed : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return value;
            int noDaysBorrowes = DateTime.Now.Subtract((DateTime)value).Days;
            return noDaysBorrowes;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Concatenate authors collection into a string to display in a cell: 
    /// </summary>
    class AuthorsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return value;
            HashSet<Author> Authors = null;
            if (value is List<Author>)
            {
                Authors = new HashSet<Author>((List<Author>)value);
            }
            else
            {
                Authors = new HashSet<Author>((HashSet<Author>)value);
            }
            List<Author> list = Authors.ToList();
            string result = "";
            foreach (var item in list)
            {
                if (item != list.First())
                {
                    result += ", ";
                }

                result += item.name;
            }

            return result;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

    }

    /// <summary>
    /// Concatenate categories collection into a string to display in a cell: 
    /// </summary>

    class CategoriesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
        {
            if (value == null)
                return value;
            HashSet<Category> Categories = null;
            if (value is List<Category>)
            {
                Categories = new HashSet<Category>((List<Category>)value);
            } else if (value is ObservableCollection<Category>)
            {
                Categories = new HashSet<Category>((ObservableCollection<Category>)value);
            }
            else 
            {
                Categories = new HashSet<Category>((HashSet<Category>)value);
            }
            List<Category> list = Categories.ToList();
            string result = "";
            foreach (var item in list)
            {
                if (item != list.First())
                {
                    result += ", ";
                }

                result += item.name;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
        }

        /// <summary>
        /// Convert Boolean value: true/false to Visibility value: Visible/Hidden
        /// </summary>
        class BooleanToVisibilityConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object paramater, CultureInfo culture)
            {
                if (value == null || (bool)value == true)
                    return "Visible";
                return "Hidden";
            }


            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotSupportedException("MethodToValueConverter can only be used for one way conversion.");
            }
        }
    }
}