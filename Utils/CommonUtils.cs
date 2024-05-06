using System.Windows;
using System.Windows.Media.Imaging;

namespace LibraryManagement.Utils
{
    public class CommonUtils
    {
        public static BitmapSource ConvertHBitmapSource(System.Drawing.Bitmap bitmap)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                System.IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}