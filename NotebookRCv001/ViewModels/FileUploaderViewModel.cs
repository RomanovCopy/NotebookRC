using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class FileUploaderViewModel:ViewModelBase, IPageViewModel, IDisplayProgressTarget
    {

        private readonly FileUploaderModel fileUploaderModel;
        public ObservableCollection<string> Headers => fileUploaderModel.Headers;
        public ObservableCollection<string> ToolTips => fileUploaderModel.ToolTips;

        /// <summary>
        /// полный путь к каталогу с загружаемыми файлами
        /// </summary>
        internal string ContentDisposition
        { get => fileUploaderModel.ContentDisposition; private set => fileUploaderModel.ContentDisposition = value; }
        internal string DirectoryPathWithDownloadedFiles
        { get => fileUploaderModel.DirectoryPathWithDownloadedFiles; private set => fileUploaderModel.DirectoryPathWithDownloadedFiles = value; }

        /// <summary>
        /// общий прогресс загрузки
        /// </summary>
        public double ProgressValue { get => fileUploaderModel.ProgressValue; set => fileUploaderModel.ProgressValue = value; }

        public ObservableCollection<DownloadItemViewModel> ListDownoadItems => fileUploaderModel.ListDownoadItems;

        /// <summary>
        /// коллекция размеров колонок по горизонтали(Width)
        /// </summary>
        public ObservableCollection<double> ListView_ColumnsWidth { get => fileUploaderModel.ListView_ColumnsWidth;
            set => fileUploaderModel.ListView_ColumnsWidth = value; }


        public FileUploaderViewModel()
        {
            fileUploaderModel = new FileUploaderModel();
            fileUploaderModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }


        public Action BehaviorReady { get => fileUploaderModel.BehaviorReady; set => fileUploaderModel.BehaviorReady = value; }

        /// <summary>
        /// выбор всего содержимого TextBox url
        /// </summary>
        public ICommand TextBoxSelectAll => textBoxSelectAll ??= new RelayCommand(fileUploaderModel.Execute_TextBoxSelectAll,
            fileUploaderModel.CanExecute_TextBoxSelectAll);
        RelayCommand textBoxSelectAll;

        /// <summary>
        /// выбор директории загрузки для коллекции выделенных загрузок
        /// </summary>
        public ICommand SelectASpecialDirectory => selectASpecialDirectory ??= new RelayCommand(fileUploaderModel.Execute_SelectASpecialDirectory,
            fileUploaderModel.CanExecute_SelectASpecialDirectory);
        private RelayCommand selectASpecialDirectory;

        /// <summary>
        /// задание директории для загружаемых файлов ( обработка выбора команды в меню )
        /// </summary>
        public ICommand DirectoryForDownloadedFiles => directoryForDownloadedFiles ??= new RelayCommand(fileUploaderModel.Execute_DirectoryForDownloadedFiles,
            fileUploaderModel.CanExecute_DirectoryForDownloadedFiles);
        private RelayCommand directoryForDownloadedFiles;

        public ICommand PasteToTextBox => pasteToTextBox ??= new RelayCommand(fileUploaderModel.Execute_PasteToTextBox, 
            fileUploaderModel.CanExecute_PasteToTextBox);
        RelayCommand pasteToTextBox;

        public ICommand TexBoxCopy => texBoxCopy ??= new RelayCommand(fileUploaderModel.Execute_TexBoxCopy, fileUploaderModel.CanExecute_TexBoxCopy);
        RelayCommand texBoxCopy;

        public ICommand TextBoxClear => textBoxClear ??= new RelayCommand(fileUploaderModel.Execute_TextBoxClear, fileUploaderModel.CanExecute_TextBoxClear);
        RelayCommand textBoxClear;

        public ICommand AddNewDownloadItem => addNewDownloadItem ??= new RelayCommand(fileUploaderModel.Execute_AddNewDownloadItem,
            fileUploaderModel.CanExecute_AddNewDownloadItem);
        RelayCommand addNewDownloadItem;

        public ICommand TextBoxLoaded => textBoxLoaded ??= new RelayCommand(fileUploaderModel.Execute_TextBoxLoaded, 
            fileUploaderModel.CanExecute_TextBoxLoaded);
        RelayCommand textBoxLoaded;

        public ICommand ListViewLoaded => listViewLoaded ??= new RelayCommand(fileUploaderModel.Execute_ListViewLoaded,
            fileUploaderModel.CanExecute_ListViewLoaded);
        RelayCommand listViewLoaded;

        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(fileUploaderModel.Execute_PageLoaded, fileUploaderModel.CanExecute_PageLoaded);
        RelayCommand pageLoaded;

        public ICommand PageSizeChanged => pageSizeChanged ??= new RelayCommand(fileUploaderModel.Execute_PageSizeChanged,
            fileUploaderModel.CanExecute_PageSizeChanged);
        RelayCommand pageSizeChanged;

        public ICommand PageClose => pageClose ??= new RelayCommand(fileUploaderModel.Execute_PageClose, fileUploaderModel.CanExecute_PageClose);
        RelayCommand pageClose;

        public ICommand ListRemove => listRemove ??= new RelayCommand(fileUploaderModel.Execute_ListRemove, fileUploaderModel.CanExecute_ListRemove);
        RelayCommand listRemove;
        public ICommand ListClear => listClear ??= new RelayCommand(fileUploaderModel.Execute_ListClear, fileUploaderModel.CanExecute_ListClear);
        RelayCommand listClear;

        public ICommand PageClear => pageClear ??= new RelayCommand(fileUploaderModel.Execute_PageClear, fileUploaderModel.CanExecute_PageClear);
        private RelayCommand pageClear;



    }
}
