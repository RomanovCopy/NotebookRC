using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Helpers;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Models
{
    internal class FlowDocumentReaderModel : ViewModelBase
    {
        private readonly ViewModels.MainWindowViewModel mainWindowViewModel;
        private readonly ViewModels.RichTextBoxViewModel richTextBoxViewModel;
        private readonly Helpers.BehaviorRichTextBox behaviorRichTextBox;
        private readonly ViewModels.HomeViewModel homeViewModel;
        private readonly ViewModels.HomeMenuFileViewModel homeMenuFileViewModel;
        private object readerPage;
        private BehaviorFlowDocumentReader behaviorFlowDocumentReader { get; set; }
        private Languages language => mainWindowViewModel.Language;

        internal Visibility Visibility { get => behaviorFlowDocumentReader.Visibility; set => behaviorFlowDocumentReader.Visibility = value; }
        internal FlowDocument Document => behaviorFlowDocumentReader?.Document;

        internal ObservableCollection<string> Headers => language.HomeMenuFile;
        internal ObservableCollection<string> ToolTips => language.ToolTipsHomeMenuFile;

        /// <summary>
        /// делегат выполняемый после определения BehaviorFlowDocumentReader
        /// </summary>
        internal Action<object> BehaviorReady { get => behaviorReady; set => SetProperty(ref behaviorReady, value); }
        private Action<object> behaviorReady;


        /// <summary>
        /// путь к последнему открытому или сохраненному файлу
        /// </summary>
        internal string PathToLastFile { get => pathToLastFile; set => SetProperty(ref pathToLastFile, value); }
        private string pathToLastFile;
        /// <summary>
        /// Имя последнего открытого или сохраненного файла
        /// </summary>
        internal string LastFileName { get => lastFileName; set => SetProperty(ref lastFileName, value); }
        private string lastFileName;



        internal FlowDocumentReaderModel()
        {
            mainWindowViewModel = (ViewModels.MainWindowViewModel)Application.Current.MainWindow.DataContext;
            mainWindowViewModel.Language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            var home = mainWindowViewModel.FrameList.Where((x) => x is Views.Home).FirstOrDefault();
            if (home != null)
            {
                homeViewModel = (HomeViewModel)home.DataContext;
                var richtextbox = (MyControls.RichTextBox)home.FindResource("richtextbox");
                var menuhome = (MyControls.MenuHome)home.FindResource("menuhome");
                homeMenuFileViewModel = (ViewModels.HomeMenuFileViewModel)menuhome.FindResource("menufile");
                richTextBoxViewModel = (ViewModels.RichTextBoxViewModel)richtextbox.DataContext;
                behaviorRichTextBox = richTextBoxViewModel.BehaviorRichTextBox;
            }
        }




        internal bool CanExecute_OpenFile(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OpenFile(object obj)
        {
            try
            {
                homeMenuFileViewModel.OpenFile.Execute(null);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_EditFile(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorFlowDocumentReader.Document != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_EditFile(object obj)
        {
            try
            {
                var document = behaviorFlowDocumentReader.CloneDocument();
                behaviorRichTextBox.Document = document;
                mainWindowViewModel.CurrentPage = mainWindowViewModel.FrameList.Where((x) => x is Views.Home).FirstOrDefault();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageClear(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorFlowDocumentReader?.Document.Blocks.Count > 0;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClear(object obj)
        {
            try
            {
                behaviorFlowDocumentReader.Clear();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_ReaderLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ReaderLoaded(object obj)
        {
            try
            {
                if (obj is BehaviorFlowDocumentReader behavior)
                {
                    if (behaviorFlowDocumentReader == null)
                    {
                        behaviorFlowDocumentReader = behavior;
                        var document = behaviorRichTextBox.CloneDocument();
                        behaviorFlowDocumentReader.Document = document;
                        LastFileName = homeViewModel.LastFileName;
                        PathToLastFile = homeViewModel.PathToLastFile;
                    }
                    if (BehaviorReady != null)
                    {
                        BehaviorReady.Invoke(behavior);
                        BehaviorReady = null;
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageLoaded(object obj)
        {
            try
            {
                if (obj is Views.FlowDocumentReader reader)
                {
                    readerPage = reader;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageClose(object obj)
        {
            try
            {
                bool c = false;
                c = readerPage != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose(object obj)
        {
            try
            {
                if (readerPage is Views.FlowDocumentReader reader)
                {
                    if (mainWindowViewModel.PageClosed.CanExecute(reader))
                        mainWindowViewModel.PageClosed.Execute(reader);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal FlowDocument GetCloneDocument()
        {
            try
            {
                return behaviorFlowDocumentReader.CloneDocument();
            }
            catch (Exception e) { ErrorWindow(e); return new FlowDocument(); }
        }

        private void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"FlowDocumentReaderModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }


    }
}
