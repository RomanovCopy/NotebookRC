﻿using System;
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
using System.Security.AccessControl;
using System.Diagnostics;
using System.Resources;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Security.Principal;

namespace NotebookRCv001.Models
{

    internal class FileOverviewModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly HomeMenuFileViewModel homeMenuFileViewModel;
        private readonly HomeMenuEncryptionViewModel homeMenuEncryptionViewModel;
        private Languages language => mainWindowViewModel.Language;

        /// <summary>
        /// открытый в окне каталог(для диска - null)
        /// </summary>
        internal DirectoryInfo CurrentDirectory { get => currentDirectory; set => SetProperty( ref currentDirectory, value ); }
        private DirectoryInfo currentDirectory;
        /// <summary>
        /// полный путь к открытому в окне каталогу
        /// </summary>
        internal string CurrentDirectoryFullName { get => currentDirectoryFullName; set => SetProperty( ref currentDirectoryFullName, value ); }
        private string currentDirectoryFullName;

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
        internal ObservableCollection<DirectoryItem> CurrentDirectoryList
        {
            get => currentDirectoryList;
            set => SetProperty( ref currentDirectoryList, value );
        }
        private ObservableCollection<DirectoryItem> currentDirectoryList;
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
        /// <summary>
        /// Отображение обложек файлов и папок
        /// </summary>
        internal bool CoverEnabled { get => coverEnabled; set => SetProperty( ref coverEnabled, value ); }
        private bool coverEnabled;
        /// <summary>
        /// Возможные размеры иконки обложки(высота)
        /// </summary>
        internal ObservableCollection<int> IconSizes { get => iconSizes; set => SetProperty( ref iconSizes, value ); }
        private ObservableCollection<int> iconSizes;
        /// <summary>
        /// индекс выбранного размера иконки
        /// </summary>
        internal int IconSizesIndex { get => iconSizesIndex; set => SetProperty( ref iconSizesIndex, value ); }
        private int iconSizesIndex;
        /// <summary>
        /// текущая высота иконок задается для выравнивания
        /// </summary>
        internal int ImageHeight { get => imageHeight; set=>SetProperty(ref imageHeight, value); }
        private int imageHeight;


