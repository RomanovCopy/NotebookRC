using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;
using NotebookRCv001.ViewModels;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NotebookRCv001.Models
{
    internal class MediaPlayerModel:ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;
        private object page { get; set; }

        public ObservableCollection<string> Headers => language.HeadersMediaPlayer;

        public ObservableCollection<string> ToolTips => language.ToolTipsMediaPlayer;

        public Action BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        private Action behaviorReady;


        internal MediaPlayerModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += ( s, e ) => OnPropertyChanged( new string[] { "Headers", "ToolTips" } );

        }




        internal bool CanExecute_PageLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }catch(Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageLoaded( object obj )
        {
            try
            {
                page = obj;
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_PageClear( object obj )
        {
            try
            {
                bool c = false;
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
                if (mainWindowViewModel.FrameListRemovePage.CanExecute( page ))
                    mainWindowViewModel.FrameListRemovePage.Execute( page );
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        private void ErrorWindow( Exception e, [CallerMemberName] string name = "" )
        {
            Thread thread = new( () => MessageBox.Show( e.Message, $"MediaPlayerModel.{name}" ) );
            thread.SetApartmentState( ApartmentState.STA );
            thread.Start();
        }


    }
}
