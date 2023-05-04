using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace NotebookRCv001.ViewModels
{
    public class MediaImageViewModel:ViewModelBase, IPageViewModel
    {
        private readonly MediaImageModel mediaImageModel;
        public ObservableCollection<string> Headers => mediaImageModel.Headers;
        public ObservableCollection<string> ToolTips => mediaImageModel.ToolTips;
        public BitmapImage Bitmap { get => mediaImageModel.Bitmap; set => mediaImageModel.Bitmap = value; }

        public Action<object> BehaviorReady { get => mediaImageModel.BehaviorReady; set => mediaImageModel.BehaviorReady=value; }


        public MediaImageViewModel()
        {
            mediaImageModel = new MediaImageModel();
            mediaImageModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(mediaImageModel.Execute_PageLoaded,
            mediaImageModel.CanExecute_PageLoaded);
        private RelayCommand pageLoaded;

        public ICommand PageClose => pageClose ??= new RelayCommand(mediaImageModel.Execute_PageClose
            , mediaImageModel.CanExecute_PageClose);
        private RelayCommand pageClose;

        public ICommand PageClear => pageClear ??= new RelayCommand(mediaImageModel.Execute_PageClear,
            mediaImageModel.CanExecute_PageClear);
        private RelayCommand pageClear;
    }
}
