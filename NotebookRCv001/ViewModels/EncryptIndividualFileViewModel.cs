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
    public class EncryptIndividualFileViewModel:ViewModelBase, IPageViewModel
    {
        private readonly EncryptIndividualFileModel encryptIndividualFileModel;
        public ObservableCollection<string> Headers => encryptIndividualFileModel.Headers;
        public ObservableCollection<string> ToolTips => encryptIndividualFileModel.ToolTips;
        public Action BehaviorReady { get => encryptIndividualFileModel.BehaviorReady; set => encryptIndividualFileModel.BehaviorReady = value; }


        public EncryptIndividualFileViewModel()
        {
            encryptIndividualFileModel = new();
            encryptIndividualFileModel.PropertyChanged += ( s, e ) => OnPropertyChanged( e.PropertyName );
        }


        public ICommand PageLoaded => pageLoaded ??= new RelayCommand( encryptIndividualFileModel.Execute_PageLoaded, encryptIndividualFileModel.CanExecute_PageLoaded );
        RelayCommand pageLoaded;

        public ICommand PageClose => pageClose ??= new RelayCommand( encryptIndividualFileModel.Execute_PageClose, 
            encryptIndividualFileModel.CanExecute_PageClose );
        RelayCommand pageClose;

        public ICommand PageClear => pageClear ??= new RelayCommand( encryptIndividualFileModel.Execute_PageClear, 
            encryptIndividualFileModel.CanExecute_PageClear );
        RelayCommand pageClear;
    }
}
