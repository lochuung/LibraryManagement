using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LibraryManagement.Utils.Validations.Book
{
    class BookNameValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string name = (string)value;
            if (value == null || name.Trim().Length == 0)
            {
                return new ValidationResult(false, "Vui lòng nhập tên sách");
            }
            return ValidationResult.ValidResult;
        }
    }
}
