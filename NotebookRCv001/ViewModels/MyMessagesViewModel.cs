using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;

namespace NotebookRCv001.ViewModels
{
    public class MyMessagesViewModel : ViewModelBase, IWindowViewModel
    {
        private readonly MyMessagesModel myMessagesModel;
        public string Title { get => myMessagesModel.Title; }
        public string Message { get => myMessagesModel.Message; }
        public string ButtonText { get => myMessagesModel.ButtonText; }
        public double WindowWidth { get => myMessagesModel.WindowWidth; set => myMessagesModel.WindowWidth = value; }
        public double WindowHeight { get => myMessagesModel.WindowHeight; set => myMessagesModel.WindowHeight = value; }
        public double WindowTop { get => myMessagesModel.WindowTop; set => myMessagesModel.WindowTop = value; }
        public double WindowLeft { get => myMessagesModel.WindowLeft; set => myMessagesModel.WindowLeft = value; }
        public object WindowState { get => myMessagesModel.WindowState; set => myMessagesModel.WindowState = value; }
        public bool Rezult => myMessagesModel.Rezult;

        public ObservableCollection<string> Headers { get => myMessagesModel.Headers; }
        public ObservableCollection<string> ToolTips { get => myMessagesModel.ToolTips; }




        public MyMessagesViewModel()
        {
            myMessagesModel = new();
            myMessagesModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }
        /// <summary>
        /// Заголовок окна. Входной параметр - текст заголовка ( string )
        /// </summary>
        public ICommand SetTitle => setTitle ??= new RelayCommand(myMessagesModel.Execute_SetTitle, myMessagesModel.CanExecute_SetTitle);
        RelayCommand setTitle;



        /// <summary>
        /// Сообщение об ошибке. Входной параметр - текст сообщения (string)
        /// </summary>
        public ICommand SetMessage => setMessage ??= new RelayCommand(myMessagesModel.Execute_SetMessage, myMessagesModel.CanExecute_SetMessage);
        RelayCommand setMessage;
        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand(myMessagesModel.Execute_WindowLoaded, myMessagesModel.CanExecute_WindowLoaded);
        RelayCommand windowLoaded;
        public ICommand SetButtonText => setButtonText ??= new RelayCommand(myMessagesModel.Execute_SetButtonText, myMessagesModel.CanExecute_SetButtonText);
        RelayCommand setButtonText;
        public ICommand ClickOk => clickOk ??= new RelayCommand(myMessagesModel.Execute_ClickOk, myMessagesModel.CanExecute_ClickOk);
        RelayCommand clickOk;
        public ICommand WindowClosing => windowClosing ??= new RelayCommand(myMessagesModel.Execute_WindowClosing, myMessagesModel.CanExecute_WindowClosing);
        RelayCommand windowClosing;

    }
}
