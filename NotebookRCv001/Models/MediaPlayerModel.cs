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
//using System.Windows.Forms;

namespace NotebookRCv001.Models
{
    internal class MediaPlayerModel : ViewModelBase, IDisplayProgressTarget
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private readonly HomeMenuEncryptionViewModel homeMenuEncryptionViewModel;
        private Languages language => mainWindowViewModel.Language;
        private Page page { get; set; }
        private BehaviorMediaElement behaviorMediaElement { get; set; }
        private BehaviorSlider behaviorSlider { get; set; }
        private bool play { get; set; }
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

        internal BitmapImage Bitmap { get => bitmap; set => SetProperty(ref bitmap, value); }
        private BitmapImage bitmap;

        internal ObservableCollection<Image> Images
        {
            get => images;
            set => SetProperty(ref images, value);
        }
        private ObservableCollection<Image> images;

        internal Image CurrentImage
        {
            get => currentImage;
            set => SetProperty(ref currentImage, value);
        }
        private Image currentImage;

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
            play = false;
            BehaviorReady += (x) => { InitializePlayerAndSlider(x); };
        }

        internal bool CanExecute_Play(object obj)
        {
            try
            {
                bool c = false;
                c = behaviorMediaElement != null && behaviorSlider != null && !play;
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
                PlayIndex = PlayIndex - 1;
                if (ThisImage)
                {
                    CurrentImage = Images[PlayIndex];
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
                PlayIndex = PlayIndex + 1;
                if (ThisImage)
                {
                    CurrentImage = Images[PlayIndex];
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
                    CurrentImage = (Image)Images.Where((x) => (string)x.Tag == path).FirstOrDefault();
                    if (CurrentImage != null)
                        PlayIndex = Images.IndexOf(CurrentImage);
                }
                else if (ThisAudio)
                {

                }
                else if (ThisVideo)
                {
                    if (string.IsNullOrWhiteSpace(key))
                    {
                        Content = path;
                        if (CanExecute_Play(obj))
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
                            if (CanExecute_Play(obj))
                                Execute_Play(null);
                            else
                                BehaviorReady += (obj) => { Execute_Play(obj); };
                        }
                    }
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
                    //создаем и заполняем коллекцию файлов для плей листа
                    PlayList = new();
                    Images = new();
                    string key = homeMenuEncryptionViewModel.EncryptionKey;
                    foreach (var file in Directory.GetFiles(dirpath))
                    {
                        if (ThisImage)
                        {
                            PlayList.Add(file);
                            if (string.IsNullOrWhiteSpace(key))
                            {
                                Bitmap = new BitmapImage(new Uri(file));
                                if (Bitmap == null)
                                    EncryptionKeyError(file);
                            }
                            else
                            {
                                Bitmap = await Command_executors.Executors.ImageDecrypt(file, key);
                                if (Bitmap == null)
                                    EncryptionKeyError(file);
                            }
                            if (Bitmap != null)
                            {
                                var image = new Image() { Source = Bitmap };
                                image.Tag = file;
                                Images.Add(image);
                            }
                        }
                        else if (ThisAudio)
                        {
                            if (AudioFileExtensions.Any((x) => x == Path.GetExtension(file).ToLower()))
                                PlayList.Add(file);
                        }
                        else if (ThisVideo)
                        {
                            if (VideoFileExtensions.Any((x) => x == Path.GetExtension(file).ToLower()))
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
                        Bitmap = new BitmapImage(new Uri(path));
                        if (Bitmap == null)
                            throw new Exception();
                    }
                    else if (ThisVideo)
                    {
                        if (File.Exists(path))
                            Content = path;
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
