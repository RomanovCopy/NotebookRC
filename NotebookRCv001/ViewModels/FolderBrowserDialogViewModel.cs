using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;

namespace NotebookRCv001.ViewModels
{
    public class FolderBrowserDialogViewModel : ViewModelBase
    {

        private readonly FolderBrowserDialogModel folderBrowserDialogModel;

        public ObservableCollection<string> Headers => folderBrowserDialogModel.Headers;
        public ObservableCollection<string> ToolTips => folderBrowserDialogModel.ToolTips;

        public string WorkingDirectory => folderBrowserDialogModel.WorkingDirectory;

        #region ________________________Положение и размеры главного окна___________________
        public double WindowWidth
        {
            get => folderBrowserDialogModel.WindowWidth;
            set => folderBrowserDialogModel.WindowWidth = value;
        }
        public double WindowHeight
        {
            get => folderBrowserDialogModel.WindowHeight;
            set => folderBrowserDialogModel.WindowHeight = value;
        }
        public double WindowTop
        {
            get => folderBrowserDialogModel.WindowTop;
            set => folderBrowserDialogModel.WindowTop = value;
        }
        public double WindowLeft
        {
            get => folderBrowserDialogModel.WindowLeft;
            set => folderBrowserDialogModel.WindowLeft = value;
        }

        #endregion


        public FolderBrowserDialogViewModel()
        {
            folderBrowserDialogModel = new FolderBrowserDialogModel();
            folderBrowserDialogModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }

        #region _____________________________________Buttons____________________________________________

        public ICommand ClickNewDirectory => clickNewDirectory ??= new RelayCommand(folderBrowserDialogModel.Execute_ClickNewDirectory,
            folderBrowserDialogModel.CanExecute_ClickNewDirectory);
        RelayCommand clickNewDirectory;

        public ICommand ClickAccept => clickAccept ??= new RelayCommand(folderBrowserDialogModel.Execute_ClickAccept,
            folderBrowserDialogModel.CanExecute_ClickAccept);
        RelayCommand clickAccept;

        public ICommand ClickCancel => clickCancel ??= new RelayCommand(folderBrowserDialogModel.Execute_ClickCancel,
            folderBrowserDialogModel.CanExecute_ClickCancel);
        RelayCommand clickCancel;

        public ICommand ClickMinimizeAllDrives => clickMinimizeAllDrives ??= 
            new RelayCommand(folderBrowserDialogModel.Execute_ClickMinimizeAllDrives,
            folderBrowserDialogModel.CanExecute_ClickMinimizeAllDrives);
        RelayCommand clickMinimizeAllDrives;

        #endregion


        public ICommand TreeViewLoaded => treeViewLoaded ??= new RelayCommand(folderBrowserDialogModel.Execute_TreeViewLoaded,
            folderBrowserDialogModel.CanExecute_TreeViewLoaded);
        private RelayCommand treeViewLoaded;

        public ICommand FolderBrowserDialogLoaded => folderBrowserDialogLoaded ??= new RelayCommand(folderBrowserDialogModel.Execute_FolderBrowserDialogLoaded,
            folderBrowserDialogModel.CanExecute_FolderBrowserDialogLoaded);
        private RelayCommand folderBrowserDialogLoaded;

        public ICommand FolderBrowserDialogClosing => folderBrowserDialogClosing ??= new RelayCommand(folderBrowserDialogModel.Execute_FolderBrowserDialogClosing,
            folderBrowserDialogModel.CanExecute_FolderBrowserDialogClosing);
        private RelayCommand folderBrowserDialogClosing;

    }
}
