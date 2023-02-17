using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using NotebookRCv001.Models;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;

namespace NotebookRCv001.ViewModels
{
    public class PopUpTextBoxViewModel:ViewModelBase, IWindowViewModel
    {
        private readonly PopUpTextBoxModel popUpTextBoxModel;
        public double WindowWidth { get => popUpTextBoxModel.WindowWidth; set => popUpTextBoxModel.WindowWidth=value; }
        public double WindowHeight { get => popUpTextBoxModel.WindowHeight; set => popUpTextBoxModel.WindowHeight = value; }
        public double WindowTop { get => popUpTextBoxModel.WindowTop; set => popUpTextBoxModel.WindowTop = value; }
        public double WindowLeft { get => popUpTextBoxModel.WindowLeft; set => popUpTextBoxModel.WindowLeft=value; }
        public object WindowState { get => popUpTextBoxModel.WindowState; set => popUpTextBoxModel.WindowState = value; }


        public ObservableCollection<string> Headers => popUpTextBoxModel.Headers;
        public ObservableCollection<string> ToolTips => popUpTextBoxModel.ToolTips;

        /// <summary>
        /// заголовок окна
        /// </summary>
        public string Title { get => popUpTextBoxModel.Title; set => popUpTextBoxModel.Title = value; }
        /// <summary>
        /// заголовок TextBox
        /// </summary>
        public string Header { get => popUpTextBoxModel.Header; set => popUpTextBoxModel.Header = value; }
        /// <summary>
        /// подсказка для окна
        /// </summary>
        public string ToolTip { get => popUpTextBoxModel.ToolTip; set => popUpTextBoxModel.ToolTip = value; }

        public string Text { get => popUpTextBoxModel.Text; set => popUpTextBoxModel.Text = value; }

        public PopUpTextBoxViewModel()
        {
            popUpTextBoxModel = new();
            popUpTextBoxModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }


        /// <summary>
        /// событие: окончание загрузки TextBox
        /// </summary>
        public ICommand TextBoxLoaded => textBoxLoaded ??= new RelayCommand(popUpTextBoxModel.Execute_TextBoxLoaded,
            popUpTextBoxModel.CanExecute_TextBoxLoaded);
        private RelayCommand textBoxLoaded;

        /// <summary>
        /// событие: окончание загрузки окна
        /// </summary>
        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand(popUpTextBoxModel.Execute_WindowLoaded, 
            popUpTextBoxModel.CanExecute_WindowLoaded);
        private RelayCommand windowLoaded;

        /// <summary>
        /// событие: перед закрытием окна
        /// </summary>
        public ICommand WindowClosing => windowClosing ??= new RelayCommand(popUpTextBoxModel.Execute_WindowClosing, 
            popUpTextBoxModel.CanExecute_WindowClosing);
        private RelayCommand windowClosing;
    }
}
