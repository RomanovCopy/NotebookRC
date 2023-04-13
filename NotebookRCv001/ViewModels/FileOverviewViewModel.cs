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
    public class FileOverviewViewModel:ViewModelBase, IWindowViewModel
    {
        private readonly FileOverviewModel fileOverviewModel;

        #region _______________Sizes and Positions____________________________________
        public double WindowWidth { get => fileOverviewModel.WindowWidth; set => fileOverviewModel.WindowWidth = value; }
        public double WindowHeight { get => fileOverviewModel.WindowHeight; set => fileOverviewModel.WindowHeight = value; }
        public double WindowTop { get => fileOverviewModel.WindowTop; set => fileOverviewModel.WindowTop = value; }
        public double WindowLeft { get => fileOverviewModel.WindowLeft; set => fileOverviewModel.WindowLeft = value; }
        public object WindowState { get => fileOverviewModel.WindowState; set => fileOverviewModel.WindowState = value; }
        #endregion


        #region ________________ListView Columns____________________________

        /// <summary>
        /// коллекция размеров колонок по горизонтали(Width)
        /// </summary>
        public ObservableCollection<double> ListView_ColumnsWidth => fileOverviewModel.ListView_ColumnsWidth;

        #endregion

        public ObservableCollection<string> Headers => fileOverviewModel.Headers;
        public ObservableCollection<string> ToolTips => fileOverviewModel.ToolTips;
        public ObservableCollection<DirectoryItem> CurrentDirectoryList => fileOverviewModel.CurrentDirectoryList;
        public ObservableCollection<DriveInfo> DriveInfos => fileOverviewModel.DriveInfos;
        public int SelectedIndex { get => fileOverviewModel.SelectedIndex; set => fileOverviewModel.SelectedIndex = value; }

        public bool CoverEnabled { get => fileOverviewModel.CoverEnabled; set => fileOverviewModel.CoverEnabled = value; }
        public DirectoryInfo CurrentDirectory => fileOverviewModel.CurrentDirectory;
        public string CurrentDirectoryFullName => fileOverviewModel.CurrentDirectoryFullName;

        /// <summary>
        /// Размер иконки обложки(высота)
        /// </summary>
        public ObservableCollection<int> IconSizes { get => fileOverviewModel.IconSizes ; set => fileOverviewModel.IconSizes=value; }
        /// <summary>
        /// индекс выбранного размера иконки
        /// </summary>
        public int IconSizesIndex { get => fileOverviewModel.IconSizesIndex; set => fileOverviewModel.IconSizesIndex=value; }
        /// <summary>
        /// текущая высота иконок, задается для выравнивания
        /// </summary>
        public int ImageHeight { get => fileOverviewModel.ImageHeight; set => fileOverviewModel.ImageHeight = value; }

        public FileOverviewViewModel()
        {
            fileOverviewModel = new FileOverviewModel();
            fileOverviewModel.PropertyChanged += ( s, e ) => OnPropertyChanged( e.PropertyName );
        }

        public ICommand CheckedIsCover => checkedIsCover ??= new RelayCommand( fileOverviewModel.Execute_CheckedIsCover, 
            fileOverviewModel.CanExecute_CheckedIsCover );
        private RelayCommand checkedIsCover;
        public ICommand ComboBoxLoaded => comboBoxLoaded ??= new RelayCommand( fileOverviewModel.Execute_ComboBoxLoaded,
            fileOverviewModel.CanExecute_ComboBoxLoaded );
        private RelayCommand comboBoxLoaded;
        public ICommand ComboBoxSelectionChanged => comboBoxSelectionChanged ??= new RelayCommand( fileOverviewModel.Execute_ComboBoxSelectionChanged,
            fileOverviewModel.CanExecute_ComboBoxSelectionChanged );
        private RelayCommand comboBoxSelectionChanged;
        public ICommand IconSizesLoaded => iconSizesLoaded ??= new RelayCommand( fileOverviewModel.Execute_IconSizesLoaded,
            fileOverviewModel.CanExecute_IconSizesLoaded );
        private RelayCommand iconSizesLoaded;
        public ICommand IconSizesSelectionChanged => iconSizesSelectionChanged ??= new RelayCommand( fileOverviewModel.Execute_IconSizesSelectionChanged,
            fileOverviewModel.CanExecute_IconSizesSelectionChanged );
        private RelayCommand iconSizesSelectionChanged;
        public ICommand ToParentDirectory => toParentDirectory ??= new RelayCommand( fileOverviewModel.Execute_ToParentDirectory,
            fileOverviewModel.CanExecute_ToParentDirectory );
        private RelayCommand toParentDirectory;
        public ICommand ListViewNameMouseLeftButtonDown => listViewNameMouseLeftButtonDown ??= new RelayCommand(
            fileOverviewModel.Execute_ListViewNameMouseLeftButtonDown, fileOverviewModel.CanExecute_ListViewNameMouseLeftButtonDown );
        private RelayCommand listViewNameMouseLeftButtonDown;
        public ICommand ListViewNameContextMenuOpen => listViewNameContextMenuOpen ??= new RelayCommand(
            fileOverviewModel.Execute_ListViewNameContextMenuOpen, fileOverviewModel.CanExecute_ListViewNameContextMenuOpen );
        private RelayCommand listViewNameContextMenuOpen;
        public ICommand ListViewLoaded => listViewLoaded ??= new RelayCommand( fileOverviewModel.Execute_ListViewLoaded, fileOverviewModel.CanExecute_ListViewLoaded );
        private RelayCommand listViewLoaded;
        public ICommand WindowSizeChanged => windowSizeChanged ??= new RelayCommand( fileOverviewModel.Execute_WindowSizeChanged, fileOverviewModel.CanExecute_WindowSizeChanged );
        private RelayCommand windowSizeChanged;
        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand
            ( fileOverviewModel.Execute_WindowLoaded, fileOverviewModel.CanExecute_WindowLoaded );
        private RelayCommand windowLoaded;

        public ICommand WindowClosing => windowClosing ??= new RelayCommand
            ( fileOverviewModel.Execute_WindowClosing, fileOverviewModel.CanExecute_WindowClosing );
        private RelayCommand windowClosing;
    }
}
