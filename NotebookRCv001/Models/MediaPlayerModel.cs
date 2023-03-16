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
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NotebookRCv001.Models
{
    internal class MediaPlayerModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly HomeMenuEncryptionViewModel homeMenuEncryptionViewModel;
        private Languages language => mainWindowViewModel.Language;
        private Page page { get; set; }
        private MediaElement player { get; set; }
        internal ObservableCollection<string> Headers => language.HeadersMediaPlayer;

        internal ObservableCollection<string> ToolTips => language.ToolTipsMediaPlayer;

        public Action BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        private Action behaviorReady;

        internal Uri Content { get => content; private set => SetProperty( ref content, value ); }
        private Uri content;

        internal BitmapImage Bitmap { get => bitmap; set => SetProperty( ref bitmap, value ); }
        private BitmapImage bitmap;

        internal bool ThisVideo { get => thisVideo; private set => SetProperty( ref thisVideo, value ); }
        private bool thisVideo;
        internal bool ThisAudio { get => thisAudio; private set => SetProperty( ref thisAudio, value ); }
        private bool thisAudio;
        internal bool ThisImage { get => thisImage; private set => SetProperty( ref thisImage, value ); }
        private bool thisImage;

        internal string[] VideoFileExtensions => videoFileExtensions ??= new string[] { ".mp4", ".mpg", ".avi" };
        private string[] videoFileExtensions;

        internal string[] ImageFileExtensions => imageFileExtensions ??= new string[] { ".jpg", ".jpeg", ".png" };
        private string[] imageFileExtensions;

        internal string[] AudioFileExtensions => audioFileExtensions ??= new string[] { ".mp3", ".flac" };
        private string[] audioFileExtensions;

        /// <summary>
        /// список воспроизведения(пути к файлам) PlayList
        /// </summary>
        internal ObservableCollection<string> PlayList { get => playList; set => SetProperty( ref playList, value ); }
        private ObservableCollection<string> playList;

        /// <summary>
        /// индекс текущего элемента в PlayList
        /// </summary>
        internal int PlayIndex { get => playIndex; set => SetProperty( ref playIndex, value ); }
        private int playIndex;

        internal MediaPlayerModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += ( s, e ) => OnPropertyChanged( new string[] { "Headers", "ToolTips" } );
            var home = mainWindowViewModel.FrameList.Where( ( x ) => x is Views.Home ).FirstOrDefault();
            var menu = (MyControls.MenuHome)home.FindResource( "menuhome" );
            homeMenuEncryptionViewModel = (HomeMenuEncryptionViewModel)menu.FindResource( "menuencryption" );
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

        /// <summary>
        /// предыдущий файл в плей листе
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_Back( object obj )
        {
            try
            {
                bool c = false;
                c = PlayIndex > 0;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal async void Execute_Back( object obj )
        {
            try
            {
                PlayIndex = PlayIndex - 1;
                if (ThisImage)
                {
                    if (string.IsNullOrWhiteSpace( homeMenuEncryptionViewModel.KeyCript ))
                    {
                        Bitmap = new BitmapImage( new Uri( PlayList[PlayIndex] ) );
                    }
                    else
                    {
                        Bitmap = await ImageDecrypt( PlayList[PlayIndex], homeMenuEncryptionViewModel.KeyCript );
                    }
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// следующий файл в плей листе
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_Forward( object obj )
        {
            try
            {
                bool c = false;
                c = PlayIndex < PlayList?.Count - 1;
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal async void Execute_Forward( object obj )
        {
            try
            {
                PlayIndex = PlayIndex + 1;
                if (ThisImage)
                {
                    if (string.IsNullOrWhiteSpace( homeMenuEncryptionViewModel.KeyCript ))
                    {
                        Bitmap = new BitmapImage( new Uri( PlayList[PlayIndex] ) );
                    }
                    else
                    {
                        Bitmap = await ImageDecrypt( PlayList[PlayIndex], homeMenuEncryptionViewModel.KeyCript );
                    }
                }
            }
            catch (Exception e) { ErrorWindow( e ); }
        }

        /// <summary>
        /// установка источника контента
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_SetContent( object obj )
        {
            try
            {
                bool c = false;
                c = obj is string str && !string.IsNullOrWhiteSpace( str ) && File.Exists( str );
                return c;
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal async void Execute_SetContent( object obj )
        {
            try
            {
                string path = (string)obj;
                SetContentType( path );
                if (ThisImage)
                {
                    if (string.IsNullOrWhiteSpace( homeMenuEncryptionViewModel.KeyCript ))
                        Bitmap = new BitmapImage( new Uri( path ) );
                    else
                        Bitmap = await ImageDecrypt( path, homeMenuEncryptionViewModel.KeyCript );
                }
                else if (ThisAudio)
                {

                }
                else if (ThisVideo)
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
                if (obj is MediaElement player)
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
            }
            catch (Exception e) { ErrorWindow( e ); return false; }
        }
        internal void Execute_PageLoaded( object obj )
        {
            try
            {
                if (obj is Page p)
                {
                    page = p;
                }
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

        private async Task<BitmapImage> ImageDecrypt( string path, string key )
        {
            BitmapImage bitmapImage = new();
            try
            {
                byte[] bytes = null;
                using (var fs = new FileStream( path, FileMode.Open ))
                {
                    bytes = new byte[fs.Length];
                    await fs.ReadAsync( bytes, 0, (int)fs.Length );
                }
                bytes = Command_executors.Executors.Decrypt( bytes, key );
                using (var stream = new MemoryStream( bytes ))
                {
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze();
                }
                return bitmapImage;
            }
            catch (Exception e) { ErrorWindow( e ); return bitmapImage; }
        }

        /// <summary>
        /// переключение типа контента
        /// </summary>
        /// <param name="path"></param>
        private void SetContentType( string path )
        {
            try
            {
                if (!string.IsNullOrWhiteSpace( path ) && File.Exists( path ))
                {
                    //включаем нужную панель
                    var ext = Path.GetExtension( path ).ToLower();
                    ThisAudio = AudioFileExtensions.Any( ( x ) => x == ext );
                    ThisImage = ImageFileExtensions.Any( ( x ) => x == ext );
                    ThisVideo = VideoFileExtensions.Any( ( x ) => x == ext );
                    //родительская папка
                    var dirpath = Path.GetDirectoryName( path );
                    //создаем и заполняем коллекцию файлов для плей листа
                    PlayList = new();
                    foreach (var file in Directory.GetFiles( dirpath ))
                    {
                        if (ThisImage)
                        {
                            if (ImageFileExtensions.Any( ( x ) => x == Path.GetExtension( file ).ToLower() ))
                                PlayList.Add( file );
                        }
                        else if (ThisAudio)
                        {
                            if (AudioFileExtensions.Any( ( x ) => x == Path.GetExtension( file ).ToLower() ))
                                PlayList.Add( file );
                        }
                        else if (ThisVideo)
                        {
                            if (VideoFileExtensions.Any( ( x ) => x == Path.GetExtension( file ).ToLower() ))
                                PlayList.Add( file );
                        }
                    }
                    PlayIndex = PlayList.IndexOf( path );
                }
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
