using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace NotebookRCv001.Converters
{
    [ValueConversion(typeof(string), typeof(FlowDocument))]
    public class FlowDocumentConverter : IValueConverter
    {
        #region IValueConverter Members

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

        public object ConvertBack( object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if (value == null)
                return string.Empty;
            var flowDocument = (FlowDocument)value;

            return XamlWriter.Save(flowDocument);
        }

        #endregion
    }
}
