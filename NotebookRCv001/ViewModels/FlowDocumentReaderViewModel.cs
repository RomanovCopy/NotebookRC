using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;
using System.Windows;
using System.Windows.Documents;
using NotebookRCv001.Helpers;

namespace NotebookRCv001.ViewModels
{
    public class FlowDocumentReaderViewModel: ViewModelBase, IPageViewModel, ILastFileName
    {

        private readonly FlowDocumentReaderModel flowDocumentReaderModel;

        public ObservableCollection<string> Headers => flowDocumentReaderModel.Headers;

        public ObservableCollection<string> ToolTips => flowDocumentReaderModel.ToolTips;

        internal Visibility Visibility { get => flowDocumentReaderModel.Visibility; set => flowDocumentReaderModel.Visibility = value; }
        internal FlowDocument Document => flowDocumentReaderModel.Document;

        public FlowDocument CloneDocument => flowDocumentReaderModel.GetCloneDocument();

        public Action<object> BehaviorReady { get => flowDocumentReaderModel.BehaviorReady; set => flowDocumentReaderModel.BehaviorReady=value; }

        public string PathToLastFile { get => flowDocumentReaderModel.PathToLastFile; set => flowDocumentReaderModel.PathToLastFile = value; }
        public string LastFileName { get => flowDocumentReaderModel.LastFileName; set => flowDocumentReaderModel.LastFileName = value; }


        public ObservableCollection<string> Icons => throw new NotImplementedException();

        public ObservableCollection<string> Images => throw new NotImplementedException();


        public FlowDocumentReaderViewModel()
        {
            flowDocumentReaderModel = new FlowDocumentReaderModel();
            flowDocumentReaderModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }


        public ICommand OpenFile => openFile ??= new RelayCommand(flowDocumentReaderModel.Execute_OpenFile,
            flowDocumentReaderModel.CanExecute_OpenFile);
        private RelayCommand openFile;

        public ICommand EditFile => editFile ??= new RelayCommand(flowDocumentReaderModel.Execute_EditFile,
            flowDocumentReaderModel.CanExecute_EditFile);
        private RelayCommand editFile;

        public ICommand PageClear => pageClear ??= new RelayCommand(flowDocumentReaderModel.Execute_PageClear,
            flowDocumentReaderModel.CanExecute_PageClear);
        private RelayCommand pageClear;

        public ICommand ReaderLoaded => readerLoaded ??= new RelayCommand(flowDocumentReaderModel.Execute_ReaderLoaded,
            flowDocumentReaderModel.CanExecute_ReaderLoaded);
        private RelayCommand readerLoaded;

        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(flowDocumentReaderModel.Execute_PageLoaded,
            flowDocumentReaderModel.CanExecute_PageLoaded);
        private RelayCommand pageLoaded;

        public ICommand PageClose => pageClose ??= new RelayCommand(flowDocumentReaderModel.Execute_PageClose,
            flowDocumentReaderModel.CanExecute_PageClose);
        private RelayCommand pageClose;


    }
}
