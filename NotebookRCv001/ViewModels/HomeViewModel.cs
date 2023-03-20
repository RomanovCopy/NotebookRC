using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Drawing = System.Drawing;
using Media = System.Windows.Media;
using System.Collections.ObjectModel;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using NotebookRCv001.Helpers;
using System.Windows.Media;

namespace NotebookRCv001.ViewModels
{
    public class HomeViewModel : ViewModelBase, IPageViewModel, ILastFileName
    {
        private readonly HomeModel homeModel;
        public ObservableCollection<string> Headers => throw new NotImplementedException();
        public ObservableCollection<string> ToolTips => throw new NotImplementedException();

        public string EncryptionStatus => homeModel?.EncryptionStatus;
        public Encoding HomeEncoding => homeModel?.HomeEncoding;
        public ObservableCollection<Drawing.Color> HomeMyColors => homeModel.HomeMyColors;
        public Media.Brush MyBackground { get => homeModel.MyBackground;
            set => homeModel.MyBackground = value; }
        public Media.Brush MyForeground { get => homeModel.MyForeground;
            set => homeModel.MyForeground = value; }
        public Media.Brush MyFontBackground { get => homeModel.MyFontBackground;
            set => homeModel.MyFontBackground = value; }
        public string WorkingDirectory => homeModel.WorkingDirectory;
        public string WorkingDirectoryName => homeModel.WorkingDirectoryName;
        public string PathToLastFile { get => homeModel.PathToLastFile; set => homeModel.PathToLastFile = value; }
        public string LastFileName { get => homeModel.LastFileName;  }


        public ObservableCollection<string> Icons => throw new NotImplementedException();

        public ObservableCollection<string> Images => throw new NotImplementedException();


        public Action<object> BehaviorReady { get => homeModel.BehaviorReady; set => homeModel.BehaviorReady = value; }

        public HomeViewModel()
        {
            homeModel = new HomeModel();
            homeModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }


        public ICommand HomeLoaded => homeLoaded ??= new RelayCommand(homeModel.Execute_HomeLoaded, homeModel.CanExecute_HomeLoaded);
        private RelayCommand homeLoaded;

        /// <summary>
        /// очистка страницы Home и строки пути к последненму открытому файлу
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ICommand PageClear => pageClear ??= new RelayCommand(homeModel.Execute_PageClear, homeModel.CanExecute_PageClear);
        private RelayCommand pageClear;


        public ICommand PageClose => pageClose ??= new RelayCommand(homeModel.Execute_PageClose, homeModel.CanExecute_PageClose);
        private RelayCommand pageClose;

        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(homeModel.Execute_HomeLoaded, homeModel.CanExecute_HomeLoaded);
        RelayCommand pageLoaded;

    }
}
