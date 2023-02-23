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

        /// <summary>
        /// путь к открываемому файлу
        /// </summary>
        public string PathToOpenFile { get => encryptIndividualFileModel.PathToOpenFile; set => encryptIndividualFileModel.PathToOpenFile = value; }
        /// <summary>
        /// путь к сохраняемому файлу
        /// </summary>
        public string PathToSaveFile { get => encryptIndividualFileModel.PathToSaveFile; set => encryptIndividualFileModel.PathToSaveFile = value; }
        /// <summary>
        /// путь к открываемой директории
        /// </summary>
        public string PathToOpenDirectory { get => encryptIndividualFileModel.PathToOpenDirectory; 
            set => encryptIndividualFileModel.PathToOpenDirectory = value; }
        /// <summary>
        /// путь к сохраняемой директории
        /// </summary>
        public string PathToSaveDirectory { get => encryptIndividualFileModel.PathToSaveDirectory; 
            set => encryptIndividualFileModel.PathToSaveDirectory = value; }



        public EncryptIndividualFileViewModel()
        {
            encryptIndividualFileModel = new();
            encryptIndividualFileModel.PropertyChanged += ( s, e ) => OnPropertyChanged( e.PropertyName );
        }

        public ICommand SelectOpenFile => selectOpenFile ??= new RelayCommand( encryptIndividualFileModel.Execute_SelectOpenFile,
            encryptIndividualFileModel.CanExecute_SelectOpenFile );
        RelayCommand selectOpenFile;
        public ICommand ClearOpenFile => clearOpenFile ??= new RelayCommand( encryptIndividualFileModel.Execute_ClearOpenFile,
            encryptIndividualFileModel.CanExecute_ClearOpenFile );
        private RelayCommand clearOpenFile;
        public ICommand SelectOpenDirectory => selectOpenDirectory ??= new RelayCommand( encryptIndividualFileModel.Execute_SelectOpenDirectory,
            encryptIndividualFileModel.CanExecute_SelectOpenDirectory );
        private RelayCommand selectOpenDirectory;
        public ICommand ClearOpenDirectory => clearOpenDirectory ??= new RelayCommand( encryptIndividualFileModel.Execute_ClearOpenDirectory,
            encryptIndividualFileModel.CanExecute_ClearOpenDirectory);
        private RelayCommand clearOpenDirectory;



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
