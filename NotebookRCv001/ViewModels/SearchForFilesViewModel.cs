using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using System.Windows.Input;
using System.IO;

namespace NotebookRCv001.ViewModels
{
    public class SearchForFilesViewModel:ViewModelBase, IPageViewModel
    {
        public SearchForFilesModel SearchForFilesModel => searchForFilesModel;
        private readonly SearchForFilesModel searchForFilesModel;

        /// <summary>
        /// инициатор запроса
        /// </summary>
        public object RequestInitiator { get => SearchForFilesModel.RequestInitiator; 
            set => SearchForFilesModel.RequestInitiator = value; }

        /// <summary>
        /// обнаруженные при поиске файлы
        /// </summary>
        public ObservableCollection<FileInfo> DetectedFiles
        {
            get => SearchForFilesModel.DetectedFiles;
            set => SearchForFilesModel.DetectedFiles = value;
        }
        /// <summary>
        /// заголовки
        /// </summary>
        public ObservableCollection<string> Headers => SearchForFilesModel.Headers;
        /// <summary>
        /// подсказки
        /// </summary>
        public ObservableCollection<string> ToolTips => SearchForFilesModel.ToolTips;
        /// <summary>
        /// размеры колонок в процентах
        /// </summary>
        public ObservableCollection<double> ColumnsWidth 
        { get => SearchForFilesModel.ColumnsWidth; set => SearchForFilesModel.ColumnsWidth = value; }

        public Action BehaviorReady { get => searchForFilesModel.BehaviorReady; set => searchForFilesModel.BehaviorReady = value; }

        public ObservableCollection<string> Icons => throw new NotImplementedException();

        public ObservableCollection<string> Images => throw new NotImplementedException();

        public SearchForFilesViewModel()
        {
            searchForFilesModel = new SearchForFilesModel();
            SearchForFilesModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }

        public ICommand ListViewLoaded => listViewLoaded ??= new RelayCommand(SearchForFilesModel.Execute_ListViewLoaded,
            SearchForFilesModel.CanExecute_ListViewLoaded);
        RelayCommand listViewLoaded;

        public ICommand ListViewSelectionChanged => listViewSelectionChanged ??= 
            new RelayCommand(SearchForFilesModel.Execute_ListViewSelectionChanged, SearchForFilesModel.CanExecute_ListViewSelectionChanged);
        RelayCommand listViewSelectionChanged;

        public ICommand ListViewPreviewMouseDoubleClick => listViewPreviewMouseDoubleClick ??=
            new RelayCommand(SearchForFilesModel.Execute_ListViewPreviewMouseDoubleClick, 
            SearchForFilesModel.CanExecute_ListViewPreviewMouseDoubleClick);
        RelayCommand listViewPreviewMouseDoubleClick;


        public ICommand DeleteFile => deleteFile ??= new RelayCommand(searchForFilesModel.Execute_DeleteFile, 
            searchForFilesModel.CanExecute_DeleteFile);
        RelayCommand deleteFile;


        public ICommand PageClose => pageClose ??= new RelayCommand(SearchForFilesModel.Execute_PageClose,
            SearchForFilesModel.CanExecute_PageClose);
        RelayCommand pageClose;

        public ICommand PageLoaded => throw new NotImplementedException();

        public ICommand PageClear => throw new NotImplementedException();
    }
}
