﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;

namespace NotebookRCv001.ViewModels
{
    public class MediaPlayerViewModel:ViewModelBase, IPageViewModel
    {
        private MediaPlayerModel mediaPlayerModel { get; set; }
        public ObservableCollection<string> Headers => mediaPlayerModel.Headers;

        public ObservableCollection<string> ToolTips => mediaPlayerModel.ToolTips;

        public Action BehaviorReady { get => mediaPlayerModel.BehaviorReady; set => mediaPlayerModel.BehaviorReady = value; }

        public Uri Content => mediaPlayerModel.Content;
        public BitmapImage Bitmap  => mediaPlayerModel.Bitmap;
        public bool ThisVideo => mediaPlayerModel.ThisVideo;
        public bool ThisAudio => mediaPlayerModel.ThisAudio;
        public bool ThisImage => mediaPlayerModel.ThisImage;

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

        /// <summary>
        /// установка источника контента
        /// </summary>
        public ICommand SetContent => setContent ??= new RelayCommand( mediaPlayerModel.Execute_SetContent, mediaPlayerModel.CanExecute_SetContent );
        private RelayCommand setContent;
        public ICommand MediaPlayerLoaded => mediaPlayerLoaded ??= new RelayCommand( mediaPlayerModel.Execute_MediaPlayerLoaded,
            mediaPlayerModel.CanExecute_MediaPlayerLoaded );
        private RelayCommand mediaPlayerLoaded;
        public ICommand PageLoaded => pageLoaded ??= new RelayCommand( mediaPlayerModel.Execute_PageLoaded, mediaPlayerModel.CanExecute_PageLoaded );
        private RelayCommand pageLoaded;
        public ICommand PageClear => pageClear ??= new RelayCommand( mediaPlayerModel.Execute_PageClear, mediaPlayerModel.CanExecute_PageClear );
        private RelayCommand pageClear;
        public ICommand PageClose => pageClose ??= new RelayCommand( mediaPlayerModel.Execute_PageClose, mediaPlayerModel.CanExecute_PageClose );
        private RelayCommand pageClose;

    }
}
