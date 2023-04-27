using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models;
using NotebookRCv001.Helpers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.IO;

namespace NotebookRCv001.ViewModels
{
    public class FileExplorerViewModel:ViewModelBase, IPageViewModel
    {
        private readonly FileExplorerModel fileExplorerModel;

        public ObservableCollection<string> Headers => fileExplorerModel.Headers;
        public ObservableCollection<string> ToolTips => fileExplorerModel.ToolTips;
        public Action<object> BehaviorReady { get => fileExplorerModel.BehaviorReady;
            set => fileExplorerModel.BehaviorReady = value; }
        public ObservableCollection<double> ListView_ColumnsWidth { get => fileExplorerModel.ListView_ColumnsWidth; 
            set => fileExplorerModel.ListView_ColumnsWidth = value; }
        public ObservableCollection<DriveInfo> DriveInfos { get => fileExplorerModel.DriveInfos; 
            set => fileExplorerModel.DriveInfos=value; }
        public ObservableCollection<DirectoryItem> CurrentDirectoryList
        { get => fileExplorerModel.CurrentDirectoryList; set => fileExplorerModel.CurrentDirectoryList = value; }
        public ObservableCollection<int> CoverSizes { get => fileExplorerModel.CoverSizes;
            set => fileExplorerModel.CoverSizes = value; }
        public int SelectedIndexDrives { get => fileExplorerModel.SelectedIndexDrives; 
            set => fileExplorerModel.SelectedIndexDrives = value; }
        public int CoverSizesIndex { get => fileExplorerModel.CoverSizesIndex;
            set => fileExplorerModel.CoverSizesIndex = value; }
        public int ImageHeight { get => fileExplorerModel.ImageHeight; set => fileExplorerModel.ImageHeight = value; }
        public string CurrentDirectoryFullName { get => fileExplorerModel.CurrentDirectoryFullName; 
            set => fileExplorerModel.CurrentDirectoryFullName = value; }
        public bool IsCoverEnabled { get => fileExplorerModel.IsCoverEnabled; set => fileExplorerModel.IsCoverEnabled = value; }
        public bool IsTilesEnabled { get => fileExplorerModel.IsTilesEnabled; set => fileExplorerModel.IsTilesEnabled = value; }


        public FileExplorerViewModel()
        {
            fileExplorerModel = new();
            fileExplorerModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);

        }



        public ICommand ListViewNameMouseLeftButtonDown => listViewNameMouseLeftButtonDown ??=
            new RelayCommand(fileExplorerModel.Execute_ListViewNameMouseLeftButtonDown, fileExplorerModel.CanExecute_ListViewNameMouseLeftButtonDown);
        private RelayCommand listViewNameMouseLeftButtonDown;
        public ICommand CheckedIsTilesEnabled => checkedIsTilesEnabled ??= 
            new RelayCommand(fileExplorerModel.Execute_CheckedIsTilesEnabled, fileExplorerModel.CanExecute_CheckedIsTilesEnabled);
        private RelayCommand checkedIsTilesEnabled;
        public ICommand CheckedIsCover => checkedIsCover ??= new RelayCommand(fileExplorerModel.Execute_CheckedIsCover,
            fileExplorerModel.CanExecute_CheckedIsCover);
        private RelayCommand checkedIsCover;
        public ICommand ClickToParentDirectory => clickToParentDirectory ??= new RelayCommand(
            fileExplorerModel.Execute_ClickToParentDirectory, fileExplorerModel.CanExecute_ClickToParentDirectory);
        private RelayCommand clickToParentDirectory;
        public ICommand ComboBoxDrivesSelectionChanged => comboBoxDrivesSelectionChanged ??= new RelayCommand(
            fileExplorerModel.Execute_ComboBoxDrivesSelectionChanged, fileExplorerModel.CanExecute_ComboBoxDrivesSelectionChanged);
        private RelayCommand comboBoxDrivesSelectionChanged;
        public ICommand CoverSizesSelectionChanged => coverSizesSelectionChanged ??= new RelayCommand(
            fileExplorerModel.Execute_CoverSizesSelectionChanged, fileExplorerModel.CanExecute_CoverSizesSelectionChanged);
        private RelayCommand coverSizesSelectionChanged;
        public ICommand CoverSizesLoaded => coverSizesLoaded ??= new RelayCommand(fileExplorerModel.Execute_CoverSizesLoaded,
            fileExplorerModel.CanExecute_CoverSizesLoaded);
        private RelayCommand coverSizesLoaded;
        public ICommand ComboBoxDrivesLoaded => comboBoxDrivesLoaded ??= new RelayCommand(fileExplorerModel.Execute_ComboBoxDrivesLoaded,
            fileExplorerModel.CanExecute_ComboBoxDrivesLoaded);
        private RelayCommand comboBoxDrivesLoaded;
        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(fileExplorerModel.Execute_PageLoaded,
            fileExplorerModel.CanExecute_PageLoaded);
        private RelayCommand pageLoaded;
        public ICommand PageSizeChanged => pageSizeChanged ??= new RelayCommand(fileExplorerModel.Execute_PageSizeChanged,
            fileExplorerModel.CanExecute_PageSizeChanged);
        private RelayCommand pageSizeChanged;
        public ICommand PageClose => pageClose ??= new RelayCommand(fileExplorerModel.Execute_PageClose,
            fileExplorerModel.CanExecute_PageClose);
        private RelayCommand pageClose;
        public ICommand PageClear => pageClear ??= new RelayCommand(fileExplorerModel.Execute_PageClear,
            fileExplorerModel.CanExecute_PageClear);
        private RelayCommand pageClear;
    }
}
