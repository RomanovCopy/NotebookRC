using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NotebookRCv001.Converters
{
    public class FontPropertiesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                object result = DependencyProperty.UnsetValue;
                if (value is PropertyInfo propertyInfo)
                {
                    result = propertyInfo.GetValue(null);
                }
                return result;
            }
            catch (Exception e)
            {
                ErrorWindow(e);
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                object result = null;
                result = DependencyProperty.UnsetValue;
                return result;
            }
            catch (Exception e)
            {
                ErrorWindow(e);
                return DependencyProperty.UnsetValue;
            }
        }

        private void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new Thread(() => MessageBox.Show(e.Message, $"FontProperties_Converter.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
