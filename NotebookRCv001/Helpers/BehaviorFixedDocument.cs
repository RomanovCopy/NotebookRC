using Microsoft.Xaml.Behaviors;

using NotebookRCv001.ViewModels;

//using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Xps.Packaging;
using controls = System.Windows.Controls;
using System.IO;

namespace NotebookRCv001.Helpers
{
    public class BehaviorFixedDocument : Behavior<controls.DocumentViewer>
    {

        #region ______________________PrivateProperies_________________________________


        private readonly ViewModels.MainWindowViewModel mainWindowViewModel;
        private ViewModels.HomeViewModel homeViewModel { get; set; }
        private ViewModels.RichTextBoxViewModel richTextBoxViewModel { get; set; }


        #endregion


        #region _________________________PublicProperty_______________________________




        #endregion


        #region ____________DependencyProperty_______________________

        internal XpsDocument Document { get => (XpsDocument)GetValue(DocumentProperty); set => SetValue(DocumentProperty, value); }
        public static readonly DependencyProperty DocumentProperty;

        #endregion


        #region _________________Constructors__________________________

        static BehaviorFixedDocument()
        {
            try
            {
                DocumentProperty = DependencyProperty.Register("Document", typeof(XpsDocument),
                    typeof(BehaviorFixedDocument),
                    new PropertyMetadata(new PropertyChangedCallback(DocumentChanged)));
            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
        }


        public BehaviorFixedDocument()
        {
            mainWindowViewModel = (ViewModels.MainWindowViewModel)Application.Current.MainWindow.DataContext;
            var home = mainWindowViewModel.FrameList.Where((x) => x is Views.Home).FirstOrDefault();
            homeViewModel = (ViewModels.HomeViewModel)home.DataContext;
            richTextBoxViewModel = (ViewModels.RichTextBoxViewModel)((MyControls.RichTextBox)home.FindResource("richtextbox")).DataContext;
        }


        protected override void OnAttached()
        {
            try
            {

            }
            catch (Exception e)
            {
                ErrorWindow(e);
            }
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

        #endregion


        #region ______________________Event handlers________________

        private static void DocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if( d is BehaviorFixedDocument behavior)
                {
                    behavior.AssociatedObject.Document = behavior.Document.GetFixedDocumentSequence();
                }
            }
            catch (Exception ex) { ErrorWindow(ex); }

        }

        #endregion

        #region ________________________Public Methods________________________________

        public void Clear()
        {
            try
            {
            }
            catch(Exception e) { ErrorWindow(e); }
        }


        public XpsDocument LoadDocumentPDF(string path)
        {
            try
            {
                XpsDocument xpsDocument = null;
                if(File.Exists(path) && Path.GetExtension(path).ToLower() == ".pdf")
                {
                    //var doc = new Document(path);
                    //var currentpath = Path.Combine("temp", Path.GetFileNameWithoutExtension(path), ".xps");
                    //doc.Save(currentpath);
                    //xpsDocument = new XpsDocument(currentpath, FileAccess.Read);
                }
                return xpsDocument;
            }
            catch(Exception e) { ErrorWindow(e); return null; }
        }

        #endregion

        #region _____________________Private Methods____________________________



        private static void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            System.Windows.Application.Current.Dispatcher.Invoke( (Action)(() =>
            { System.Windows.MessageBox.Show( e.Message, $"BehaviorFixedDocument.{name}" ); }) );
        }

        #endregion



    }
}
