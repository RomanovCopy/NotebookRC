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

namespace NotebookRCv001.Models
{
    internal class EncryptIndividualFileModel : ViewModelBase
    {
        private readonly ViewModels.MainWindowViewModel mainWindowViewModel;
        private readonly ViewModels.HomeViewModel homeViewModel;
        private readonly ViewModels.HomeMenuEncryptionViewModel homeMenuEncryptionViewModel;
        private Languages language => mainWindowViewModel.Language;

        internal ObservableCollection<string> Headers => language.EncryptIndividualFile;

        internal ObservableCollection<string> ToolTips => language.ToolTipsEncryptIndividualFile;

        internal Action BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        Action behaviorReady;


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
        internal string PathToOpenFile { get => pathToOpenFile; set => SetProperty( ref pathToOpenFile, value); }
        private string pathToOpenFile;
        /// <summary>
        /// имя открываемого для шифрования файла
        /// </summary>
        internal string NameOpenFile { get => nameOpenFile; set => SetProperty( ref nameOpenFile, value ); }
        private string nameOpenFile;
        /// <summary>
        /// путь к сохраняемому файлу
        /// </summary>
        internal string PathToSaveFile { get => pathToSaveFile; set => SetProperty( ref pathToSaveFile, value ); }
        private string pathToSaveFile;
        /// <summary>
        /// имя сохраняемого файла
        /// </summary>
        internal string NameSaveFile { get => nameSaveFile; set => SetProperty( ref nameSaveFile, value ); }
        private string nameSaveFile;
        /// <summary>
        /// путь к открываемой директории
        /// </summary>
        internal string PathToOpenDirectory { get => pathToOpenDirectory; set => SetProperty( ref pathToOpenDirectory, value ); }
        private string pathToOpenDirectory;
        /// <summary>
        /// имя открываемого для шифрования/дешифрования каталога
        /// </summary>
        internal string NameOpenDirectory { get => nameOpenDirectory; set => SetProperty( ref nameOpenDirectory, value ); }
        private string nameOpenDirectory;
        /// <summary>
        /// путь к сохраняемой директории
        /// </summary>
        internal string PathToSaveDirectory { get => pathToSaveDirectory; set => SetProperty( ref pathToSaveDirectory, value ); }
        private string pathToSaveDirectory;
        /// <summary>
        /// имя директории для сохранения зашфрованного/дешифрованного каталога
        /// </summary>
        internal string NameSaveDirectory { get => nameSaveDirectory; set => SetProperty( ref nameSaveDirectory, value ); }
        private string nameSaveDirectory;


        internal EncryptIndividualFileModel()
        {
            mainWindowViewModel = (ViewModels.MainWindowViewModel)Application.Current.MainWindow.DataContext;
            var home = (Views.Home)mainWindowViewModel.FrameList.Where( ( x ) => x is Views.Home ).FirstOrDefault();
            homeViewModel = (ViewModels.HomeViewModel)home.DataContext;
            var menu = (MyControls.MenuHome)home.FindResource( "menuhome" );
            homeMenuEncryptionViewModel = (ViewModels.HomeMenuEncryptionViewModel)menu.FindResource( "menuencryption" );
            string[] ex = new[] { "Headers", "ToolTips" };
            language.PropertyChanged += ( s, e ) => OnPropertyChanged( ex );
        }



