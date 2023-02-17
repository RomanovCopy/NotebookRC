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
using Media = System.Windows.Media;
using Drawing = System.Drawing;
using System.Reflection;
//using ParserDownloader_WPF_.PD_Page_ScriptEditor;

namespace NotebookRCv001.Converters
{
    class TextBoxPropertiesConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                object result = DependencyProperty.UnsetValue;
                if (targetType == null || value == null)
                    return result;
                if (targetType == typeof(Media.Brush))
                {
                    if (value.GetType() == typeof(Media.Color))
                        result = new Media.SolidColorBrush((Media.Color)value);
                    else if (value.GetType() == typeof(Drawing.Color))
                    {
                        var a = (Drawing.Color)value;
                        Media.Color color = Media.Color.FromArgb(a.A, a.R, a.G, a.B);
                        result = new Media.SolidColorBrush(color);
                    }
                }
                else if (targetType == typeof(Media.FontFamily))
                {
                    if (value.GetType() == typeof(string))
                        result = new Media.FontFamily(value.ToString());
                }
                else if (targetType == typeof(FontWeight))
                {
                    if (value.GetType() == typeof(string))
                    {
                        PropertyInfo propertyInfo = typeof(FontWeights).GetProperty(value.ToString());
                        result = propertyInfo != null ? propertyInfo.GetValue(null) : DependencyProperty.UnsetValue;
                    }
                }
                else if (targetType == typeof(FontStyle))
                {
                    if (value is string str)
                    {
                        var propertyInfo = typeof(FontStyles).GetProperty(str);
                        result = propertyInfo != null ? propertyInfo.GetValue(null) : DependencyProperty.UnsetValue;
                    }
                }
                else if (targetType == typeof(string) && parameter != null)
                {//преобразование массива байт в текст в соответствии с заданной кодировкой
                    //if (parameter is ScriptEditor_MenuSettings encoding && value is byte[] bytes)
                    //{
                    //    Encoding enc = Encoding.GetEncoding(encoding.CurrentCodePage);
                    //    result = enc != null ? enc.GetString(bytes) : "";
                    //}
                }
                return result;
            }
            catch (Exception e)
            {
                ErrorWindow(e);
                return DependencyProperty.UnsetValue;
            }
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                object result = DependencyProperty.UnsetValue;
                if (targetType == null || value == null)
                    return result;
                if (parameter != null)
                {
                    //if (targetType == typeof(byte[]) && value is string str && parameter is ScriptEditor_MenuSettings encoding)
                    //{//преобразование текста в массив байт в соответствии с заданной кодировкой
                    //    Encoding enc = Encoding.GetEncoding(encoding.CurrentCodePage);
                    //    result = enc != null ? enc.GetBytes(str) : null;
                    //}
                }
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
            Thread thread = new Thread(() => MessageBox.Show(e.Message, $"TextBoxPropertiesConverter.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
