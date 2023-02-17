using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class InputWindowViewModel:ViewModelBase
    {
        public readonly InputWindowModel inputWindowModel;

        #region ________________________Положение и размеры главного окна___________________
        public double WindowWidth
        {
            get => inputWindowModel.WindowWidth;
            set => inputWindowModel.WindowWidth = value;
        }
        public double WindowHeight
        {
            get => inputWindowModel.WindowHeight;
            set => inputWindowModel.WindowHeight = value;
        }
        public double WindowTop
        {
            get => inputWindowModel.WindowTop;
            set => inputWindowModel.WindowTop = value;
        }
        public double WindowLeft
        {
            get => inputWindowModel.WindowLeft;
            set => inputWindowModel.WindowLeft = value;
        }

        #endregion

        public Languages Language => inputWindowModel.Language;
        public ObservableCollection<string> Headers => inputWindowModel.Headers;
        public ObservableCollection<string> ToolTips => inputWindowModel.ToolTips;
        public string KeyCrypt => inputWindowModel.KeyCrypt;

        public InputWindowViewModel()
        {
            inputWindowModel = new InputWindowModel();
            inputWindowModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }

        public ICommand PasswordOneLoaded => passwordOneLoaded ??= new RelayCommand(inputWindowModel.Execute_PasswordOneLoaded,
            inputWindowModel.CanExecute_PasswordOneLoaded);
        RelayCommand passwordOneLoaded;

        public ICommand PasswordTwoLoaded => passwordTwoLoaded ??= new RelayCommand(inputWindowModel.Execute_PasswordTwoLoaded,
            inputWindowModel.CanExecute_PasswordTwoLoaded);
        RelayCommand passwordTwoLoaded;


        public ICommand InputWindowLoaded => inputWindowLoaded ??= new RelayCommand(inputWindowModel.Execute_InputWindowLoaded,
            inputWindowModel.CanExecute_InputWindowLoaded);
        RelayCommand inputWindowLoaded;

        public ICommand InputWindowClosing => inputWindowClosing ??= new RelayCommand(inputWindowModel.Execute_InputWindowClosing,
             inputWindowModel.CanExecute_InputWindowClosing);
        RelayCommand inputWindowClosing;
    }
}
