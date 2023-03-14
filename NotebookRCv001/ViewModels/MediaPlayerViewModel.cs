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

namespace NotebookRCv001.ViewModels
{
    public class MediaPlayerViewModel:ViewModelBase, IPageViewModel
    {
        private MediaPlayerModel mediaPlayerModel { get; set; }
        public ObservableCollection<string> Headers => mediaPlayerModel.Headers;

        public ObservableCollection<string> ToolTips => mediaPlayerModel.ToolTips;

        public Action BehaviorReady { get => mediaPlayerModel.BehaviorReady; set => mediaPlayerModel.BehaviorReady = value; }



        public MediaPlayerViewModel()
        {
            mediaPlayerModel = new MediaPlayerModel();
            mediaPlayerModel.PropertyChanged += ( s, e ) => OnPropertyChanged( e.PropertyName );

        }


        public ICommand PageLoaded => pageLoaded ??= new RelayCommand( mediaPlayerModel.Execute_PageLoaded, mediaPlayerModel.CanExecute_PageLoaded );
        private RelayCommand pageLoaded;
        public ICommand PageClear => pageClear ??= new RelayCommand( mediaPlayerModel.Execute_PageClear, mediaPlayerModel.CanExecute_PageClear );
        private RelayCommand pageClear;
        public ICommand PageClose => pageClose ??= new RelayCommand( mediaPlayerModel.Execute_PageClose, mediaPlayerModel.CanExecute_PageClose );
        private RelayCommand pageClose;

    }
}
