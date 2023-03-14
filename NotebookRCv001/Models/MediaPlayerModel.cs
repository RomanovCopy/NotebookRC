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
using NotebookRCv001.MyControls;
using System.Windows.Controls;

namespace NotebookRCv001.Models
{
    internal class MediaPlayerModel:ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;
        private Page page { get; set; }
        private MediaElement player { get; set; }
        public ObservableCollection<string> Headers => language.HeadersMediaPlayer;

        public ObservableCollection<string> ToolTips => language.ToolTipsMediaPlayer;

        public Action BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        private Action behaviorReady;

        internal Uri Content { get => content; set => SetProperty( ref content, value ); }
        private Uri content;


        internal MediaPlayerModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += ( s, e ) => OnPropertyChanged( new string[] { "Headers", "ToolTips" } );
        }

        internal bool CanExecute_Play( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_Play( object obj )
        {
            try
            {
                if (obj is MediaElement player)
                    player.Play();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_Pause( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_Pause( object obj )
        {
            try
            {
                if (obj is MediaElement player)
                    player.Pause();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_Stop( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_Stop( object obj )
        {
            try
            {
                if (obj is MediaElement player)
                    player.Stop();
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_SetContent( object obj )
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_SetContent( object obj )
        {
            try
            {
                if(obj is string str && !string.IsNullOrWhiteSpace( str ))
                {

                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        internal bool CanExecute_MediaPlayerLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_MediaPlayerLoaded( object obj )
        {
            try
            {
                if(obj is MediaElement player)
                {
                    this.player = player;
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
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
                if (obj is Page p)
                    page = p;
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
