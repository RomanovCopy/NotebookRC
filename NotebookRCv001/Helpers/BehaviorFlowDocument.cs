using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;

namespace NotebookRCv001.Helpers
{
    public class BehaviorFlowDocument : Behavior<FlowDocument>
    {
        MainWindowViewModel MainWindowViewModel { get; set; }
        RichTextBoxViewModel FlowDocumentEditorViewModel { get; set; }
        public FlowDocument Document => AssociatedObject;

        public TextRange TextRange 
        { 
            get => textRange;
            set=>textRange = value;
        }
        TextRange textRange;

        public void Bold(TextPointer start, TextPointer end)
        {
            TextRange = new TextRange(start, end);
            TextRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }

        #region *******************************"Loading and closing"****************************************


        public BehaviorFlowDocument()
        {
            MainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;

        }
        protected override void OnAttached()
        {
            try
            {
                AssociatedObject.Loaded += AssociatedObject_Loaded;
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        protected override void OnDetaching()
        {
            try
            {

            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }
        private void AssociatedObject_Loaded( object sender, RoutedEventArgs e )
        {
            FlowDocumentEditorViewModel = (RichTextBoxViewModel)Application.Current.MainWindow.
                FindResource("rsflowdocumenteditorviewmodel");
            //if (FlowDocumentEditorViewModel.FlowDocumentLoaded.CanExecute(this))
            //    FlowDocumentEditorViewModel.FlowDocumentLoaded.Execute(this);
        }

        #endregion

        #region*********************************** Error processing ****************************************  

        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            var mytype = GetType().ToString().Split( '.' ).LastOrDefault();
            System.Windows.Application.Current.Dispatcher.Invoke( (Action)(() =>
            { System.Windows.MessageBox.Show( e.Message, $"{mytype}.{name}" ); }) );
        }

        #endregion

    }
}