        internal FileOverviewModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += ( s, e ) => OnPropertyChanged( new string[] { "Headers", "ToolTips" } );
            var home = (Views.Home)mainWindowViewModel.FrameList.Where( ( x ) => x is Views.Home ).FirstOrDefault();
            var menu = (MyControls.MenuHome)home.FindResource( "menuhome" );
            homeMenuFileViewModel = (HomeMenuFileViewModel)menu.FindResource( "menufile" );
            homeMenuEncryptionViewModel = (HomeMenuEncryptionViewModel)menu.FindResource( "menuencryption" );
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
            IconSizes = new ObservableCollection<int>() { 16, 24, 32, 40, 48, 56, 64, 72 };
            //восстанавливаем состояние окна
            WindowState = Properties.Settings.Default.FileOverviewState;
            //устанавливаем размеры колонок
            //Properties.Settings.Default.FileOverview_ListViewColumnsWidth = null;
            if (Properties.Settings.Default.FileOverview_ListViewColumnsWidth == null)
                Properties.Settings.Default.FileOverview_ListViewColumnsWidth = new System.Collections.Specialized.StringCollection()
                { "40" ,"20" ,"15", "15" ,"10"  };
            ListView_ColumnsWidth.Clear();
            for (int i = 0; i < Properties.Settings.Default.FileOverview_ListViewColumnsWidth.Count; i++)
                ListView_ColumnsWidth.Add( double.Parse( Properties.Settings.Default.FileOverview_ListViewColumnsWidth[i] ) );
        }

        /// <summary>
        /// переход состояния IsCover в Checked
        /// </summary>
        /// <param name="obj">checked</param>
        /// <returns></returns>
        internal bool CanExecute_CheckedIsCover( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal async void Execute_CheckedIsCover( object obj )
        {
            try
            {
                if (CoverEnabled)
                {
                    await Task.Factory.StartNew( () => AddingIcons(ImageHeight) );
                }
                else
                {
                    await Task.Factory.StartNew( () => AddingIcons(ImageHeight) );
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
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
                UpdateDrives();
                if (CanExecute_ComboBoxSelectionChanged( DriveInfos[SelectedIndex] ))
                    Execute_ComboBoxSelectionChanged( DriveInfos[SelectedIndex] );
                CurrentDirectoryFullName = DriveInfos[SelectedIndex].Name;
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// изменение выбора диска (ComboBox)
        /// </summary>
        /// <param name="obj">DriveInfo</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ComboBoxSelectionChanged( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal async void Execute_ComboBoxSelectionChanged( object obj )
        {
            try
            {
                if (obj is DriveInfo driveInfo)
                {
                    await Task.Factory.StartNew( () =>
                    {
                        string encryptionKey = homeMenuEncryptionViewModel.EncryptionKey;
                        CurrentDirectoryFullName = driveInfo.Name;
                        CurrentDirectoryList = new();
                        foreach (var folder in driveInfo.RootDirectory.EnumerateDirectories())
                            CurrentDirectoryList.Add( new DirectoryItem( folder, encryptionKey ) );
                        foreach (var file in driveInfo.RootDirectory.EnumerateFiles())
                            CurrentDirectoryList.Add( new DirectoryItem( file, encryptionKey ) );
                        if (CoverEnabled)
                            AddingIcons(ImageHeight);
                        CurrentDirectory = null;
                    } );
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// окончание загрузки IconSizes
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_IconSizesLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_IconSizesLoaded( object obj )
        {
            try
            {
                ImageHeight = Properties.Settings.Default.FileOverviewIconSize;
                IconSizesIndex = IconSizes.IndexOf(ImageHeight);
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// изменение выбора размера иконки обложки
        /// </summary>
        /// <param name="obj">выбор int</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_IconSizesSelectionChanged( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_IconSizesSelectionChanged( object obj )
        {
            try
            {
                if(obj is int height)
                {
                    ImageHeight = height;
                    Task.Factory.StartNew(() => { AddingIcons(height); });
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// нажатие кнопки перемещения в родительскую директорию (Up)
        /// </summary>
        /// <param name="obj">null</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ToParentDirectory( object obj )
        {
            try
            {
                bool c = false;
                c = CurrentDirectory?.Parent != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal async void Execute_ToParentDirectory( object obj )
        {
            try
            {
                if (CurrentDirectory.Parent != null)
                {
                    await Task.Factory.StartNew( () =>
                    {
                        CurrentDirectoryList = GetCurrentDirectoryList( CurrentDirectory.Parent );
                        if (CoverEnabled)
                            AddingIcons(ImageHeight);
                    } );
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// выбор элемента
        /// </summary>
        /// <param name="obj">DirectoryInfo/FileInfo</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ListViewNameMouseLeftButtonDown( object obj )
        {
            try
            {
                bool c = false;
                c = obj is DirectoryInfo;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ListViewNameMouseLeftButtonDown( object obj )
        {
            try
            {
                OpenFileDirectory( obj );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// контекстное меню Open
        /// </summary>
        /// <param name="obj">DirectoryInfo/FileInfo</param>
        /// <returns></returns>
        internal bool CanExecute_ListViewNameContextMenuOpen( object obj )
        {
            try
            {
                bool c = false;
                c = obj is FileInfo;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ListViewNameContextMenuOpen( object obj )
        {
            try
            {
                if (obj is FileInfo fileInfo && homeMenuFileViewModel.SupportedFileExtensions.Any( ( x ) => x == fileInfo.Extension ))
                    homeMenuFileViewModel.OpenFile.Execute( fileInfo.FullName );
                else
                    OpenFileDirectory( obj );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// окончание загрузки ListView
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ListViewLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch(Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ListViewLoaded( object obj )
        {
            try
            {
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
                c = true;
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
                foreach (var width in ListView_ColumnsWidth)
                {
                    propert.FileOverview_ListViewColumnsWidth.Add( width.ToString() );
                }
                propert.FileOverviewIconSize = IconSizes[IconSizesIndex];
                Application.Current.MainWindow.Focus();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }



        /// <summary>
        /// открытие файла/директории
        /// </summary>
        /// <param name="obj"></param>
        private async void OpenFileDirectory( object obj )
        {
            try
            {
                if (obj is DirectoryInfo dirInfo)
                {//выбран каталог
                    await Task.Factory.StartNew( () => CurrentDirectoryList = GetCurrentDirectoryList( dirInfo ) );
                    if (CoverEnabled)
                    {
                        await Task.Factory.StartNew(() => { AddingIcons(ImageHeight); });
                    }
                }
                else if (obj is FileInfo fileInfo)
                {//выбран файл
                    var player = mainWindowViewModel.FrameList.Where( ( x ) => x is MyControls.MediaPlayer ).FirstOrDefault();
                    if (player == null)
                    {
                        player = new MyControls.MediaPlayer();
                        mainWindowViewModel.FrameListAddPage.Execute( player );
                    }
                    var playerVM = (MediaPlayerViewModel)player.DataContext;
                    if (playerVM.SetContent.CanExecute( fileInfo.FullName ))
                        playerVM.SetContent.Execute( fileInfo.FullName );
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// дешифровка(если установлен ключ) и открытие файла в дефолтном приложении
        /// </summary>
        /// <param name="fileInfo">информация об открываемом файле (FileInfo)</param>
        /// <param name="newWindow">открыть файл в новом окне( True )</param>
        private async Task OpenAFileInTheDefaultApplication( FileInfo fileInfo, bool newWindow )
        {
            try
            {
                string path = fileInfo.FullName;
                string ext = fileInfo.Extension;
                using (var myProcess = new Process())
                {
                    if (!string.IsNullOrWhiteSpace( homeMenuEncryptionViewModel.EncryptionKey ))
                    {
                        byte[] bytes = new byte[fileInfo.Length];
                        using (var fs = new FileStream( path, FileMode.OpenOrCreate ))
                        {
                            await fs.ReadAsync( bytes, 0, bytes.Length );
                            bytes = Command_executors.Executors.Decrypt( bytes, homeMenuEncryptionViewModel.EncryptionKey );
                        }
                        path = $"{Environment.CurrentDirectory}/temp/temp{ext}";
                        using (var fs = new FileStream( path, FileMode.Create ))
                        {
                            await fs.WriteAsync( bytes, 0, bytes.Length );
                        }
                    }
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.CreateNoWindow = newWindow;
                    myProcess.StartInfo.FileName = path;
                    myProcess.Start();
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
        /// <summary>
        /// получение всех папок и файлов из заданного каталога
        /// </summary>
        /// <param name="directoryInfo">каталог</param>
        /// <returns>коллекция папок и файлов</returns>
        private ObservableCollection<DirectoryItem> GetCurrentDirectoryList( DirectoryInfo directoryInfo )
        {
            ObservableCollection<DirectoryItem> list = new();
            try
            {
                string key = homeMenuEncryptionViewModel.EncryptionKey;
                foreach (var folder in directoryInfo.GetDirectories())
                    list.Add( new DirectoryItem( folder, key ) );
                foreach (var file in directoryInfo.GetFiles())
                    list.Add( new DirectoryItem( file, key ) );
                CurrentDirectory = directoryInfo;
                CurrentDirectoryFullName = directoryInfo.FullName;
                return list;
            }
            catch (Exception e) { ErrorWindow( e ); return CurrentDirectoryList; }
        }
        /// <summary>
        /// добавление Icons
        /// </summary>
        private void AddingIcons( int height )
        {
            try
            {
                BitmapImage bitmap = null;
                string path = null;
                foreach (var item in CurrentDirectoryList)
                {
                    bitmap = null;
                    item.IsCover = CoverEnabled;
                    if (item.IsFolder && item.Tag is DirectoryInfo dir)
                    {
                        FileInfo icon = null;
                        try
                        {//на случай, когда каталог закрыт для просмотра
                            icon = dir.GetFiles().Where( ( x ) => x.Extension.ToLower() == ".jpg" ||
                            x.Extension.ToLower() == ".jpeg" ).FirstOrDefault();
                        }
                        catch { item.IsCover = false; continue; }
                        if (icon != null)
                        {
                            path = icon.FullName;
                            try
                            {
                                if (!string.IsNullOrWhiteSpace( homeMenuEncryptionViewModel.EncryptionKey ))
                                {
                                    bitmap = Command_executors.Executors.ImageDecrypt( path, homeMenuEncryptionViewModel.EncryptionKey, height ).Result;
                                    if (bitmap == null)
                                        item.IsCover = false;
                                    else
                                        item.Icon = bitmap;
                                    continue;
                                }
                                else
                                {
                                    bitmap = new BitmapImage();
                                }
                            }
                            catch { item.IsCover = false; continue; }
                        }
                    }
                    else if (item.IsFile && item.Tag is FileInfo info)
                    {
                        path = info.FullName;
                        if (info.Extension.ToLower() == ".jpg" || info.Extension.ToLower() == ".jpeg")
                        {
                            try
                            {
                                if (homeMenuEncryptionViewModel.EncryptionKey != null)
                                {
                                    bitmap = Command_executors.Executors.ImageDecrypt( path, homeMenuEncryptionViewModel.EncryptionKey, height).Result;
                                    item.Icon = bitmap;
                                    continue;
                                }
                                else
                                {
                                    bitmap = new BitmapImage();
                                }
                            }
                            catch
                            {
                                item.IsCover = false;
                                continue;
                            }
                        }
                    }
                    else
                        continue;
                    if (bitmap != null && !string.IsNullOrWhiteSpace( path ))
                    {
                        try
                        {
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.BaseUri = bitmap.UriSource = new Uri( path );
                            bitmap.DecodePixelHeight = height;
                            bitmap.EndInit();
                            bitmap.Freeze();
                            item.Icon = bitmap;
                        }
                        catch { item.IsCover = false; continue; }
                    }
                    else
                    {
                        item.IsCover = false;
                    }
                }
                OnPropertyChanged( "CurrentDirectoryList" );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        internal async Task<BitmapImage> RetrievingAnImageFromADirectory( DirectoryInfo dir, string imageName )
        {
            BitmapImage bitmap = null;
            try
            {
                string path = Path.Combine( dir.FullName, imageName );
                if (File.Exists( path ))
                {
                    if (dir.GetFiles().Any( ( x ) => x.Name == imageName ))
                    {
                        using (FileStream fs = new( path, FileMode.Open ))
                        {
                            if (!string.IsNullOrWhiteSpace( homeMenuEncryptionViewModel.EncryptionKey ))
                                bitmap = await Command_executors.Executors.ImageDecrypt( path, homeMenuEncryptionViewModel.EncryptionKey, 24 );
                            else
                            {
                                bitmap = new BitmapImage( new Uri( path ) );
                                bitmap.BeginInit();
                                bitmap.DecodePixelHeight = 24;
                                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                                bitmap.EndInit();
                                bitmap.Freeze();
                            }
                        }
                    }
                }
                return bitmap;
            }
            catch (Exception e) { ErrorWindow( e ); return bitmap; }
        }

    }
}