        /// <summary>
        /// выбор пути к открываемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectOpenFile( object obj )
        {
            try
            {
                bool c = false;
                c = string.IsNullOrWhiteSpace( PathToOpenDirectory ) && string.IsNullOrWhiteSpace( PathToSaveDirectory );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SelectOpenFile( object obj )
        {
            try
            {
                var initionalDirectory = "";
                if (!string.IsNullOrWhiteSpace( EncryptPathtoLastFileOpen ) && File.Exists( EncryptPathtoLastFileOpen ))
                    initionalDirectory = new FileInfo( EncryptPathtoLastFileOpen ).DirectoryName;
                var path = Command_executors.Executors.OpenFileDialog( Headers[9], initionalDirectory, "", "" );
                PathToOpenFile = string.IsNullOrWhiteSpace( path ) ? PathToOpenFile : path;
                if (!string.IsNullOrWhiteSpace( PathToOpenFile ))
                {
                    EncryptPathtoLastFileOpen = PathToOpenFile;
                    NameOpenFile = new FileInfo( PathToOpenFile ).Name;
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// очистка пути к открываемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearOpenFile( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( PathToOpenFile );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ClearOpenFile( object obj )
        {
            try
            {
                PathToOpenFile = NameOpenFile = "";
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// выбор пути к сохраняемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectSaveFile( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( PathToOpenFile ) && File.Exists( PathToOpenFile );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SelectSaveFile( object obj )
        {
            try
            {
                var initionalDirectory = "";
                if (!string.IsNullOrWhiteSpace( EncryptPathtoLastFileSave ) && File.Exists( EncryptPathtoLastFileSave ))
                    initionalDirectory = new FileInfo( EncryptPathtoLastFileSave ).DirectoryName;
                var path = Command_executors.Executors.SaveFileDialog( Headers[10], initionalDirectory, "", "" );
                PathToSaveFile = string.IsNullOrWhiteSpace( path ) ? PathToSaveFile : path;
                if (!string.IsNullOrWhiteSpace( PathToSaveFile ))
                {
                    EncryptPathtoLastFileSave = PathToSaveFile;
                    NameSaveFile = new FileInfo( PathToSaveFile ).Name;
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// очистка пути к сохраняемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearSaveFile( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( PathToSaveFile );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ClearSaveFile( object obj )
        {
            try
            {
                PathToSaveFile = NameSaveFile = "";
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// выбор пути к открываемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectOpenDirectory( object obj )
        {
            try
            {
                bool c = false;
                c = string.IsNullOrWhiteSpace( PathToOpenFile ) && string.IsNullOrWhiteSpace( PathToSaveFile );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SelectOpenDirectory( object obj )
        {
            try
            {
                var initionalDirectory = "";
                if (!string.IsNullOrWhiteSpace( EncryptPathtoLastDirectoryOpen ) && Directory.Exists( EncryptPathtoLastDirectoryOpen ))
                    initionalDirectory = new DirectoryInfo( EncryptPathtoLastDirectoryOpen ).FullName;
                var path = Command_executors.Executors.FolderBrowserDialog( "", initionalDirectory );
                PathToOpenDirectory = string.IsNullOrWhiteSpace( path ) ? PathToOpenDirectory : path;
                if (!string.IsNullOrWhiteSpace( PathToOpenDirectory ))
                {
                    EncryptPathtoLastDirectoryOpen = PathToOpenDirectory;
                    NameOpenDirectory = new DirectoryInfo( PathToOpenDirectory ).Name;
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// очистка пути к открываемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearOpenDirectory( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( PathToOpenDirectory );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ClearOpenDirectory( object obj )
        {
            try
            {
                PathToOpenDirectory = NameOpenDirectory = "";
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// выбор пути к сохраняемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectSaveDirectory( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( PathToOpenDirectory ) && Directory.Exists( PathToOpenDirectory );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SelectSaveDirectory( object obj )
        {
            try
            {
                var initionalDirectory = "";
                if (!string.IsNullOrWhiteSpace( EncryptPathtoLastDirectorySave ) && Directory.Exists( EncryptPathtoLastDirectorySave ))
                    initionalDirectory = new DirectoryInfo( EncryptPathtoLastDirectorySave ).FullName;
                var path = Command_executors.Executors.FolderBrowserDialog( "", initionalDirectory );
                PathToSaveDirectory = string.IsNullOrWhiteSpace( path ) ? PathToSaveDirectory : path;
                if (!string.IsNullOrWhiteSpace( PathToSaveDirectory ))
                {
                    EncryptPathtoLastDirectorySave = PathToSaveDirectory;
                    NameSaveDirectory = new DirectoryInfo( PathToSaveDirectory ).Name;
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// очистка пути к сохраняемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearSaveDirectory( object obj )
        {
            try
            {
                bool c = false;
                c = !string.IsNullOrWhiteSpace( PathToSaveDirectory );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_ClearSaveDirectory( object obj )
        {
            try
            {
                PathToSaveDirectory = NameSaveDirectory = "";
            }
            catch (Exception e) { ErrorWindow( e ); }
        }
        /// <summary>
        /// шифрование и сохранение выбранного файла/каталога
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClickButtonEncrypt( object obj )
        {
            throw new NotImplementedException();
        }
        internal void Execute_ClickButtonEncrypt( object obj )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// деифрование и сохранение выбранного файла/каталога
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClickButtonDecrypt( object obj )
        {
            throw new NotImplementedException();
        }
        internal void Execute_ClickButtonDecrypt( object obj )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// окончание загрузки страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageLoaded( object obj )
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// закрытие страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageClose( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageClose( object obj )
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// очистка страницы
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_PageClear( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageClear( object obj )
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow( e ); }
        }


        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new( () => System.Windows.MessageBox.Show( e.Message, $"EncryptIndividualFileModel.{name}" ) );
            thread.SetApartmentState( ApartmentState.STA );
            thread.Start();
        }

    }
}
