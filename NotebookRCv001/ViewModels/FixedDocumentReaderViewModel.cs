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
using NotebookRCv001.Models;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace NotebookRCv001.ViewModels
{
    public class FixedDocumentReaderViewModel : ViewModelBase, IPageViewModel, ILastFileName
    {

        private readonly FixedDocumentReaderModel fixedDocumentReaderModel;
        public ObservableCollection<string> Headers => fixedDocumentReaderModel.Headers;

        public ObservableCollection<string> ToolTips => fixedDocumentReaderModel.ToolTips;

        public Action BehaviorReady { get => fixedDocumentReaderModel.BehaviorReady; set => fixedDocumentReaderModel.BehaviorReady = value; }

        public XpsDocument Document { get => fixedDocumentReaderModel.Document; set => fixedDocumentReaderModel.Document = value; }


        public ObservableCollection<string> Icons => throw new NotImplementedException();

        public ObservableCollection<string> Images => throw new NotImplementedException();


        public string PathToLastFile { get => fixedDocumentReaderModel.PathToLastFile; set => fixedDocumentReaderModel.PathToLastFile=value; }
        public string LastFileName { get => fixedDocumentReaderModel.LastFileName; set => fixedDocumentReaderModel.LastFileName=value; }



        public FixedDocumentReaderViewModel()
        {
            fixedDocumentReaderModel = new FixedDocumentReaderModel();
            fixedDocumentReaderModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// загрузка PDF файла с заданного адресса
        /// </summary>
        public ICommand LoadedPDF => loadedPDF ??= new RelayCommand(fixedDocumentReaderModel.Execute_LoadedPDF,
            fixedDocumentReaderModel.CanExecute_LoadedPDF);
        private RelayCommand loadedPDF;

        public ICommand DocumentViewerLoaded => documentViewerLoaded ??= new RelayCommand(fixedDocumentReaderModel.Execute_DocumentViewerLoaded,
            fixedDocumentReaderModel.CanExecute_DocumentViewerLoaded);
        private RelayCommand documentViewerLoaded;

        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(fixedDocumentReaderModel.Execute_PageLoaded,
            fixedDocumentReaderModel.CanExecute_PageLoaded);
        private RelayCommand pageLoaded;

        public ICommand PageClose => pageClose ??= new RelayCommand(fixedDocumentReaderModel.Execute_PageClose,
            fixedDocumentReaderModel.CanExecute_PageClose);
        private RelayCommand pageClose;

        public ICommand PageClear => pageClear ??= new RelayCommand(fixedDocumentReaderModel.Execute_PageClear,
            fixedDocumentReaderModel.CanExecute_PageClear);
        private RelayCommand pageClear;

    }
}
