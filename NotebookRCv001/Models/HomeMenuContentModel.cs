using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Views;

namespace NotebookRCv001.Models
{
    internal class HomeMenuContentModel : ViewModelBase
    {

        private readonly MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;
        private MenuHomeViewModel menuHomeViewModel { get; set; }
        private HomeMenuFileViewModel homeMenuFileViewModel { get; set; }
        private RichTextBoxViewModel richTextBoxViewModel { get; set; }
        private FlowDocumentReaderViewModel flowDocumentReaderViewModel { get; set; }
        private string Filter => "All files (*.*)|*.*|Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";
        internal ObservableCollection<string> Headers => language.HomeMenuContent;
        internal ObservableCollection<string> ToolTips => language.ToolTipsHomeMenuContent;
        internal HomeMenuContentModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            mainWindowViewModel.FrameList.CollectionChanged += (s, e) =>
            {
                if (mainWindowViewModel.CurrentPage is Home home && e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                {
                    var richtextbox = (MyControls.RichTextBox)home.FindResource("richtextbox");
                    richTextBoxViewModel = (RichTextBoxViewModel)richtextbox.FindResource("viewmodel");
                    flowDocumentReaderViewModel = (FlowDocumentReaderViewModel)richtextbox.FindResource("viewmodelreader");
                    var homemenu = (MyControls.MenuHome)home.FindResource("menuhome");
                    menuHomeViewModel = (MenuHomeViewModel)homemenu.DataContext;
                    homeMenuFileViewModel = (HomeMenuFileViewModel)homemenu.FindResource("menufile");
                }
            };
        }


        /// <summary>
        /// переключение в режим чтения
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_Reading(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_Reading(object obj)
        {
            try
            {
                Views.FlowDocumentReader flowDocumentReader = new() { KeepAlive=true };
                if (mainWindowViewModel.FrameListAddPage.CanExecute(flowDocumentReader))
                    mainWindowViewModel.FrameListAddPage.Execute(flowDocumentReader);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// вставка изображения
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_InsertImage(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_InsertImage(object obj)
        {
            try
            {
                var win = new SelectAndPasteWindow();
                win.Owner = Application.Current.MainWindow;
                win.Show();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// вставка текста
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_InsertText(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_InsertText(object obj)
        {
            try
            {
                var editor = new Views.TextEditor();
                editor.Closing += (s, e) =>
                {
                    if (s is Views.TextEditor texteditor)
                    {

                        var datacontext = (TextEditorViewModel)texteditor.DataContext;
                        var textrange = new TextRange(datacontext.Document.ContentStart, datacontext.Document.ContentEnd);
                        Paragraph paragraph = (Paragraph)richTextBoxViewModel.Document.Blocks.Where((x) => x is Paragraph).LastOrDefault();
                        TextRange range = null;
                        foreach(var w in Application.Current.Windows)
                        {//ищем выбранный параграф в DocumentTree
                            if(w is Views.DocumentTree docTree)
                            {
                                var docTreeVM = (DocumentTreeViewModel)docTree.DataContext;
                                if (docTreeVM.FlowDocumentLastSelected is Paragraph p)
                                    paragraph = p;
                                break;
                            }
                        }
                        if(paragraph==null)
                        {//в Document нет параграфов
                            paragraph = new(new Run(" "));
                            richTextBoxViewModel.Document.Blocks.Add(paragraph);
                        }
                        paragraph.Inlines.Add(new Run(" "));
                        range = new(paragraph.Inlines.LastInline.ContentStart, paragraph.Inlines.LastInline.ContentEnd);
                        using (MemoryStream stream = new())
                        {//помещаем текст в выбранный параграф
                            System.Windows.Markup.XamlWriter.Save(textrange, stream);
                            textrange.Save(stream, DataFormats.XamlPackage);
                            range.Load(stream, DataFormats.XamlPackage);
                        }
                    }
                };
                editor.ShowDialog();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// открыть дерево документа
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_OpenDocumentTree(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OpenDocumentTree(object obj)
        {
            try
            {
                DocumentTree documentTree = new();
                documentTree.Owner = Application.Current.MainWindow;
                documentTree.Show();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        private void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            MyMessages myMessages = new MyMessages();
            var viewmodel = (ViewModels.MyMessagesViewModel)myMessages.DataContext;
            var mytype = GetType().ToString().Split('.').LastOrDefault();
            viewmodel.SetTitle.Execute($"{mainWindowViewModel.Language.MyMessagesHeaders[0]}! ({mytype}.{name})");
            viewmodel.SetMessage.Execute(e.Message);
            myMessages.Show();
        }

    }
}
