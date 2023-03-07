using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NotebookRCv001.ViewModels
{
    public class FileOverviewViewModel:ViewModelBase, IWindowViewModel
    {
        private readonly FileOverviewModel fileOverviewModel;

        #region _______________Sizes and Positions____________________________________
        public double WindowWidth { get => fileOverviewModel.WindowWidth; set => fileOverviewModel.WindowWidth = value; }
        public double WindowHeight { get => fileOverviewModel.WindowHeight; set => fileOverviewModel.WindowHeight = value; }
        public double WindowTop { get => fileOverviewModel.WindowTop; set => fileOverviewModel.WindowTop = value; }
        public double WindowLeft { get => fileOverviewModel.WindowLeft; set => fileOverviewModel.WindowLeft = value; }
        public object WindowState { get => fileOverviewModel.WindowState; set => fileOverviewModel.WindowState = value; }
        #endregion

        public ObservableCollection<string> Headers => fileOverviewModel.Headers;

        public ObservableCollection<string> ToolTips => fileOverviewModel.ToolTips;



        public FileOverviewViewModel()
        {
            fileOverviewModel = new FileOverviewModel();
            fileOverviewModel.PropertyChanged += ( s, e ) => OnPropertyChanged( e.PropertyName );
        }



        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand
            ( fileOverviewModel.Execute_WindowLoaded, fileOverviewModel.CanExecute_WindowLoaded );
        private RelayCommand windowLoaded;

        public ICommand WindowClosing => windowClosing ??= new RelayCommand
            ( fileOverviewModel.Execute_WindowClosing, fileOverviewModel.CanExecute_WindowClosing );
        private RelayCommand windowClosing;
    }
}
