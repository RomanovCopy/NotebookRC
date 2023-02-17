using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Converters
{
    public class EncodingConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                object result = null;
                if (value is byte[] bytes && targetType == typeof(string) && parameter is HomeViewModel view)
                {
                        //result = view.HomeEncoding.GetString(bytes);
                }
                return result;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                object result = null;
                if (value is string str && targetType == typeof(byte[]) && parameter is HomeViewModel view)
                {
                        //result = view.HomeEncoding.GetBytes(str);
                }
                return result;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
    }
}
