using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace NotebookRCv001.Infrastructure
{
    public class RTB_DependencyProperties : DependencyObject
    {
        public static readonly DependencyProperty CaretPositionProperty;

        public int CaretPosition
        {
            get { return (int)GetValue(CaretPositionProperty); }
            set { SetValue(CaretPositionProperty, value); }
        }

        static RTB_DependencyProperties()
        {
            //CaretPositionProperty = DependencyProperty.Register("CaretPosition", typeof(TextPointer), typeof(RTB_DependencyProperties));
            CaretPositionProperty = DependencyProperty.Register("CaretPosition",
                typeof(int),
                typeof(Views.FlowDocumentEditor),
            new FrameworkPropertyMetadata(
            new PropertyChangedCallback(CaretPositionChanged)//реакция на изменение входящего значения
            /*, new CoerceValueCallback(CoerceCretPosition)*/)); //корректировка входящего значения
        }

        //private static object CoerceCretPosition( DependencyObject d, object baseValue )
        //{

        //}

        private static void CaretPositionChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            try
            {

            }
            catch (Exception ex) { ErrorWindow(ex); }
        }


        private static void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"FlowDocumentEditorModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
