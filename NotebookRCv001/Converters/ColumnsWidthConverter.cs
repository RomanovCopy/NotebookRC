using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Converters
{
    public class ColumnsWidthConverter : IValueConverter
    {
        /// <summary>
        /// родительское окно
        /// </summary>
        internal Window window { get; set; }
        /// <summary>
        /// сумма размеров всех колонок(служит для вычисления размера последней колонки)
        /// </summary>
        double sum = 0;

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if (window == null) return null;
            try
            {
                if (parameter is string str && str == "last")
                {// последний размер формируется по остаточному принципу
                    double width = window.Width - sum;
                    sum = 0;
                    return width;
                }
                else
                {//преобразуем проценты в значение размера и суммируем с предыдущими размерами
                    double width = window.Width / 100 * (double)value;
                    sum += width;
                    return width;
                }
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            try
            {
                if (window == null) return null;
                //преобразуем значения в проценты
                return (double)value / window.Width * 100;
                //если обратное преобразование не требуется, возвращает пустое значение для свойста
                //return DependencyProperty.UnsetValu
            }
            catch (Exception e) { ErrorWindow( e ); return null; }
        }

        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new Thread( () => MessageBox.Show( e.Message, $"ColumnsWidth_Converter.{name}" ) );
            thread.SetApartmentState( ApartmentState.STA );
            thread.Start();
        }
    }
}
