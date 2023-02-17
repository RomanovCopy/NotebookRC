using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class SelectWindowViewModel : ViewModelBase, IWindowViewModel
    {
        private readonly SelectWindowModel selectWindowModel;


        #region  _________________________Window dimensions and position______________________________

        public double WindowWidth { get => selectWindowModel.WindowWidth; set => selectWindowModel.WindowWidth = value; }
        public double WindowHeight { get => selectWindowModel.WindowHeight; set => selectWindowModel.WindowHeight = value; }
        public double WindowTop { get => selectWindowModel.WindowTop; set => selectWindowModel.WindowTop = value; }
        public double WindowLeft { get => selectWindowModel.WindowLeft; set => selectWindowModel.WindowLeft = value; }
        public object WindowState { get => selectWindowModel.WindowState; set => selectWindowModel.WindowState = value; }



        #endregion

        public string Result { get => selectWindowModel.Result; set => selectWindowModel.Result = value; }
        /// <summary>
        /// заголовок окна
        /// </summary>
        public string Title { get => selectWindowModel.Title; set => selectWindowModel.Title = value; }
        /// <summary>
        /// сообщение окна
        /// </summary>
        public string Message { get => selectWindowModel.Message; set => selectWindowModel.Message = value; }
        /// <summary>
        /// текст левой кнопки
        /// </summary>
        public string LeftButtonContent { get => selectWindowModel.LeftButtonContent; set => selectWindowModel.LeftButtonContent = value; }
        /// <summary>
        /// текст центральной кнопки
        /// </summary>
        public string CenterButtonContent { get => selectWindowModel.CenterButtonContent; set => selectWindowModel.CenterButtonContent = value; }
        /// <summary>
        /// текст правой кнопки
        /// </summary>
        public string RightButtonContent { get => selectWindowModel.RightButtonContent; set => selectWindowModel.RightButtonContent=value; }


        public ObservableCollection<string> Headers => selectWindowModel.Headers;
        public ObservableCollection<string> ToolTips => selectWindowModel.ToolTips;
        public ObservableCollection<string> Messages => selectWindowModel.Messages;



        public SelectWindowViewModel()
        {
            selectWindowModel = new SelectWindowModel();
            selectWindowModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }

        public ICommand ButtonsClick => buttonsClick ??= new RelayCommand(selectWindowModel.Execute_ButtonsClick, selectWindowModel.CanExecute_ButtonsClick);
        RelayCommand buttonsClick;

        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand(selectWindowModel.Execute_WindowLoaded, selectWindowModel.CanExecute_WindowLoaded);
        RelayCommand windowLoaded;

        public ICommand WindowClosing => windowClosing ??= new RelayCommand(selectWindowModel.Execute_WindowClosing, selectWindowModel.CanExecute_WindowClosing);
        RelayCommand windowClosing;
    }
}
