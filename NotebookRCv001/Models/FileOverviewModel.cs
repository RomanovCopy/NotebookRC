using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Windows;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;
using System.Runtime.CompilerServices;
using System.Threading;
using NotebookRCv001.Converters;
using System.IO;
using NotebookRCv001.Helpers;
using System.Printing;

namespace NotebookRCv001.Models
{
    internal class FileOverviewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly HomeMenuFileViewModel homeMenuFileViewModel;
        private Languages language => mainWindowViewModel.Language;

        public class DirectoryItem
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public double Size { get; set; }
            public object Tag { get; set; }
        }


        #region ________________Sizes and Position________________________

        internal double WindowWidth { get => windowWidth; set => SetProperty( ref windowWidth, value ); }
        private double windowWidth;
        internal double WindowHeight { get => windowHeight; set => SetProperty( ref windowHeight, value ); }
        private double windowHeight;
        internal double WindowTop { get => windowTop; set => SetProperty( ref windowTop, value ); }
        private double windowTop;
        internal double WindowLeft { get => windowLeft; set => SetProperty( ref windowLeft, value ); }
        private double windowLeft;
        internal object WindowState { get => windowState; set => SetProperty( ref windowState, value ); }
        private object windowState;


        #endregion


        #region ________________Columns____________________________

        /// <summary>
        /// коллекция размеров колонок по горизонтали(Width)
        /// </summary>
        internal ObservableCollection<double> ListView_ColumnsWidth
        {
            get => listView_ColumnsWidth ??= new ObservableCollection<double>();
            set => SetProperty( ref listView_ColumnsWidth, value );
        }
        ObservableCollection<double> listView_ColumnsWidth;

        #endregion

        internal ObservableCollection<string> Headers => language.HeadersFileOverview;

        internal ObservableCollection<string> ToolTips => language.ToolTipsFileOverview;
        /// <summary>
        /// содержимое текущей директории
        /// </summary>
        internal ObservableCollection<object> CurrentDirectory 
        { 
            get => currentDirectory; 
            set => SetProperty( ref currentDirectory, value ); 
        }
        private ObservableCollection<object> currentDirectory;
        /// <summary>
        /// все каталоги текущей директории
        /// </summary>
        internal ObservableCollection<DirectoryInfo> DirectoriesDirectories { get => directoriesDirectories; 
            set => SetProperty( ref directoriesDirectories, value ); }
        private ObservableCollection<DirectoryInfo> directoriesDirectories;
        /// <summary>
        /// все файлы текущей директории
        /// </summary>
        internal ObservableCollection<FileInfo> DirectoryFiles { get => directoryFiles; set => SetProperty( ref directoryFiles, value ); }
        private ObservableCollection<FileInfo> directoryFiles;

        /// <summary>
        /// коллекция доступных для работы дисков
        /// </summary>
        internal ObservableCollection<DriveInfo> DriveInfos { get => driveInfos; private set => SetProperty( ref driveInfos, value ); }
        private ObservableCollection<DriveInfo> driveInfos;
        /// <summary>
        /// индекс выбранного диска в коллекции DriverInfos
        /// </summary>
        internal int SelectedIndex { get => selectedIndex; set => SetProperty( ref selectedIndex, value ); }
        private int selectedIndex;

        internal FileOverviewModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += ( s, e ) => OnPropertyChanged( new string[] { "Headers", "ToolTips" } );
            var home = (Views.Home)mainWindowViewModel.FrameList.Where( ( x ) => x is Views.Home ).FirstOrDefault();
            var menu = (MyControls.MenuHome)home.FindResource( "menuhome" );
            homeMenuFileViewModel = (HomeMenuFileViewModel)menu.FindResource( "menufile" );
            //восстанавливаем размеры и положение окна
            if (Properties.Settings.Default.FileOverviewFirstStart)
            {
                WindowHeight = 40;
                WindowWidth = 40;
                WindowLeft = 40;
                WindowTop = 40;
                Properties.Settings.Default.FileOverviewFirstStart = false;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.FileOverviewHeight;
                WindowWidth = Properties.Settings.Default.FileOverviewWidth;
                WindowLeft = Properties.Settings.Default.FileOverviewLeft;
                WindowTop = Properties.Settings.Default.FileOverviewTop;
            }
            //восстанавливаем состояние окна
            WindowState = Properties.Settings.Default.FileOverviewState;
            //устанавливаем размеры колонок
            //Properties.Settings.Default.FileUploader_ListViewColumnsWidth = null;
            if (Properties.Settings.Default.FileOverview_ListViewColumnsWidth == null)
                Properties.Settings.Default.FileOverview_ListViewColumnsWidth = new System.Collections.Specialized.StringCollection()
                { "40" ,"20" ,"30" ,"10"  };
            ListView_ColumnsWidth.Clear();
            for (int i = 0; i < Properties.Settings.Default.FileOverview_ListViewColumnsWidth.Count; i++)
                ListView_ColumnsWidth.Add( double.Parse( Properties.Settings.Default.FileOverview_ListViewColumnsWidth[i] ) );
        }

        /// <summary>
        /// окончание загрузки ComboBox
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ComboBoxLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ComboBoxLoaded( object obj )
        {
            try
            {
                SelectedIndex = 0;
                if (CanExecute_ComboBoxSelectionChanged( DriveInfos[SelectedIndex] ))
                    Execute_ComboBoxSelectionChanged( DriveInfos[SelectedIndex] );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// изменение выбора диска (ComboBox)
        /// </summary>
        /// <param name="obj">выбор: ComboBoxItem</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ComboBoxSelectionChanged( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ComboBoxSelectionChanged( object obj )
        {
            try
            {
                if (obj is DriveInfo drive)
                {
                    DirectoriesDirectories = new();
                    DirectoryFiles = new();
                    CurrentDirectory = new();
                    foreach(var dir in drive.RootDirectory.EnumerateDirectories())
                    {
                        var item = new DirectoryItem() { Name = dir.Name, Type = dir.GetType().ToString(), Size = 0, Tag = dir };
                        CurrentDirectory.Add( item );
                    }
                    foreach(var file in drive.RootDirectory.EnumerateFiles())
                    {
                        DirectoryFiles.Add( file );
                    }
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// при изменении размеров окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_WindowSizeChanged( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_WindowSizeChanged( object obj )
        {
            try
            {
                OnPropertyChanged( "ListView_ColumnsWidth" );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// окончание загрузки окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_WindowLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_WindowLoaded( object obj )
        {
            try
            {
                if (obj is Window window)
                {
                    var convert = (ColumnsWidthConverter)window.FindResource( "columnswidth" );
                    convert.window = window;
                }
                OnPropertyChanged( "ListView_ColumnsWidth" );
                UpdateDrives();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// перед закрытием окна
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_WindowClosing( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_WindowClosing( object obj )
        {
            try
            {
                var propert = Properties.Settings.Default;
                if (obj is System.Windows.Window win && win.WindowState == System.Windows.WindowState.Normal)
                {
                    propert.FileOverviewHeight = WindowHeight;
                    propert.FileOverviewWidth = WindowWidth;
                    propert.FileOverviewLeft = WindowLeft;
                    propert.FileOverviewTop = WindowTop;
                }
                propert.FileOverviewState = WindowState.ToString();
                propert.FileOverview_ListViewColumnsWidth = new();
                foreach(var width in ListView_ColumnsWidth)
                {
                    propert.FileOverview_ListViewColumnsWidth.Add( width.ToString() );
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }


        /// <summary>
        /// обновление коллекции доступных дисков
        /// </summary>
        private void UpdateDrives()
        {
            try
            {
                DriveInfos = new ObservableCollection<DriveInfo>();
                foreach (var info in GetDraveInfos())
                    DriveInfos.Add( info );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// получение информации о всех дисках готовых к работе
        /// </summary>
        /// <returns></returns>
        private DriveInfo[] GetDraveInfos()
        {
            var driveInfos = new DriveInfo[DriveInfo.GetDrives().Length];
            try
            {
                int count = 0;
                //загрузка дисков готовых к работе
                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    if (drive.IsReady)//диск готов к работе
                    {
                        driveInfos[count] = drive;
                        count++;
                    }
                }
                return driveInfos;
            }
            catch (Exception e) { ErrorWindow( e ); return driveInfos; }
        }

        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new( () => MessageBox.Show( e.Message, $"FileOverviewModel.{name}" ) );
            thread.SetApartmentState( ApartmentState.STA );
            thread.Start();
        }

    }
}
