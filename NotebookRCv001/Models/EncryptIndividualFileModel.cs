using System;
using System.Collections.Generic;
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
    internal class EncryptIndividualFileModel:ViewModelBase
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
        /// путь к открываемому файлу
        /// </summary>
        internal string PathToOpenFile { get => pathToOpenFile; set => SetProperty( ref pathToOpenFile, value ); }
        private string pathToOpenFile;
        /// <summary>
        /// путь к сохраняемому файлу
        /// </summary>
        internal string PathToSaveFile { get => pathToSaveFile; set => SetProperty( ref pathToSaveFile, value ); }
        private string pathToSaveFile;

        /// <summary>
        /// путь к открываемой директории
        /// </summary>
        internal string PathToOpenDirectory { get => pathToOpenDirectory; set => SetProperty( ref pathToOpenDirectory, value ); }
        private string pathToOpenDirectory;
        /// <summary>
        /// путь к сохраняемой директории
        /// </summary>
        internal string PathToSaveDirectory { get => pathToSaveDirectory; set => SetProperty( ref pathToSaveDirectory, value ); }
        private string pathToSaveDirectory;


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
        internal void Execute_SelectOpenFile( object obj )
        {
            throw new NotImplementedException();
        }
        internal bool CanExecute_SelectOpenFile( object obj )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// очистка пути к открываемому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearOpenFile( object obj )
        {
            throw new NotImplementedException();
        }
        internal void Execute_ClearOpenFile( object obj )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// выбор пути к открываемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectOpenDirectory( object obj )
        {
            throw new NotImplementedException();
        }
        internal void Execute_SelectOpenDirectory( object obj )
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// очистка пути к открываемой директории
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ClearOpenDirectory(object obj)
        {
            throw new NotImplementedException();
        }
        internal void Execute_ClearOpenDirectory(object obj)
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
            catch(Exception e) { ErrorWindow( e ); return false; }
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
