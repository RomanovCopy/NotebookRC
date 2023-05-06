using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models;
//using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using NotebookRCv001.Helpers;
using NotebookRCv001.MyControls;
using System.Windows.Controls;

namespace NotebookRCv001.ViewModels
{
    public class MediaPlayerViewModel:ViewModelBase, IPageViewModel, ILastFileName
    {
        private MediaPlayerModel mediaPlayerModel { get; set; }
        public ObservableCollection<string> Headers => mediaPlayerModel.Headers;

        public ObservableCollection<string> ToolTips => mediaPlayerModel.ToolTips;

        public bool UserIsDraggingSlider { get => mediaPlayerModel.UserIsDraggingSlider; set => mediaPlayerModel.UserIsDraggingSlider = value; }
        /// <summary>
        /// позиция воспроизведения плеера
        /// </summary>
        public TimeSpan Position { get => mediaPlayerModel.Position; set => mediaPlayerModel.Position = value; }
        /// <summary>
        /// максимальное значение слайдера
        /// </summary>
        public double Maximum { get => mediaPlayerModel.Maximum; set => mediaPlayerModel.Maximum = value; }
        /// <summary>
        /// минимальное значение слайдера
        /// </summary>
        public double Minimum { get => mediaPlayerModel.Minimum; set => mediaPlayerModel.Minimum = value; }
        /// <summary>
        /// текущее положение ползунка слайдера
        /// </summary>
        public double Value { get => mediaPlayerModel.Value; set => mediaPlayerModel.Value = value; }
        public Action<object> BehaviorReady { get => mediaPlayerModel.BehaviorReady; set => mediaPlayerModel.BehaviorReady = value; }

        public string Content => mediaPlayerModel.Content;


        public BitmapImage Bitmap  => mediaPlayerModel.Bitmap;

        public ObservableCollection<Image> Images 
        { get => mediaPlayerModel.Images; set => mediaPlayerModel.Images = value; }

        public ObservableCollection<string> PlayList => mediaPlayerModel.PlayList;

        public Image CurrentImage 
        { get => mediaPlayerModel.CurrentImage; set => mediaPlayerModel.CurrentImage= value; }

        public bool ThisVideo => mediaPlayerModel.ThisVideo;
        public bool ThisAudio => mediaPlayerModel.ThisAudio;
        public bool ThisImage => mediaPlayerModel.ThisImage;



        public string PathToLastFile { get => mediaPlayerModel.PathToLastFile; set => mediaPlayerModel.PathToLastFile=value; }
        public string LastFileName { get => mediaPlayerModel.LastFileName; set => mediaPlayerModel.LastFileName = value; }



        public MediaPlayerViewModel()
        {
            mediaPlayerModel = new MediaPlayerModel();
            mediaPlayerModel.PropertyChanged += ( s, e ) => OnPropertyChanged( e.PropertyName );

        }
        public ICommand Play => play ??= new RelayCommand( mediaPlayerModel.Execute_Play, mediaPlayerModel.CanExecute_Play );
        private RelayCommand play;
        public ICommand Pause => pause ??= new RelayCommand( mediaPlayerModel.Execute_Pause, mediaPlayerModel.CanExecute_Pause );
        private RelayCommand pause;
        public ICommand Stop => stop ??= new RelayCommand( mediaPlayerModel.Execute_Stop, mediaPlayerModel.CanExecute_Stop );
        private RelayCommand stop;

        public ICommand Back => back ??= new RelayCommand( mediaPlayerModel.Execute_Back, mediaPlayerModel.CanExecute_Back );
        private RelayCommand back;
        public ICommand Forward => forward ??= new RelayCommand( mediaPlayerModel.Execute_Forward, mediaPlayerModel.CanExecute_Forward );
        private RelayCommand forward;

        public ICommand SliderLoaded => sliderLoaded ??= new RelayCommand( mediaPlayerModel.Execute_SliderLoaded, mediaPlayerModel.CanExecute_SliderLoaded );
        private RelayCommand sliderLoaded;
        /// <summary>
        /// установка источника контента
        /// </summary>
        public ICommand SetContent => setContent ??= new RelayCommand( mediaPlayerModel.Execute_SetContent, mediaPlayerModel.CanExecute_SetContent );
        private RelayCommand setContent;

        public ICommand FrameLoaded => frameLoaded ??= new RelayCommand(mediaPlayerModel.Execute_FrameLoaded, mediaPlayerModel.CanExecute_FrameLoaded);
        private RelayCommand frameLoaded;

        public ICommand FrameUnloaded => frameUnloaded ??= new RelayCommand(mediaPlayerModel.Execute_FrameUnloaded, mediaPlayerModel.CanExecute_FrameUnloaded);
        private RelayCommand frameUnloaded;
        public ICommand MediaPlayerLoaded => mediaPlayerLoaded ??= new RelayCommand( mediaPlayerModel.Execute_MediaPlayerLoaded,
            mediaPlayerModel.CanExecute_MediaPlayerLoaded );
        private RelayCommand mediaPlayerLoaded;
        public ICommand ThumbDragStarted => thumbDragStarted ??= new RelayCommand( mediaPlayerModel.Execute_ThumbDragStarted,
            mediaPlayerModel.CanExecute_ThumbDragStarted );
        private RelayCommand thumbDragStarted;
        public ICommand ThumbDragCompleted => thumbDragCompleted ??= new RelayCommand( mediaPlayerModel.Execute_ThumbDragCompleted,
            mediaPlayerModel.CanExecute_ThumbDragCompleted );
        private RelayCommand thumbDragCompleted;
        public ICommand PageLoaded => pageLoaded ??= new RelayCommand( mediaPlayerModel.Execute_PageLoaded, mediaPlayerModel.CanExecute_PageLoaded );
        private RelayCommand pageLoaded;
        public ICommand PageClear => pageClear ??= new RelayCommand( mediaPlayerModel.Execute_PageClear, mediaPlayerModel.CanExecute_PageClear );
        private RelayCommand pageClear;
        public ICommand PageClose => pageClose ??= new RelayCommand( mediaPlayerModel.Execute_PageClose, mediaPlayerModel.CanExecute_PageClose );
        private RelayCommand pageClose;

    }
}
