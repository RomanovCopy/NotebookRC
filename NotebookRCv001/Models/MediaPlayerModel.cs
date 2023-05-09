using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xaml.Behaviors;

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
using NotebookRCv001.Helpers;
using System.Windows.Threading;
using NotebookRCv001.Views;
using System.Windows.Input;
using System.Windows.Media.Media3D;
//using forms = System.Windows.Forms;
//using System.Windows.Forms;

namespace NotebookRCv001.Models
{
    internal class MediaPlayerModel : ViewModelBase, IDisplayProgressTarget
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly HomeMenuEncryptionViewModel homeMenuEncryptionViewModel;
        private readonly HomeMenuFileViewModel homeMenuFileViewModel;
        private Languages language => mainWindowViewModel.Language;
        private Page page { get; set; }
        private BehaviorMediaElement behaviorMediaElement { get; set; }
        private BehaviorSlider behaviorSlider { get; set; }
        private BehaviorImage behaviorImage { get; set; }
        private bool play { get; set; }
        private Size sizeImage { get; set; }
        internal ObservableCollection<string> Headers => language.HeadersMediaPlayer;

        internal ObservableCollection<string> ToolTips => language.ToolTipsMediaPlayer;

        internal bool UserIsDraggingSlider { get => userIsDraggingSlider; set => SetProperty(ref userIsDraggingSlider, value); }
        private bool userIsDraggingSlider;
        /// <summary>
        /// текущая позиция воспроизведения плеера
        /// </summary>
        internal TimeSpan Position { get => behaviorMediaElement.Position; set => behaviorMediaElement.Position = value; }
        /// <summary>
        /// максимальное значение слайдера
        /// </summary>
        internal double Maximum { get => behaviorSlider.Maximum; set => behaviorSlider.Maximum = value; }
        /// <summary>
        /// минимальное значение слайдера
        /// </summary>
        internal double Minimum { get => behaviorSlider.Minimum; set => behaviorSlider.Minimum = value; }
        /// <summary>
        /// текущее положение ползунка слейдера
        /// </summary>
        internal double Value { get => behaviorSlider == null ? 0 : behaviorSlider.Value; set => behaviorSlider.Value = value; }

        public Action<object> BehaviorReady { get => behaviorReady; set => behaviorReady = value; }
        private Action<object> behaviorReady;

        //internal Uri Content { get => content; private set => SetProperty( ref content, value ); }
        //private Uri content;
        internal string Content { get => content; set => SetProperty(ref content, value); }
        private string content;

        internal BitmapImage CurrentBitmap { get => currentBitmap; set => SetProperty(ref currentBitmap, value); }
        private BitmapImage currentBitmap;

        internal Image CurrentImage
        {
            get => currentImage;
            set => SetProperty(ref currentImage, value);
        }
        private Image currentImage;

        internal double CurrentImageScale { get => currentImageScale; set => SetProperty(ref currentImageScale, value); }
        private double currentImageScale;

        internal bool ThisVideo { get => thisVideo; private set => SetProperty(ref thisVideo, value); }
        private bool thisVideo;
        internal bool ThisAudio { get => thisAudio; private set => SetProperty(ref thisAudio, value); }
        private bool thisAudio;
        internal bool ThisImage { get => thisImage; private set => SetProperty(ref thisImage, value); }
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
        internal ObservableCollection<string> PlayList { get => playList; set => SetProperty(ref playList, value); }
        private ObservableCollection<string> playList;

        /// <summary>
        /// индекс текущего элемента в PlayList
        /// </summary>
        internal int PlayIndex { get => playIndex; set => SetProperty(ref playIndex, value); }
        private int playIndex;

        /// <summary>
        /// прогресс выполнения 
        /// </summary>
        public double ProgressValue { get => progressValue; set => SetProperty(ref progressValue, value); }
        private double progressValue;


        internal string PathToLastFile { get => pathToLastFile; set => SetProperty(ref pathToLastFile, value); }
        private string pathToLastFile;
        internal string LastFileName { get => lastFileName; set => SetProperty(ref lastFileName, value); }
        private string lastFileName;

