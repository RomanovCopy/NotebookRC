using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using System.Collections.ObjectModel;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class DisplayProgressViewModel:ViewModelBase, IWindowViewModel
    {
        private readonly DisplayProgressModel displayProgressModel;
        public object Target { get => displayProgressModel.Target; set => displayProgressModel.Target = value; }
        public double WindowWidth { get => displayProgressModel.WindowWidth; set => displayProgressModel.WindowWidth=value; }
        public double WindowHeight { get => displayProgressModel.WindowHeight; set => displayProgressModel.WindowHeight = value; }
        public double WindowTop { get => displayProgressModel.WindowTop; set => displayProgressModel.WindowTop=value; }
        public double WindowLeft { get => displayProgressModel.WindowLeft; set => displayProgressModel.WindowLeft=value; }
        public object WindowState { get => displayProgressModel.WindowState; set => displayProgressModel.WindowState=value; }

        public double ProgressValue { get => displayProgressModel.ProgressValue; set => displayProgressModel.ProgressValue = value; }

        public ObservableCollection<string> Headers => displayProgressModel.Headers;

        public ObservableCollection<string> ToolTips => displayProgressModel.ToolTips;

        public DisplayProgressViewModel()
        {
            displayProgressModel = new DisplayProgressModel();
            displayProgressModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }


        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand(displayProgressModel.Execute_WindowLoaded);
        private RelayCommand windowLoaded;

        public ICommand WindowClosing => windowClosing ??= new RelayCommand(displayProgressModel.Execute_WindowClosing);
        private RelayCommand windowClosing;


    }
}
