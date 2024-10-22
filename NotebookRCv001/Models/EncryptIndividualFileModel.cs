using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Security.Cryptography;
using System.Windows.Forms;
//using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Documents;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using System.Windows.Shapes;

namespace NotebookRCv001.Models
{
    internal class EncryptIndividualFileModel: ViewModelBase, IDisplayProgressTarget
    {
        private readonly ViewModels.MainWindowViewModel mainWindowViewModel;
        private readonly ViewModels.HomeViewModel homeViewModel;
        private readonly ViewModels.HomeMenuEncryptionViewModel homeMenuEncryptionViewModel;
        private Languages language => mainWindowViewModel.Language;
        /// <summary>
        /// применить к подпапкам
        /// </summary>
        private bool applyToSubfolders { get; set; }
        /// <summary>
        /// шифрование/дешифрование не ведется
        /// </summary>
        private bool isCompleted { get; set; }
        /// <summary>
        /// отмена шифрования/дешифрвания
        /// </summary>
        private CancellationTokenSource cancellationTokenSource { get; set; }
        /// <summary>
        /// коллекция путей к добавленным файлам
        /// </summary>
        internal ObservableCollection<string> AddedFiles { get => addedFiles; set => SetProperty(ref addedFiles, value); }
        private ObservableCollection<string> addedFiles;
        /// <summary>
        /// коллекция путей к добавленным папкам
        /// </summary>
        internal ObservableCollection<string> AddedFolders { get => addedFolders; set => SetProperty(ref addedFolders, value); }
        private ObservableCollection<string> addedFolders;

        internal ObservableCollection<string> Headers => language.EncryptIndividualFile;

        internal ObservableCollection<string> ToolTips => language.ToolTipsEncryptIndividualFile;



        internal Action<object> BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        Action<object> behaviorReady;


        /// <summary>
        /// Путь к последнему открытому для шифрования файлу
        /// </summary>
        private string EncryptPathtoLastFileOpen
        {
            get => Properties.Settings.Default.EncryptPathtoLastFileOpen ??= " ";
            set => Properties.Settings.Default.EncryptPathtoLastFileOpen = value;
        }
        /// <summary>
        /// Путь к последнему сохраненному зашифрованному файлу
        /// </summary>
        private string EncryptPathtoLastFileSave
        {
            get => Properties.Settings.Default.EncryptPathtoLastFileSave ??= " ";
            set => Properties.Settings.Default.EncryptPathtoLastFileSave = value;
        }
        /// <summary>
        /// Путь к последней открытой для шифрования директории 
        /// </summary>
        private string EncryptPathtoLastDirectoryOpen
        {
            get => Properties.Settings.Default.EncryptPathtoLastDirectoryOpen ??= " ";
            set => Properties.Settings.Default.EncryptPathtoLastDirectoryOpen = value;
        }
        /// <summary>
        /// Путь к последней сохраненной зашифрованной директории
        /// </summary>
        private string EncryptPathtoLastDirectorySave
        {
            get => Properties.Settings.Default.EncryptPathtoLastDirectorySave ??= " ";
            set => Properties.Settings.Default.EncryptPathtoLastDirectorySave = value;
        }


