using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

using Controls = System.Windows.Controls;
using Drawing = System.Drawing;

namespace NotebookRCv001.Converters
{
    internal class BitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                object rezult = null;
                if (value is Drawing.Bitmap bitmap)
                {
                    rezult = GetImageFromBitmap(bitmap);
                }
                return rezult;
            }
            catch (Exception ex) { ErrorWindow(ex); return null; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                object rezult = null;
                if (value is Controls.Image image)
                {

                }
                return rezult;
            }
            catch (Exception ex) { ErrorWindow(ex); return null; }
        }

        private Controls.Image GetImageFromBitmap(Drawing.Bitmap bitmap)
        {
            try
            {
                Controls.Image image = new();
                BitmapImage bitmapImage = null;
                using (MemoryStream ms = new())
                {
                    bitmap.Save(ms, Drawing.Imaging.ImageFormat.Jpeg);
                    ms.Seek(0, SeekOrigin.Begin);
                    bitmapImage = new();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = ms;
                    bitmapImage.DecodePixelWidth = (int)bitmap.Width;
                    bitmapImage.DecodePixelHeight = (int)bitmap.Height;
                    bitmapImage.EndInit();
                    image.Source = bitmapImage;
                }
                return image;
            }
            catch (Exception e) { ErrorWindow(e); return null; }
        }

        private void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"BitmapConverter.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
