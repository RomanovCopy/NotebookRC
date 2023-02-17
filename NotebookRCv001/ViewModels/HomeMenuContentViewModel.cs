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
    public class HomeMenuContentViewModel:ViewModelBase
    {
        private readonly HomeMenuContentModel homeMenuContentModel;
        public ObservableCollection<string> Headers => homeMenuContentModel.Headers;

        public ObservableCollection<string> ToolTips => homeMenuContentModel.ToolTips;


        public HomeMenuContentViewModel()
        {
            homeMenuContentModel = new HomeMenuContentModel();
            homeMenuContentModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }

        public ICommand Reading => reading ??= new RelayCommand(homeMenuContentModel.Execute_Reading, homeMenuContentModel.CanExecute_Reading);
        RelayCommand reading;

        public ICommand InsertImage => insertImage ??= new RelayCommand(homeMenuContentModel.Execute_InsertImage, homeMenuContentModel.CanExecute_InsertImage);
        RelayCommand insertImage;

        public ICommand InsertText => insertText ??= new RelayCommand(homeMenuContentModel.Execute_InsertText, homeMenuContentModel.CanExecute_InsertText);
        RelayCommand insertText;

        public ICommand OpenDocumentTree => openDocumentTree ??= new RelayCommand(homeMenuContentModel.Execute_OpenDocumentTree,
            homeMenuContentModel.CanExecute_OpenDocumentTree);
        RelayCommand openDocumentTree;

        public ICommand PageLoaded => throw new NotImplementedException();

        public ICommand PageClose => throw new NotImplementedException();

        public ICommand PageClear => throw new NotImplementedException();
    }
}