        /// <summary>
        /// путь к открываемому файлу
        /// </summary>
        internal string PathToOpenFile { get => pathToOpenFile; set => SetProperty(ref pathToOpenFile, value); }
        private string pathToOpenFile;
        /// <summary>
        /// имя открываемого для шифрования файла
        /// </summary>
        internal string NameOpenFile { get => nameOpenFile; set => SetProperty(ref nameOpenFile, value); }
        private string nameOpenFile;
        /// <summary>
        /// путь к сохраняемому файлу
        /// </summary>
        internal string PathToSaveFile { get => pathToSaveFile; set => SetProperty(ref pathToSaveFile, value); }
        private string pathToSaveFile;
        /// <summary>
        /// имя сохраняемого файла
        /// </summary>
        internal string NameSaveFile { get => nameSaveFile; set => SetProperty(ref nameSaveFile, value); }
        private string nameSaveFile;
        /// <summary>
        /// путь к открываемой директории
        /// </summary>
        internal string PathToOpenDirectory { get => pathToOpenDirectory; set => SetProperty(ref pathToOpenDirectory, value); }
        private string pathToOpenDirectory;
        /// <summary>
        /// имя открываемого для шифрования/дешифрования каталога
        /// </summary>
        internal string NameOpenDirectory { get => nameOpenDirectory; set => SetProperty(ref nameOpenDirectory, value); }
        private string nameOpenDirectory;
        /// <summary>
        /// путь к сохраняемой директории
        /// </summary>
        internal string PathToSaveDirectory { get => pathToSaveDirectory; set => SetProperty(ref pathToSaveDirectory, value); }
        private string pathToSaveDirectory;
        /// <summary>
        /// имя директории для сохранения зашфрованного/дешифрованного каталога
        /// </summary>
        internal string NameSaveDirectory { get => nameSaveDirectory; set => SetProperty(ref nameSaveDirectory, value); }
        private string nameSaveDirectory;
        /// <summary>
        /// прогресс шифрования файла/каталога
        /// </summary>
        public double ProgressValue { get => progressValue; set => SetProperty(ref progressValue, value); }
        private double progressValue;




        internal EncryptIndividualFileModel()
        {
            mainWindowViewModel = (ViewModels.MainWindowViewModel)System.Windows.Application.Current.MainWindow.DataContext;
            var home = (Views.Home)mainWindowViewModel.FrameList.Where((x) => x is Views.Home).FirstOrDefault();
            homeViewModel = (ViewModels.HomeViewModel)home.DataContext;
            var menu = (MyControls.MenuHome)home.FindResource("menuhome");
            homeMenuEncryptionViewModel = (ViewModels.HomeMenuEncryptionViewModel)menu.FindResource("menuencryption");
            string[] ex = new[] { "Headers", "ToolTips" };
            language.PropertyChanged += (s, e) => OnPropertyChanged(ex);
            isCompleted = true;
            applyToSubfolders = false;
        }



