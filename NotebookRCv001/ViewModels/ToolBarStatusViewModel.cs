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
    public class ToolBarStatusViewModel:ViewModelBase
    {

        private readonly ToolBarStatusModel toolBarStatusModel;

        public string HomeEncoding => toolBarStatusModel.HomeEncoding;
        public string EncryptionStatus => toolBarStatusModel.EncryptionStatus;
        public string WorkingDirectory => toolBarStatusModel.WorkingDirectory;
        public string WorkingDirectoryName => toolBarStatusModel.WorkingDirectoryName;
        public ObservableCollection<string> Headers => toolBarStatusModel.Headers;
        public ObservableCollection<string> ToolTips => toolBarStatusModel.ToolTips;




        public ToolBarStatusViewModel()
        {
            toolBarStatusModel = new ToolBarStatusModel();
            toolBarStatusModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }




        public ICommand ToolBarStatusLoaded => toolBarStatusLoaded ??= new RelayCommand(toolBarStatusModel.Execute_ToolBarStatusLoaded, toolBarStatusModel.CanExecute_ToolBarStatusLoaded);
        RelayCommand toolBarStatusLoaded;

    }
}
