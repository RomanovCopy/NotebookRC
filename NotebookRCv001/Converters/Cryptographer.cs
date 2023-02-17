using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Security;
using System.Security.Cryptography;

namespace NotebookRCv001.Converters
{
    public class Cryptographer: IValueConverter
    {
        public Cryptographer()
        {

        }

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                object rezult = null;
                return rezult;
            }catch (Exception ex) { ErrorWindow(ex); return null; }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                object rezult = null;




                return rezult;
            }
            catch (Exception ex) { ErrorWindow(ex); return null; }
        }

        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"Cryptographer.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
