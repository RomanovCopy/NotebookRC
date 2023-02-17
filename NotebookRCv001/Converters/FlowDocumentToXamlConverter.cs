using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace NotebookRCv001.Converters
{
    [ValueConversion(typeof(string), typeof(FlowDocument))]
    public class FlowDocumentToXamlConverter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// Преобразует разметку XAML в WPF FlowDocument.
        /// </summary>
        public object Convert( object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var flowDocument = new FlowDocument();
            if (value != null)
            {
                var xamlText = (string)value;
                flowDocument = (FlowDocument)XamlReader.Parse(xamlText);
            }
            return flowDocument;
        }

        /// <summary>
        /// Преобразует из WPF FlowDocument в строку разметки XAML.
        /// </summary>
        public object ConvertBack( object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            //Выйти, если FlowDocument имеет значение null
            if (value == null)
                return string.Empty;

            // Получить потоковый документ из переданного значения
            var flowDocument = (FlowDocument)value;

            // Преобразование в XAML и возврат
            return XamlWriter.Save(flowDocument);
        }

        #endregion
    }
}
