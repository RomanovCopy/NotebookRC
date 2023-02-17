using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Styles.CustomizedWindow;
using System.Net.Http;

namespace NotebookRCv001.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly MainWindowModel mainWindowModel;

        public Languages Language => mainWindowModel.Language;

        /// <summary>
        /// текущая кодировка
        /// </summary>
        public Encoding HomeEncoding { get => mainWindowModel.HomeEncoding; set => mainWindowModel.HomeEncoding = value; }

        //public HttpClient client => mainWindowModel.client;

        public ObservableCollection<EncodingInfo> HomeEncodings => mainWindowModel.HomeEncodings;


        public ObservableCollection<Page> FrameList => mainWindowModel.FrameList;
        public Page CurrentPage { get => mainWindowModel.CurrentPage; set => mainWindowModel.CurrentPage = value; }

        public delegate string SelectWindow( string title, string message, string left, string centr, string right );

        public SelectWindow NewSelectWindow;


        #region ________________________Положение и размеры главного окна___________________
        public double WindowWidth
        {
            get => mainWindowModel.WindowWidth;
            set => mainWindowModel.WindowWidth = value;
        }
        public double WindowHeight
        {
            get => mainWindowModel.WindowHeight;
            set => mainWindowModel.WindowHeight = value;
        }
        public double WindowTop
        {
            get => mainWindowModel.WindowTop;
            set => mainWindowModel.WindowTop = value;
        }
        public double WindowLeft
        {
            get => mainWindowModel.WindowLeft;
            set => mainWindowModel.WindowLeft = value;
        }
        public object WindowState
        {
            get => mainWindowModel.WindowState;
            set => mainWindowModel.WindowState = value;
        }
        #endregion


        public MainWindowViewModel()
        {
            mainWindowModel = new MainWindowModel();
            mainWindowModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
            NewSelectWindow = new SelectWindow(mainWindowModel.NewSelectWindow);
        }


        public ICommand FrameListAddPage => frameListAddPage ??= new RelayCommand(mainWindowModel.Execute_FrameListAddPage,
            mainWindowModel.CanExecute_FrameListAddPage);
        RelayCommand frameListAddPage;

        public ICommand FrameListRemovePage => frameListRemovePage ??= new RelayCommand(mainWindowModel.Execute_FrameListRemovePage,
            mainWindowModel.CanExecute_FrameListRemovePage);
        RelayCommand frameListRemovePage;

        public ICommand Frmelist_GoForward => frmelist_GoForward ??= new RelayCommand(mainWindowModel.Execute_Frmelist_GoForward,
            mainWindowModel.CanExecute_Frmelist_GoForward);
        RelayCommand frmelist_GoForward;

        public ICommand Frmelist_GoBack => frmelist_GoBack ??= new RelayCommand(mainWindowModel.Execute_Frmelist_GoBack,
            mainWindowModel.CanExecute_Frmelist_GoBack);
        RelayCommand frmelist_GoBack;

        public ICommand PageClosed => pageClosed ??= new RelayCommand(mainWindowModel.Execute_PageClosed, mainWindowModel.CanExecute_PageClosed);
        RelayCommand pageClosed;

        public ICommand MainWindowLoaded => mainWindowLoaded ??= new RelayCommand(mainWindowModel.Execute_MainWindowLoaded, 
            mainWindowModel.CanExecute_MainWindowLoaded);
        RelayCommand mainWindowLoaded;

        public ICommand MainWindowClosing => mainWindowClosing ??= new RelayCommand(mainWindowModel.Execute_MainWindowClosing,
             mainWindowModel.CanExecute_MainWindowClosing);
        RelayCommand mainWindowClosing;





    }
}
