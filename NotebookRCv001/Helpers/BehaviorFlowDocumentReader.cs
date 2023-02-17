using Microsoft.Xaml.Behaviors;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;

namespace NotebookRCv001.Helpers
{
    public class BehaviorFlowDocumentReader : Behavior<FlowDocumentReader>
    {
        #region ____________________Publick Properties_________________________

        public FlowDocument Document { get => AssociatedObject.Document; set => AssociatedObject.Document = value; }

        #endregion

        #region ______________________Private Properties________________________

        private readonly ViewModels.MainWindowViewModel mainWindowViewModel;
        private readonly ViewModels.HomeViewModel homeViewModel;
        private readonly ViewModels.RichTextBoxViewModel richTextBoxViewModel;

        #endregion

        #region ______________________________Dependency properties______________________________


        /// <summary>
        /// активация/деактивация элемента в контроле RichTextBox
        /// </summary>
        public Visibility Visibility { get => AssociatedObject.Visibility; set => AssociatedObject.Visibility = value; }

        #endregion

        #region ________________________________Constructors_______________________________________


        static BehaviorFlowDocumentReader()
        {

        }

        public BehaviorFlowDocumentReader()
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
                AssociatedObject.IsVisibleChanged += AssociatedObject_IsVisibleChanged;
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


        #endregion

        #region _________________________Evant Handlers______________________________

        private void AssociatedObject_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        #endregion

        #region _______________________Publick Methods____________________________



        public void Clear()
        {
            try
            {
                if (Document?.Blocks.Count != null)
                    Document.Blocks.Clear();
                homeViewModel.PathToLastFile = null;
                homeViewModel.OnPropertyChanged("LastFileName");
            }
            catch(Exception e) { ErrorWindow(e); }
        }
        public FlowDocument CloneDocument()
        {
            try
            {
                var document2 = new FlowDocument();
                var document = AssociatedObject.Document;
                if (document != null)
                {
                    TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
                    MemoryStream stream = new MemoryStream();
                    System.Windows.Markup.XamlWriter.Save(range, stream);
                    range.Save(stream, DataFormats.XamlPackage);
                    TextRange range2 = new TextRange(document2.ContentEnd, document2.ContentEnd);
                    range2.Load(stream, DataFormats.XamlPackage);
                    stream.Close();
                }
                return document2;
            }
            catch (Exception e) { ErrorWindow(e); return new FlowDocument(); }
        }

        #endregion



        private static void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new Thread(() => MessageBox.Show(e.Message, $"BehaviorFlowDocumentReader.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
