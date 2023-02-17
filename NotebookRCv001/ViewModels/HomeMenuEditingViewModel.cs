using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class HomeMenuEditingViewModel:ViewModelBase
    {
        private readonly HomeMenuEditingModel homeMenuEditingModel;
        public ObservableCollection<string> Headers => homeMenuEditingModel.Headers;

        public ObservableCollection<string> ToolTips => homeMenuEditingModel.ToolTips;

        public HomeMenuEditingViewModel()
        {
            homeMenuEditingModel = new HomeMenuEditingModel();
            homeMenuEditingModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }


        public ICommand InsertImage => insertImage ??= new RelayCommand(homeMenuEditingModel.Execute_InsertImage, homeMenuEditingModel.CanExecute_InsertImage);
        RelayCommand insertImage;


        public ICommand PageLoaded => throw new NotImplementedException();

        public ICommand PageClose => throw new NotImplementedException();

        public ICommand PageClear => throw new NotImplementedException();
    }
}
