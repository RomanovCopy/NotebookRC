using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Runtime.CompilerServices;
using System.Threading;
using NotebookRCv001.ViewModels;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.IO;

namespace NotebookRCv001.Models
{
    internal class FixedDocumentReaderModel:ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private Helpers.BehaviorFixedDocument behaviorFixedDocument { get; set; }
        private Languages language => mainWindowViewModel.Language;
        internal ObservableCollection<string> Headers => language.FixedDocumentHeaders;
        internal ObservableCollection<string> ToolTips => language.ToolTipsFixedDocument;
        internal Action<object> BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        private Action<object> behaviorReady;

        internal XpsDocument Document { get => behaviorFixedDocument.Document; set => behaviorFixedDocument.Document = value; }
        internal string PathToLastFile { get => pathToLastFile; set => SetProperty(ref pathToLastFile, value); }
        private string pathToLastFile;
        internal string LastFileName { get => lastFileName; set => SetProperty(ref lastFileName, value); }
        private string lastFileName;

        internal FixedDocumentReaderModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
        }

        /// <summary>
        /// загрузка файла PDF с заданного адресса
        /// </summary>
        /// <param name="obj">путь к файлу(string)</param>
        /// <returns></returns>
        internal bool CanExecute_LoadedPDF(object obj)
        {
            try
            {
                bool c = false;
                c = obj is string path && File.Exists(path);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_LoadedPDF(object obj)
        {
            try
            {
                var document = behaviorFixedDocument.LoadDocumentPDF(obj.ToString());
                behaviorFixedDocument.Document = document;
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_DocumentViewerLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_DocumentViewerLoaded(object obj)
        {
            try
            {
                if(obj is Helpers.BehaviorFixedDocument behavior)
                {
                    behaviorFixedDocument = behavior;
                    if (BehaviorReady != null)
                        BehaviorReady.Invoke(behavior);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageClear(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClear(object obj)
        {
            try
            {

            }
            catch(Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageLoaded(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageClose(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }



    }
}
