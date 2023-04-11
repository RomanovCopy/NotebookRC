using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models;
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

        public ObservableCollection<DriveInfo> DriveInfos { get => fileExplorerModel.DriveInfos; set => fileExplorerModel.DriveInfos=value; }
        public int SelectedIndexDrives { get => fileExplorerModel.SelectedIndexDrives; set => fileExplorerModel.SelectedIndexDrives = value; }

        public FileExplorerViewModel()
        {
            fileExplorerModel = new();
            fileExplorerModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);

        }

        public ICommand ClickToParentDirectory => clickToParentDirectory ??= new RelayCommand(
            fileExplorerModel.Execute_ClickToParentDirectory, fileExplorerModel.CanExecute_ClickToParentDirectory);
        private RelayCommand clickToParentDirectory;
        public ICommand ComboBoxDrivesSelectionChanged => comboBoxDrivesSelectionChanged ??= new RelayCommand(
            fileExplorerModel.Execute_ComboBoxDrivesSelectionChanged, fileExplorerModel.CanExecute_ComboBoxDrivesSelectionChanged);
        private RelayCommand comboBoxDrivesSelectionChanged;
        public ICommand ComboBoxDrivesLoaded => comboBoxDrivesLoaded ??= new RelayCommand(fileExplorerModel.Execute_ComboBoxDrivesLoaded,
            fileExplorerModel.CanExecute_ComboBoxDrivesLoaded);
        private RelayCommand comboBoxDrivesLoaded;

        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(fileExplorerModel.Execute_PageLoaded,
            fileExplorerModel.CanExecute_PageLoaded);
        private RelayCommand pageLoaded;

        public ICommand PageClose => pageClose ??= new RelayCommand(fileExplorerModel.Execute_PageClose,
            fileExplorerModel.CanExecute_PageClose);
        private RelayCommand pageClose;

        public ICommand PageClear => pageClear ??= new RelayCommand(fileExplorerModel.Execute_PageClear,
            fileExplorerModel.CanExecute_PageClear);
        private RelayCommand pageClear;
    }
}
