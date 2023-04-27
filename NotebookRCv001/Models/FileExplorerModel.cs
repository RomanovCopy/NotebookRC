using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;
using NotebookRCv001.Helpers;
using NotebookRCv001.Converters;

namespace NotebookRCv001.Models
{
    internal class FileExplorerModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly HomeMenuFileViewModel homeMenuFileViewModel;
        private readonly HomeMenuEncryptionViewModel homeMenuEncryptionViewModel;
        /// <summary>
        /// разрешение на обновление содержимого 
        /// </summary>
        private bool permissionToUpdate { get; set; }

        internal ObservableCollection<string> Headers => language.HeadersFileOverview;

        internal ObservableCollection<string> ToolTips => language.ToolTipsFileOverview;

        internal Action<object> BehaviorReady
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }


        /// <summary>
        /// коллекция размеров колонок по горизонтали(Width)
        /// </summary>
        internal ObservableCollection<double> ListView_ColumnsWidth
        {
            get => listView_ColumnsWidth ??= new ObservableCollection<double>();
            set => SetProperty(ref listView_ColumnsWidth, value);
        }
        ObservableCollection<double> listView_ColumnsWidth;

        private Languages language => mainWindowViewModel.Language;

        /// <summary>
        /// открытый в окне каталог(для диска - null)
        /// </summary>
        internal DirectoryInfo CurrentDirectory { get => currentDirectory; set => SetProperty(ref currentDirectory, value); }
        private DirectoryInfo currentDirectory;
        /// <summary>
        /// коллекция доступных дисков
        /// </summary>
        internal ObservableCollection<DriveInfo> DriveInfos { get => driveInfos; set => SetProperty(ref driveInfos, value); }
        private ObservableCollection<DriveInfo> driveInfos;
        /// <summary>
        /// коллекция элементов из отображаемой директории
        /// </summary>
        internal ObservableCollection<DirectoryItem> CurrentDirectoryList
        {
            get => currentDirectoryList;
            set => SetProperty(ref currentDirectoryList, value);
        }
        private ObservableCollection<DirectoryItem> currentDirectoryList;
        /// <summary>
        /// коллекция размеров обложек
        /// </summary>
        internal ObservableCollection<int> CoverSizes { get => coverSizes; set => SetProperty(ref coverSizes, value); }
        private ObservableCollection<int> coverSizes;
        /// <summary>
        /// индекс выбранного элемента коллекции доступных дисков
        /// </summary>
        internal int SelectedIndexDrives { get => selectedIndexDrives; set => SetProperty(ref selectedIndexDrives, value); }
        private int selectedIndexDrives;
        /// <summary>
        /// индекс выбранного размера обложки в коллекции CoverSizes
        /// </summary>
        internal int CoverSizesIndex { get => coverSizesIndex; set => SetProperty(ref coverSizesIndex, value); }
        private int coverSizesIndex;
        /// <summary>
        /// полный путь к текущей дирректории
        /// </summary>
        internal string CurrentDirectoryFullName
        {
            get => currentDirectoryFullName;
            set => SetProperty(ref currentDirectoryFullName, value);
        }
        private string currentDirectoryFullName;
        /// <summary>
        /// флаг: отображение обложек
        /// </summary>
        internal bool IsCoverEnabled { get => isCoverEnabled; set => SetProperty(ref isCoverEnabled, value); }
        private bool isCoverEnabled;
        /// <summary>
        /// флаг: отображение в виде плиток
        /// </summary>
        internal bool IsTilesEnabled { get => isTilesEnabled; set => SetProperty(ref isTilesEnabled, value); }
        private bool isTilesEnabled;
        /// <summary>
        /// Возможные размеры иконки обложки(высота)
        /// </summary>
        internal ObservableCollection<int> IconSizes { get => iconSizes; set => SetProperty(ref iconSizes, value); }
        private ObservableCollection<int> iconSizes;
        /// <summary>
        /// индекс выбранного размера иконки
        /// </summary>
        internal int IconSizesIndex { get => iconSizesIndex; set => SetProperty(ref iconSizesIndex, value); }
        private int iconSizesIndex;
        /// <summary>
        /// текущая высота иконок задается для выравнивания
        /// </summary>
        internal int ImageHeight { get => imageHeight; set => SetProperty(ref imageHeight, value); }
        private int imageHeight;


        internal FileExplorerModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "Tooltips" });
            var home = (Views.Home)mainWindowViewModel.FrameList.Where((x) => x is Views.Home).FirstOrDefault();
            var menu = (MyControls.MenuHome)home.FindResource("menuhome");
            homeMenuFileViewModel = (HomeMenuFileViewModel)menu.FindResource("menufile");
            homeMenuEncryptionViewModel = (HomeMenuEncryptionViewModel)menu.FindResource("menuencryption");
            CoverSizes = new ObservableCollection<int>() { 16, 24, 32, 40, 48, 56, 64, 72, 80 };
            ImageHeight = 24;
            //устанавливаем размеры колонок
            //Properties.Settings.Default.FileOverview_ListViewColumnsWidth = null;//сброс размеров колонок
            if (Properties.Settings.Default.FileOverview_ListViewColumnsWidth == null)
                Properties.Settings.Default.FileOverview_ListViewColumnsWidth =
                    new System.Collections.Specialized.StringCollection() { "40", "20", "15", "15", "10" };
            ListView_ColumnsWidth.Clear();
            for (int i = 0; i < Properties.Settings.Default.FileOverview_ListViewColumnsWidth.Count; i++)
                ListView_ColumnsWidth.Add(double.Parse(Properties.Settings.Default.FileOverview_ListViewColumnsWidth[i]));
            permissionToUpdate = true;
        }


        /// <summary>
        /// обработка события MouseLeftButtonClick на имени каталога
        /// </summary>
        /// <param name="obj"> DirectoryInfo/FileInfo</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ListViewNameMouseLeftButtonDown(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ListViewNameMouseLeftButtonDown(object obj)
        {
            try
            {
                OpenFileDirectory(obj);
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// активация/дезактивация IsTilesEnabled
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_CheckedIsTilesEnabled(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_CheckedIsTilesEnabled(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// активация/дезактивация IsCoverEnabled
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_CheckedIsCover(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal async void Execute_CheckedIsCover(object obj)
        {
            try
            {
                if (IsCoverEnabled)
                {
                    await Task.Factory.StartNew(() => AddingIcons(ImageHeight));
                }
                else
                {
                    await Task.Factory.StartNew(() => AddingIcons(ImageHeight));
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// переход к родительскому элементу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClickToParentDirectory(object obj)
        {
            try
            {
                bool c = false;
                c = CurrentDirectory?.Parent != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal async void Execute_ClickToParentDirectory(object obj)
        {
            try
            {
                if (CurrentDirectory.Parent != null)
                {
                    await Task.Factory.StartNew(() =>
                    {
                        CurrentDirectoryList = GetCurrentDirectoryList(CurrentDirectory.Parent);
                        if (IsCoverEnabled)
                            AddingIcons(ImageHeight);
                    });
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// изменение выбора размера обложки
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_CoverSizesSelectionChanged(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_CoverSizesSelectionChanged(object obj)
        {
            try
            {
                if (obj is int height)
                {
                    ImageHeight = height;
                    Task.Factory.StartNew(() => { AddingIcons(height); });
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// выбор диска
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ComboBoxDrivesSelectionChanged(object obj)
        {
            try
            {
                bool c = false;
                c = obj is DriveInfo driveInfo && CurrentDirectoryFullName != driveInfo.Name;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ComboBoxDrivesSelectionChanged(object obj)
        {
            try
            {
                if (obj is DriveInfo driveInfo )
                {
                    string encryptionKey = homeMenuEncryptionViewModel.EncryptionKey;
                    CurrentDirectoryFullName = driveInfo.Name;
                    CurrentDirectoryList = new();
                    foreach (var folder in driveInfo.RootDirectory.EnumerateDirectories())
                        CurrentDirectoryList.Add(new DirectoryItem(folder, encryptionKey));
                    foreach (var file in driveInfo.RootDirectory.EnumerateFiles())
                        CurrentDirectoryList.Add(new DirectoryItem(file, encryptionKey));
                    if (IsCoverEnabled)
                        AddingIcons(ImageHeight);
                    CurrentDirectory = null;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// окончание загрузки коллекции размеров обложек (ComboBox)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_CoverSizesLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_CoverSizesLoaded(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// окончание загрузки коллекции доступных дисков(ComboBox)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ComboBoxDrivesLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ComboBoxDrivesLoaded(object obj)
        {
            try
            {//методо должен срабатывать только при первой загрузке
                if (permissionToUpdate)
                {
                    SelectedIndexDrives = 0;
                    if (CanExecute_ComboBoxDrivesSelectionChanged(DriveInfos[SelectedIndexDrives]))
                        Execute_ComboBoxDrivesSelectionChanged(DriveInfos[SelectedIndexDrives]);
                    CurrentDirectoryFullName = DriveInfos[SelectedIndexDrives].Name;
                    permissionToUpdate = false;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// окончание загрузки страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageLoaded(object obj)
        {
            try
            {//метод должен срабатывать только при первой загрузке
                if (obj is ColumnsWidthConverter convert && permissionToUpdate)
                {
                    convert.window = Application.Current.MainWindow;
                    OnPropertyChanged("ListView_ColumnsWidth");
                    UpdateDrives();
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// изменение размера страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageSizeChanged(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageSizeChanged(object obj)
        {
            try
            {
                OnPropertyChanged("ListView_ColumnsWidth");
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// закрытие страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageClose(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose(object obj)
        {
            try
            {
                if (obj is MyControls.FileExplorer exp && mainWindowViewModel.FrameListRemovePage.CanExecute(exp))
                {
                    mainWindowViewModel.FrameListRemovePage.Execute(exp);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// очистка страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageClear(object obj)
        {
            try
            {
                bool c = false;
                //c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClear(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// открытие файла/директории
        /// </summary>
        /// <param name="obj"></param>
        private async void OpenFileDirectory(object obj)
        {
            try
            {
                if (obj is DirectoryInfo dirInfo)
                {//выбран каталог
                    await Task.Factory.StartNew(() => CurrentDirectoryList = GetCurrentDirectoryList(dirInfo));
                    if (IsCoverEnabled)
                    {
                        await Task.Factory.StartNew(() => { AddingIcons(ImageHeight); });
                    }
                }
                else if (obj is FileInfo fileInfo)
                {//выбран файл
                    var player = mainWindowViewModel.FrameList.Where((x) => x is MyControls.MediaPlayer).FirstOrDefault();
                    if (player == null)
                    {
                        player = new MyControls.MediaPlayer();
                        mainWindowViewModel.FrameListAddPage.Execute(player);
                    }
                    var playerVM = (MediaPlayerViewModel)player.DataContext;
                    if (playerVM.SetContent.CanExecute(fileInfo.FullName))
                        playerVM.SetContent.Execute(fileInfo.FullName);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// дешифровка(если установлен ключ) и открытие файла в дефолтном приложении
        /// </summary>
        /// <param name="fileInfo">информация об открываемом файле (FileInfo)</param>
        /// <param name="newWindow">открыть файл в новом окне( True )</param>
        private async Task OpenAFileInTheDefaultApplication(FileInfo fileInfo, bool newWindow)
        {
            try
            {
                string path = fileInfo.FullName;
                string ext = fileInfo.Extension;
                using (var myProcess = new Process())
                {
                    if (!string.IsNullOrWhiteSpace(homeMenuEncryptionViewModel.EncryptionKey))
                    {
                        byte[] bytes = new byte[fileInfo.Length];
                        using (var fs = new FileStream(path, FileMode.OpenOrCreate))
                        {
                            await fs.ReadAsync(bytes, 0, bytes.Length);
                            bytes = Command_executors.Executors.Decrypt(bytes, homeMenuEncryptionViewModel.EncryptionKey);
                        }
                        path = $"{Environment.CurrentDirectory}/temp/temp{ext}";
                        using (var fs = new FileStream(path, FileMode.Create))
                        {
                            await fs.WriteAsync(bytes, 0, bytes.Length);
                        }
                    }
                    myProcess.StartInfo.UseShellExecute = true;
                    myProcess.StartInfo.CreateNoWindow = newWindow;
                    myProcess.StartInfo.FileName = path;
                    myProcess.Start();
                }
            }
            catch (Exception e) { ErrorWindow(e); }
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
                    DriveInfos.Add(info);
            }
            catch (Exception e) { ErrorWindow(e); }
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
            catch (Exception e) { ErrorWindow(e); return driveInfos; }
        }
        /// <summary>
        /// получение всех папок и файлов из заданного каталога
        /// </summary>
        /// <param name="directoryInfo">каталог</param>
        /// <returns>коллекция папок и файлов</returns>
        private ObservableCollection<DirectoryItem> GetCurrentDirectoryList(DirectoryInfo directoryInfo)
        {
            ObservableCollection<DirectoryItem> list = new();
            try
            {
                string key = homeMenuEncryptionViewModel.EncryptionKey;
                foreach (var folder in directoryInfo.GetDirectories())
                    list.Add(new DirectoryItem(folder, key));
                foreach (var file in directoryInfo.GetFiles())
                    list.Add(new DirectoryItem(file, key));
                CurrentDirectory = directoryInfo;
                CurrentDirectoryFullName = directoryInfo.FullName;
                return list;
            }
            catch (Exception e) { ErrorWindow(e); return CurrentDirectoryList; }
        }
        /// <summary>
        /// добавление Icons
        /// </summary>
        private void AddingIcons(int height)
        {
            try
            {
                BitmapImage bitmap = null;
                string path = null;
                foreach (var item in CurrentDirectoryList)
                {
                    bitmap = null;
                    item.IsCover = IsCoverEnabled;
                    if (item.IsFolder && item.Tag is DirectoryInfo dir)
                    {
                        FileInfo icon = null;
                        try
                        {//на случай, когда каталог закрыт для просмотра
                            icon = dir.GetFiles().Where((x) => x.Extension.ToLower() == ".jpg" ||
                            x.Extension.ToLower() == ".jpeg").FirstOrDefault();
                        }
                        catch { item.IsCover = false; continue; }
                        if (icon != null)
                        {
                            path = icon.FullName;
                            try
                            {
                                if (!string.IsNullOrWhiteSpace(homeMenuEncryptionViewModel.EncryptionKey))
                                {
                                    bitmap = Command_executors.Executors.ImageDecrypt(path, homeMenuEncryptionViewModel.EncryptionKey, height).Result;
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
                                    bitmap = Command_executors.Executors.ImageDecrypt(path, homeMenuEncryptionViewModel.EncryptionKey, height).Result;
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
                    if (bitmap != null && !string.IsNullOrWhiteSpace(path))
                    {
                        try
                        {
                            bitmap.BeginInit();
                            bitmap.CacheOption = BitmapCacheOption.OnLoad;
                            bitmap.BaseUri = bitmap.UriSource = new Uri(path);
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
                OnPropertyChanged("CurrentDirectoryList");
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        internal async Task<BitmapImage> RetrievingAnImageFromADirectory(DirectoryInfo dir, string imageName)
        {
            BitmapImage bitmap = null;
            try
            {
                string path = Path.Combine(dir.FullName, imageName);
                if (File.Exists(path))
                {
                    if (dir.GetFiles().Any((x) => x.Name == imageName))
                    {
                        using (FileStream fs = new(path, FileMode.Open))
                        {
                            if (!string.IsNullOrWhiteSpace(homeMenuEncryptionViewModel.EncryptionKey))
                                bitmap = await Command_executors.Executors.ImageDecrypt(path, homeMenuEncryptionViewModel.EncryptionKey, 24);
                            else
                            {
                                bitmap = new BitmapImage(new Uri(path));
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
            catch (Exception e) { ErrorWindow(e); return bitmap; }
        }

    }
}