        /// <summary>
        /// выбор пути к открываемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectOpenFile(object obj)
        {
            try
            {
                bool c = false;
                c = string.IsNullOrWhiteSpace(PathToOpenDirectory) && string.IsNullOrWhiteSpace(PathToSaveDirectory) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectOpenFile(object obj)
        {
            try
            {
                var initionalDirectory = "";
                if(!string.IsNullOrWhiteSpace(EncryptPathtoLastFileOpen) && File.Exists(EncryptPathtoLastFileOpen))
                    initionalDirectory = new FileInfo(EncryptPathtoLastFileOpen).DirectoryName;
                var path = Command_executors.Executors.OpenFileDialog(Headers[9], initionalDirectory, "", "");
                PathToOpenFile = string.IsNullOrWhiteSpace(path) ? PathToOpenFile : path;
                if(!string.IsNullOrWhiteSpace(PathToOpenFile))
                {
                    EncryptPathtoLastFileOpen = PathToOpenFile;
                    NameOpenFile = new FileInfo(PathToOpenFile).Name;
                }
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// очистка пути к открываемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearOpenFile(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace(PathToOpenFile) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClearOpenFile(object obj)
        {
            try
            {
                PathToOpenFile = NameOpenFile = "";
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// выбор пути к сохраняемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectSaveFile(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace(PathToOpenFile) && File.Exists(PathToOpenFile) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectSaveFile(object obj)
        {
            try
            {
                var initionalDirectory = "";
                var fileName = new FileInfo(PathToOpenFile).Name;
                if(!string.IsNullOrWhiteSpace(EncryptPathtoLastFileSave) && File.Exists(EncryptPathtoLastFileSave))
                    initionalDirectory = new FileInfo(EncryptPathtoLastFileSave).DirectoryName;
                var path = Command_executors.Executors.SaveFileDialog(Headers[10], initionalDirectory, "", "", fileName);
                PathToSaveFile = string.IsNullOrWhiteSpace(path) ? PathToSaveFile : path;
                if(!string.IsNullOrWhiteSpace(PathToSaveFile))
                {
                    EncryptPathtoLastFileSave = PathToSaveFile;
                    NameSaveFile = new FileInfo(PathToSaveFile).Name;
                }
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// очистка пути к сохраняемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearSaveFile(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace(PathToSaveFile) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClearSaveFile(object obj)
        {
            try
            {
                PathToSaveFile = NameSaveFile = "";
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// выбор пути к открываемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectOpenDirectory(object obj)
        {
            try
            {
                bool c = false;
                c = string.IsNullOrWhiteSpace(PathToOpenFile) && string.IsNullOrWhiteSpace(PathToSaveFile) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectOpenDirectory(object obj)
        {
            try
            {
                var initionalDirectory = "";
                if(!string.IsNullOrWhiteSpace(EncryptPathtoLastDirectoryOpen) && Directory.Exists(EncryptPathtoLastDirectoryOpen))
                    initionalDirectory = new DirectoryInfo(EncryptPathtoLastDirectoryOpen).FullName;
                var path = Command_executors.Executors.FolderBrowserDialog("", initionalDirectory);
                PathToOpenDirectory = string.IsNullOrWhiteSpace(path) ? PathToOpenDirectory : path;
                if(!string.IsNullOrWhiteSpace(PathToOpenDirectory))
                {
                    EncryptPathtoLastDirectoryOpen = PathToOpenDirectory;
                    NameOpenDirectory = new DirectoryInfo(PathToOpenDirectory).Name;
                }
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// очистка пути к открываемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearOpenDirectory(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace(PathToOpenDirectory) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClearOpenDirectory(object obj)
        {
            try
            {
                PathToOpenDirectory = NameOpenDirectory = "";
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// выбор пути к сохраняемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectSaveDirectory(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace(PathToOpenDirectory) && Directory.Exists(PathToOpenDirectory) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectSaveDirectory(object obj)
        {
            try
            {
                var initionalDirectory = "";
                if(!string.IsNullOrWhiteSpace(EncryptPathtoLastDirectorySave) && Directory.Exists(EncryptPathtoLastDirectorySave))
                    initionalDirectory = new DirectoryInfo(EncryptPathtoLastDirectorySave).FullName;
                var path = Command_executors.Executors.FolderBrowserDialog("", initionalDirectory);
                PathToSaveDirectory = string.IsNullOrWhiteSpace(path) ? PathToSaveDirectory : path;
                if(!string.IsNullOrWhiteSpace(PathToSaveDirectory))
                {
                    EncryptPathtoLastDirectorySave = PathToSaveDirectory;
                    NameSaveDirectory = new DirectoryInfo(PathToSaveDirectory).Name;
                }
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// очистка пути к сохраняемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearSaveDirectory(object obj)
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace(PathToSaveDirectory) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClearSaveDirectory(object obj)
        {
            try
            {
                PathToSaveDirectory = NameSaveDirectory = "";
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// шифрование и сохранение выбранного файла/каталога
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClickButtonEncrypt(object obj)
        {
            try
            {
                bool c = false;
                bool a = !string.IsNullOrWhiteSpace(PathToOpenFile) && !string.IsNullOrWhiteSpace(PathToSaveFile);
                bool b = !string.IsNullOrWhiteSpace(PathToOpenDirectory) && !string.IsNullOrWhiteSpace(PathToSaveDirectory);
                c = (a || b) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickButtonEncrypt(object obj)
        {
            try
            {
                isCompleted = false;
                var progress = new Views.DisplayProgress();
                var progressVM = (ViewModels.DisplayProgressViewModel)progress.DataContext;
                var key = homeMenuEncryptionViewModel.EncryptionKey;
                progressVM.Target = this;
                PropertyChanged += (s, e) => progressVM.OnPropertyChanged(e.PropertyName);
                ProgressValue = 0;
                cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;
                token.Register(() => { CleanupWhenCancelingAnOperation(); });
                AddedFolders = new ObservableCollection<string>();
                AddedFiles = new ObservableCollection<string>();
                Task.Factory.StartNew(async () =>
                {
                    Thread.Sleep(500);
                    if(!string.IsNullOrWhiteSpace(PathToOpenFile) && !string.IsNullOrWhiteSpace(PathToSaveFile))
                    {//шифрование файла
                        ProgressValue = 50;
                        await Command_executors.Executors.EncryptFromStream(File.OpenRead(PathToOpenFile), PathToSaveFile, key);
                    } else if(!string.IsNullOrWhiteSpace(PathToOpenDirectory) && !string.IsNullOrWhiteSpace(PathToSaveDirectory))
                    {//щифрование каталога
                        DirectoryInfo info = new DirectoryInfo(PathToOpenDirectory);
                        int total = TotalNumberOfFilesInTheDirectory(info);
                        var path = Path.Combine(PathToSaveDirectory, info.Name);
                        if(!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        if(Directory.Exists(PathToOpenDirectory))
                        {
                            if(!applyToSubfolders)
                            {//шифрование без подкаталогов и applyToSubfolders == False
                                EncryptWithoutSubfolders(info, path, key, token);
                            } else
                            {
                                if(info.GetFiles().Length == total)
                                {//шифрование когда все файлы находятся в данном каталоге и applyToSubfolders == True
                                    EncryptWithoutSubfolders(info, path, key, token);
                                } else
                                {//шифрование когда файлы находятся в разных подкаталогах и applyToSubfolders == True
                                    EncryptionWithSubfolders(info, path, key, total, token);
                                }
                            }
                        }
                    }
                    if(!token.IsCancellationRequested)
                        Closure();
                });
                progress.Show();
            } catch(Exception e) { ErrorWindow(e); isCompleted = true; }
        }

        /// <summary>
        /// отмена шифрования/дешифрования
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClickButtonCancel(object obj)
        {
            try
            {
                bool c = false;
                c = !isCompleted && cancellationTokenSource != null;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickButtonCancel(object obj)
        {
            try
            {
                cancellationTokenSource.Cancel();
            } catch(Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// деифрование и сохранение выбранного файла/каталога
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClickButtonDecrypt(object obj)
        {
            try
            {
                bool c = false;
                bool a = !string.IsNullOrWhiteSpace(PathToOpenFile) && !string.IsNullOrWhiteSpace(PathToSaveFile);
                bool b = !string.IsNullOrWhiteSpace(PathToOpenDirectory) && !string.IsNullOrWhiteSpace(PathToSaveDirectory);
                c = (a || b) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClickButtonDecrypt(object obj)
        {
            try
            {
                isCompleted = false;
                var progress = new Views.DisplayProgress();
                var progressVM = (ViewModels.DisplayProgressViewModel)progress.DataContext;
                var key = homeMenuEncryptionViewModel.EncryptionKey;
                progressVM.Target = this;
                PropertyChanged += (s, e) => progressVM.OnPropertyChanged(e.PropertyName);
                ProgressValue = 0;
                cancellationTokenSource = new CancellationTokenSource();
                var token = cancellationTokenSource.Token;
                token.Register(() => { CleanupWhenCancelingAnOperation(); });
                AddedFiles = new ObservableCollection<string>();
                AddedFolders = new ObservableCollection<string>();
                Task.Factory.StartNew(async () =>
                {
                    Thread.Sleep(500);
                    if(!string.IsNullOrWhiteSpace(PathToOpenFile) && !string.IsNullOrWhiteSpace(PathToSaveFile))
                    {//дешифрование файла
                        ProgressValue = 50;
                        await Command_executors.Executors.DecryptFromStream(File.OpenRead(PathToOpenFile), PathToSaveFile, key);
                    } else if(!string.IsNullOrWhiteSpace(PathToOpenDirectory) && !string.IsNullOrWhiteSpace(PathToSaveDirectory))
                    {//дещифрование каталога
                        DirectoryInfo info = new DirectoryInfo(PathToOpenDirectory);
                        int total = TotalNumberOfFilesInTheDirectory(info);
                        var path = Path.Combine(PathToSaveDirectory, info.Name);
                        if(!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        if(Directory.Exists(PathToOpenDirectory))
                        {
                            if(!applyToSubfolders)
                            {//дешифрование без подкаталогов и applyToSubfolders == False
                                DecryptWithoutSubfolders(info, path, key, token);
                            } else
                            {
                                if(info.GetFiles().Length == total)
                                {//дешифрование когда все файлы находятся в данном каталоге и applyToSubfolders == True
                                    DecryptWithoutSubfolders(info, path, key, token);
                                } else
                                {//дешифрование когда файлы находятся в разных подкаталогах и applyToSubfolders == True
                                    DecryptionWithSubfolders(info, path, key, total, token);
                                }
                            }
                        }
                    }
                    if(!token.IsCancellationRequested)
                        Closure();
                });
                progress.Show();
            } catch(Exception e) { ProgressValue = 100; ErrorWindow(e); isCompleted = true; }
        }

        /// <summary>
        /// применить ко всем подпапкам
        /// </summary>
        /// <param name="obj">IsChecked</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SubfoldersCheckBox(object obj)
        {
            try
            {
                bool c = false;
                c = string.IsNullOrWhiteSpace(PathToOpenFile) && string.IsNullOrWhiteSpace(PathToSaveFile) &&
                    !string.IsNullOrWhiteSpace(PathToOpenDirectory) && !string.IsNullOrWhiteSpace(PathToSaveDirectory) &&
                    isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SubfoldersCheckBox(object obj)
        {
            try
            {
                if(obj is bool check)
                {
                    applyToSubfolders = check;
                }
            } catch(Exception e) { ErrorWindow(e); }
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
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageLoaded(object obj)
        {
            try
            {
            } catch(Exception e) { ErrorWindow(e); }
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
                c = obj != null && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose(object obj)
        {
            try
            {
                if(mainWindowViewModel.FrameListRemovePage.CanExecute(obj))
                    mainWindowViewModel.FrameListRemovePage.Execute(obj);
            } catch(Exception e) { ErrorWindow(e); }
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
                bool a = !(string.IsNullOrWhiteSpace(PathToOpenFile) && string.IsNullOrWhiteSpace(PathToSaveFile));
                bool b = !(string.IsNullOrWhiteSpace(PathToOpenDirectory) && string.IsNullOrWhiteSpace(PathToSaveDirectory));
                c = (a || b) && isCompleted;
                return c;
            } catch(Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClear(object obj)
        {
            try
            {
                PathToOpenFile = NameOpenFile = string.Empty;
                PathToSaveFile = NameSaveFile = string.Empty;
                PathToOpenDirectory = NameOpenDirectory = string.Empty;
                PathToSaveDirectory = NameSaveDirectory = string.Empty;
            } catch(Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// шифрование каталога без подкаталогов
        /// </summary>
        /// <param name="info">информация о каталоге(DirectoryInfo)</param>
        /// <param name="pathToSave">путь к катлогу для сохранения зашифрованных файлов</param>
        /// <param name="key">ключ шифрования</param>
        /// <returns>колличество зашифрованных файлов</returns>
        private int EncryptWithoutSubfolders(DirectoryInfo info, string pathToSave, string key, CancellationToken token)
        {
            try
            {
                double total = (double)info.GetFiles().Length;
                double count = 0;
                foreach(var finfo in info.GetFiles())
                {
                    var path = Path.Combine(pathToSave, finfo.Name);
                    Command_executors.Executors.EncryptFromStream(File.OpenRead(finfo.FullName), path, key);
                    count++;
                    ProgressValue = count / total * 100.0;
                    AddedFiles.Add(path);
                    if(token.IsCancellationRequested)
                        break;
                }
                return (int)count;
            } catch(Exception e) { ErrorWindow(e); return -1; }
        }
        /// <summary>
        /// шифрование каталога с подкаталогами
        /// </summary>
        /// <param name="info">информация о каталоге(DirectoryInfo)</param>
        /// <param name="pathToSave">путь к катлогу для сохранения зашифрованных файлов</param>
        /// <param name="key">ключ шифрования</param>
        /// <param name="total">общее колличество файлов во всех каталогах</param>
        /// <returns>колличество зашифрованных файлов</returns>
        private int EncryptionWithSubfolders(DirectoryInfo info, string pathToSave, string key, int total, CancellationToken token)
        {
            try
            {
                int count = 0;
                foreach(var finfo in info.GetFiles())
                {//шифрование файлов в каталоге
                    if(finfo != null)
                    {
                        var path = Path.Combine(pathToSave, finfo.Name);
                        Command_executors.Executors.EncryptFromStream(File.OpenRead(finfo.FullName), path, key);
                        count++;
                        AddedFiles.Add(path);
                    }
                    if(token.IsCancellationRequested)
                        break;
                }
                foreach(var dinfo in info.GetDirectories())
                {//переход к следующему подкаталогу
                    string newPath = Path.Combine(pathToSave, dinfo.Name);
                    Directory.CreateDirectory(newPath);
                    count += EncryptionWithSubfolders(dinfo, newPath, key, total, token);
                    ProgressValue = (double)count / (double)total * 100;
                    AddedFolders.Add(newPath);
                    if(token.IsCancellationRequested)
                        break;
                }
                return count;
            } catch(Exception e) { ErrorWindow(e); return -1; }
        }
        /// <summary>
        /// дешифрование каталога без подкаталогов
        /// </summary>
        /// <param name="info">информация о каталоге(DirectoryInfo)</param>
        /// <param name="pathToSave">путь к катлогу для сохранения дешифрованных файлов</param>
        /// <param name="key">ключ шифрования</param>
        /// <returns>колличество дешифрованных файлов</returns>
        private int DecryptWithoutSubfolders(DirectoryInfo info, string pathToSave, string key, CancellationToken token)
        {
            try
            {
                double total = (double)info.GetFiles().Length;
                double count = 0;
                foreach(var finfo in info.GetFiles())
                {
                    var path = Path.Combine(pathToSave, finfo.Name);
                    Command_executors.Executors.DecryptFromStream(File.OpenRead(finfo.FullName), path, key);
                    count++;
                    ProgressValue = count / total * 100.0;
                    AddedFiles.Add(path);
                    if(token.IsCancellationRequested)
                        break;
                }
                return (int)count;
            } catch(Exception e) { ErrorWindow(e); return -1; }
        }
        /// <summary>
        /// дешифрование каталога с подкаталогами
        /// </summary>
        /// <param name="info">информация о каталоге(DirectoryInfo)</param>
        /// <param name="pathToSave">путь к катлогу для сохранения дешифрованных файлов</param>
        /// <param name="key">ключ шифрования</param>
        /// <param name="total">общее колличество файлов во всех каталогах</param>
        /// <returns>колличество дешифрованных файлов</returns>
        private int DecryptionWithSubfolders(DirectoryInfo info, string pathToSave, string key, int total, CancellationToken token)
        {
            try
            {
                int count = 0;
                foreach(var finfo in info.GetFiles())
                {//дешифрование католога
                    var path = Path.Combine(pathToSave, finfo.Name);
                    Command_executors.Executors.DecryptFromStream(File.OpenRead(finfo.FullName), path, key);
                    count++;
                    AddedFiles.Add(path);
                    if(token.IsCancellationRequested)
                        break;
                }
                foreach(var dinfo in info.GetDirectories())
                {//переход к очередному подкаталогу
                    string newPath = Path.Combine(pathToSave, dinfo.Name);
                    Directory.CreateDirectory(newPath);
                    count += DecryptionWithSubfolders(dinfo, newPath, key, total, token);
                    ProgressValue = (double)count / (double)total * 100;
                    AddedFolders.Add(newPath);
                    if(token.IsCancellationRequested)
                        break;
                }
                return count;
            } catch(Exception e) { ErrorWindow(e); return -1; }
        }
        /// <summary>
        /// подсчет общего колличества файлов в данной директории
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int TotalNumberOfFilesInTheDirectory(DirectoryInfo info)
        {
            try
            {
                int count = 0;
                foreach(var finfo in info.GetFiles())
                    count++;
                foreach(var dinfo in info.GetDirectories())
                    count += TotalNumberOfFilesInTheDirectory(dinfo);
                return count;
            } catch(Exception e) { ErrorWindow(e); return -1; }
        }
        /// <summary>
        /// очистка при отмене операции
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void CleanupWhenCancelingAnOperation()
        {
            try
            {
                while(AddedFiles.Count > 0)
                {
                    var path = AddedFiles.FirstOrDefault();
                    var finfo = new FileInfo(path);
                    if(finfo.Exists)
                        finfo.Delete();
                    AddedFiles.Remove(path);
                }
                while(AddedFolders.Count > 0)
                {
                    var path = AddedFolders.FirstOrDefault();
                    var dinfo = new DirectoryInfo(path);
                    if(dinfo.Exists)
                        dinfo.Delete();
                    AddedFolders.Remove(path);
                }
                Closure();
            } catch(Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// завершение работы, приведение в исходное положение
        /// </summary>
        private void Closure()
        {
            try
            {
                ProgressValue = 100;
                AddedFiles = null;
                AddedFolders = null;
                cancellationTokenSource?.Dispose();
                isCompleted = true;
                PathToOpenFile = string.Empty;
                PathToSaveFile = string.Empty;
                PathToOpenDirectory = string.Empty;
                PathToSaveDirectory = string.Empty;
                NameOpenDirectory = string.Empty;
                NameSaveDirectory = string.Empty;
                NameOpenFile = string.Empty;
                NameSaveFile = string.Empty;
                //applyToSubfolders = false;
            } catch(Exception e) { ErrorWindow(e); }
        }


        #region ************* NON ***********************

        /// <summary>
        /// расшифровка каталога текстовых файлов старого образца(не используется)
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void ReencryptionOfLineFiles()
        {
            var decrypter = homeMenuEncryptionViewModel.HomeMenuEncryptionModel;
            string[] files = Directory.GetFiles(PathToOpenDirectory);
            foreach(var file in files)
            {
                var name = new FileInfo(file).Name;
                var pathToSave = System.IO.Path.Combine(PathToSaveDirectory, name);
                string text = null;
                using(StreamReader reader = new StreamReader(file))
                {
                    try
                    {
                        text = decrypter.Decryption(reader.ReadToEnd(), homeMenuEncryptionViewModel.EncryptionKey);
                    } catch { throw new Exception(mainWindowViewModel.Language.MessagesMyMessages[1]); }
                }
                using(StreamWriter writer = new StreamWriter(pathToSave))
                {
                    if(!string.IsNullOrEmpty(text))
                        writer.Write(text);
                }
            }
        }
        /// <summary>
        /// шифрование файла
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <param name="key">ключ шифрования</param>
        /// <returns>преобразованный в соответсвии с ключом массив байт</returns>
        private byte[] FileEncrypt(string path, string key)
        {
            byte[] result = new byte[0];
            try
            {
                using(FileStream fs = new FileStream(path, FileMode.Open))
                {
                    result = new byte[fs.Length];
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(result, 0, result.Length);
                    var crypt = Command_executors.Executors.Encrypt(result, key);
                    if(crypt is byte[] array)
                    {
                        result = new byte[array.Length];
                        array.CopyTo(result, 0);
                    }
                }
                return result;
            } catch(Exception e) { ErrorWindow(e); return result; }
        }
        /// <summary>
        /// дешифрование файла
        /// </summary>
        /// <param name="path">путь к зашифрованному файлу</param>
        /// <param name="key">ключ шифрования</param>
        /// <returns>преобразованный в соответсвии с ключом массив байт</returns>
        private byte[] FileDecrypt(string path, string key)
        {
            byte[] result = new byte[0];
            try
            {
                using(FileStream fs = new FileStream(path, FileMode.Open))
                {
                    result = new byte[fs.Length];
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.Read(result, 0, result.Length);
                    result = Command_executors.Executors.Decrypt(result, key);
                }
                return result;
            } catch(Exception e) { ErrorWindow(e); return result; }
        }


        #endregion


    }
}
