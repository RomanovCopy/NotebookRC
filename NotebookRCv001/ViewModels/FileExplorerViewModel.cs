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
    public class FileExplorerViewModel:ViewModelBase, IPageViewModel
    {
        private readonly FileExplorerModel fileExplorerModel;

        public ObservableCollection<string> Headers => fileExplorerModel.Headers;
        public ObservableCollection<string> ToolTips => fileExplorerModel.ToolTips;
        public Action<object> BehaviorReady { get => fileExplorerModel.BehaviorReady;
            set => fileExplorerModel.BehaviorReady = value; }



        public FileExplorerViewModel()
        {
            fileExplorerModel = new();
            fileExplorerModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);

        }


        public ICommand PageLoaded => pageLoaded ??= new RelayCommand(fileExplorerModel.Execute_PageLoaded,
            fileExplorerModel.CanExecute_PageLoaded);
        private RelayCommand pageLoaded;

        public ICommand PageClose => pageClose ??= new RelayCommand(fileExplorerModel.Execute_PageClose,
            fileExplorerModel.CanExecute_PageClose);
        private RelayCommand pageClose;

        public ICommand PageClear => pageClear ??= new RelayCommand(fileExplorerModel.Execute_PageClear,
            fileExplorerModel.CanExecute_PageClear);
        private RelayCommand pageClear;
    }
}