        internal MediaPlayerModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            var home = mainWindowViewModel.FrameList.Where((x) => x is Views.Home).FirstOrDefault();
            var menu = (MyControls.MenuHome)home.FindResource("menuhome");
            homeMenuEncryptionViewModel = (HomeMenuEncryptionViewModel)menu.FindResource("menuencryption");
            homeMenuFileViewModel = (HomeMenuFileViewModel)menu.FindResource("menufile");
            play = false;
            BehaviorReady += (x) => { InitializePlayerAndSlider(x); };
            CurrentImageScale = 1;
        }


        /// <summary>
        /// открыть файл                             
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_OpenFile(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorMediaElement != null && behaviorSlider != null && !play;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OpenFile(object obj)
        {
            try
            {
                homeMenuFileViewModel.OpenFile.Execute(null);
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// закрыть файл                                  
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_CloseFile(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorMediaElement != null && behaviorSlider != null && !play;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_CloseFile(object obj)
        {
            try
            {
                Execute_Stop(null);
                behaviorSlider.Value = 0;
                behaviorSlider.Maximum = 0;
                behaviorMediaElement.Position = TimeSpan.FromSeconds(0);
                Content = string.Empty;
                CurrentImage = null;
                PlayList.Clear();
                PlayIndex = 0;
                play = false;
                homeMenuFileViewModel.PathToLastFile = "";
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_Play(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorMediaElement != null && behaviorSlider != null && !string.IsNullOrWhiteSpace(Content) && !play;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_Play(object obj)
        {
            try
            {
                behaviorMediaElement.Play();
                play = true;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_Pause(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorMediaElement != null && behaviorSlider != null && play;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_Pause(object obj)
        {
            try
            {
                behaviorMediaElement.Pause();
                play = false;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_Stop(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorMediaElement != null && behaviorSlider != null && behaviorSlider.Value > 0;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_Stop(object obj)
        {
            try
            {
                behaviorMediaElement.Stop();
                play = false;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// предыдущий файл в плей листе
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_Back(object obj)
        {
            try
            {
                bool c = false;
                c = PlayIndex > 0;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal async void Execute_Back(object obj)
        {
            try
            {
                string key = homeMenuEncryptionViewModel.EncryptionKey;
                PlayIndex = PlayIndex - 1;
                if (ThisImage)
                {
                    CurrentImage = ImageFromPath(PlayList[PlayIndex], key).Result;
                    homeMenuFileViewModel.PathToLastFile = PlayList[PlayIndex];
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// следующий файл в плей листе
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_Forward(object obj)
        {
            try
            {
                bool c = false;
                c = PlayIndex < PlayList?.Count - 1;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal async void Execute_Forward(object obj)
        {
            try
            {
                string key = homeMenuEncryptionViewModel.EncryptionKey;
                PlayIndex = PlayIndex + 1;
                if (ThisImage)
                {
                    CurrentImage = ImageFromPath(PlayList[PlayIndex], key).Result;
                    homeMenuFileViewModel.PathToLastFile = PlayList[PlayIndex];
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// установка источника контента
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_SetContent(object obj)
        {
            try
            {
                bool c = false;
                c = obj is string str && !string.IsNullOrWhiteSpace(str) && File.Exists(str);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal async void Execute_SetContent(object obj)
        {
            string key = homeMenuEncryptionViewModel.EncryptionKey;
            string path = (string)obj;
            try
            {
                SetContentType(path);
                if (ThisImage)
                {
                    CurrentBitmap = await BitmapFromPath(path, key, (int)sizeImage.Height);
                    sizeImage = new Size(CurrentBitmap.Width, CurrentBitmap.Height);
                    PlayIndex = PlayList.IndexOf(path);
                    //CurrentImage = ImageFromPath(path, key).Result;
                    //if (CurrentImage.Tag != null)
                    //    PlayIndex = PlayList.IndexOf(path);
                    //else
                    //    CurrentImage = ImageFromPath(path, null).Result;
                    //if (CurrentImage.Tag == null)
                    //    throw new Exception();

                }
                else if (ThisAudio)
                {

                }
                else if (ThisVideo)
                {
                    VideoFromPath(path);
                }
            }
            catch
            {
                EncryptionKeyError(path);
            }
        }

        internal bool CanExecute_MediaPlayerLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_MediaPlayerLoaded(object obj)
        {
            try
            {
                if (BehaviorReady != null)
                    BehaviorReady.Invoke(obj);
                //BehaviorReady = null;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_ThumbDragStarted(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorMediaElement != null && behaviorSlider != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ThumbDragStarted(object obj)
        {
            try
            {
                UserIsDraggingSlider = true;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_ThumbDragCompleted(object obj)
        {
            try
            {
                bool c = false;
                c = UserIsDraggingSlider;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ThumbDragCompleted(object obj)
        {
            try
            {
                behaviorMediaElement.Position = TimeSpan.FromSeconds(behaviorSlider.Value);
                UserIsDraggingSlider = false;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_SliderLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SliderLoaded(object obj)
        {
            try
            {
                if (BehaviorReady != null)
                    BehaviorReady.Invoke(obj);
                //BehaviorReady = null;
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// окончание загрузки изображения
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ImageLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ImageLoaded(object obj)
        {
            try
            {
                if(obj is BehaviorImage behavior)
                {
                    behaviorImage = behavior;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


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
            {
                if (obj is Page p)
                {
                    page = p;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageClear(object obj)
        {
            try
            {
                bool c = false;
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

        internal bool CanExecute_PageClose(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose(object obj)
        {
            try
            {
                if (mainWindowViewModel.FrameListRemovePage.CanExecute(page))
                    mainWindowViewModel.FrameListRemovePage.Execute(page);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// управление клавишами клавиатуры
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if ((e.Key == Key.A || e.Key == Key.Left) && CanExecute_Back(null))
                {
                    Execute_Back(null);
                }
                else if ((e.Key == Key.D || e.Key == Key.Right) && CanExecute_Forward(null))
                {
                    Execute_Forward(null);
                }
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        /// <summary>
        /// управление зуммом (колесико мыши)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            try
            {
                if (e.Delta > 0)
                    CurrentImageScale += 0.1;
                else
                    CurrentImageScale -= 0.1;
                Application.Current.MainWindow.Focus();
                var newheight = sizeImage.Height * CurrentImageScale;
                var path = CurrentBitmap.UriSource.AbsolutePath;
                var bitmap = CreateBitmapImageFromPath(path, (int)newheight);
                CurrentBitmap = bitmap;
                //CurrentImage = new Image() { Source = bitmap, Tag = path, Stretch=Stretch.None };
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        private BitmapImage CreateBitmapImageFromPath(string path, int height=0)
        {
            try
            {
                BitmapImage bitmapImage = new();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(path);
                //bitmapImage.Rotation = Rotation.Rotate0;
                if (height > 0)
                    bitmapImage.DecodePixelHeight = (int)((double)height * (96.0 / 72.0));
                bitmapImage.EndInit();
                return bitmapImage;
            }
            catch (Exception e) { ErrorWindow(e); return null; }
        }

        private void SetCurrentPositionForSlider(TimeSpan time)
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        private void InitializePlayerAndSlider(object obj)
        {
            try
            {
                if (obj is BehaviorMediaElement mediaElement)
                {
                    behaviorMediaElement = mediaElement;
                    behaviorMediaElement.MediaFailed += (s, e) => { EncryptionKeyError((s as MediaElement).Source.LocalPath); };
                }
                else if (obj is BehaviorSlider slider)
                {
                    behaviorSlider = slider;
                }
                if (behaviorSlider != null && behaviorMediaElement != null)
                {
                    BehaviorReady = null;
                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += TimerTick;
                    timer.Start();
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            try
            {
                if (play && behaviorMediaElement.MediaElement.NaturalDuration.HasTimeSpan && !UserIsDraggingSlider)
                {
                    behaviorSlider.Minimum = 0;
                    behaviorSlider.Maximum = behaviorMediaElement.MediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                    behaviorSlider.Value = behaviorMediaElement.MediaElement.Position.TotalSeconds;
                    if (behaviorSlider.Value >= behaviorSlider.Maximum)
                    {
                        Execute_Stop(null);
                        behaviorSlider.Value = 0;
                        behaviorMediaElement.Position = TimeSpan.FromSeconds(0);
                    }
                }
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        private async Task<string> VideoDecrypt(string path, string key)
        {
            FileStream stream = null;
            try
            {
                string ext = Path.GetExtension(path);
                string newDir = Path.Combine(Directory.GetCurrentDirectory(), "temp");
                if (!Directory.Exists(newDir))
                    Directory.CreateDirectory(newDir);
                string newPath = Path.Combine(newDir, $"temp{ext}");
                stream = File.OpenRead(path);
                await Task.Factory.StartNew(() => Command_executors.Executors.DecryptFromStream(stream, newPath, key));
                return newPath;
            }
            catch (Exception e)
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
                ErrorWindow(e);
                return path;
            }
        }

        private async Task<Image> ImageFromPath(string path, string key)
        {
            BitmapImage bitmap = null;
            Image image = new();
            try
            {
                if (string.IsNullOrWhiteSpace(key))
                {
                    bitmap = new BitmapImage(new Uri(path));
                    if (bitmap == null)
                        EncryptionKeyError(path);
                }
                else
                {
                    bitmap = await Command_executors.Executors.ImageDecrypt(path, key);
                    if (bitmap == null)
                        EncryptionKeyError(path);
                }
                if (bitmap != null)
                {
                    image.Source = bitmap;
                    sizeImage = new(bitmap.PixelWidth, bitmap.PixelHeight);
                    image.Tag = path;
                }
                return image;
            }
            catch (Exception e) { ErrorWindow(e); return image; }
        }

        private async Task<BitmapImage> BitmapFromPath(string path, string key, int height=0)
        {
            BitmapImage bitmap = new BitmapImage();
            try
            {
                bitmap.BeginInit();
                if (string.IsNullOrWhiteSpace(key))
                {
                    bitmap = CreateBitmapImageFromPath(path, height);
                    if (bitmap == null)
                        EncryptionKeyError(path);
                }
                else
                {
                    bitmap = await Command_executors.Executors.ImageDecrypt(path, key, height);
                    if (bitmap == null)
                        EncryptionKeyError(path);
                }
                return bitmap;
            }
            catch (Exception e) { ErrorWindow(e); return bitmap; }
        }
        private async void VideoFromPath(string path)
        {
            try
            {
                var key = homeMenuEncryptionViewModel.EncryptionKey;
                if (string.IsNullOrWhiteSpace(key))
                {
                    Content = path;
                    if (CanExecute_Play(null))
                        Execute_Play(null);
                    else
                        BehaviorReady += (obj) => { Execute_Play(obj); };
                }
                else
                {
                    Content = await VideoDecrypt(path, key);
                    if (!File.Exists(Content))
                    {
                        EncryptionKeyError(path);
                    }
                    else
                    {
                        if (CanExecute_Play(null))
                            Execute_Play(null);
                        else
                            BehaviorReady += (obj) => { Execute_Play(obj); };
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// переключение типа контента
        /// </summary>
        /// <param name="path"></param>
        private async void SetContentType(string path)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
                {
                    //включаем нужную панель
                    var ext = Path.GetExtension(path).ToLower();
                    ThisAudio = AudioFileExtensions.Any((x) => x == ext);
                    ThisImage = ImageFileExtensions.Any((x) => x == ext);
                    ThisVideo = VideoFileExtensions.Any((x) => x == ext);
                    //родительская папка
                    var dirpath = Path.GetDirectoryName(path);
                    //создаем и заполняем коллекцию файлов для плейлиста
                    PlayList = new();
                    string key = homeMenuEncryptionViewModel.EncryptionKey;
                    if (ThisImage || ThisAudio || ThisVideo)
                    {
                        foreach (var file in Directory.GetFiles(dirpath))
                        {
                            ext = Path.GetExtension(file);
                            if (ThisAudio && AudioFileExtensions.Any((x) => x == ext))
                                PlayList.Add(file);
                            else if (ThisImage && ImageFileExtensions.Any((x) => x == ext))
                                PlayList.Add(file);
                            else if (ThisVideo && VideoFileExtensions.Any((x) => x == ext))
                                PlayList.Add(file);
                        }
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// обработка ошибок ключа шифрования
        /// </summary>
        private async void EncryptionKeyError(string path)
        {
            var messages = new MyMessages();
            var messagesVM = (ViewModels.MyMessagesViewModel)messages.DataContext;
            try
            {
                messagesVM.SetTitle.Execute(language.MyMessagesHeaders[1]);
                messagesVM.SetButtonText.Execute(language.MyMessagesHeaders[5]);
                var key = homeMenuEncryptionViewModel.EncryptionKey;
                if (string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(path))
                {
                    messagesVM.SetMessage.Execute(language.MessagesMyMessages[8]);
                    messages.ShowDialog();
                    Execute_PageClose(null);
                }
                else if (!string.IsNullOrWhiteSpace(path))
                {
                    if (ThisImage)
                    {
                        CurrentImage = ImageFromPath(path, null).Result;
                    }
                    else if (ThisVideo)
                    {
                        if (File.Exists(path))
                        {
                            Content = path;
                            Execute_Play(null);
                        }
                        else
                            throw new Exception();
                    }
                }
                messages.Close();
            }
            catch
            {
                messagesVM.SetMessage.Execute(language.MessagesMyMessages[9]);
                messages.ShowDialog();
                Execute_PageClose(null);
            }
        }
    }
}
