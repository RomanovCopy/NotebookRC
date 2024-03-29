﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    public class EncryptIndividualFileViewModel:ViewModelBase, IPageViewModel, IDisplayProgressTarget
    {
        private readonly EncryptIndividualFileModel encryptIndividualFileModel;
        public ObservableCollection<string> Headers => encryptIndividualFileModel.Headers;
        public ObservableCollection<string> ToolTips => encryptIndividualFileModel.ToolTips;
        public Action<object> BehaviorReady { get => encryptIndividualFileModel.BehaviorReady; set => encryptIndividualFileModel.BehaviorReady = value; }

        /// <summary>
        /// путь к открываемому файлу
        /// </summary>
        public string PathToOpenFile { get => encryptIndividualFileModel.PathToOpenFile; set => encryptIndividualFileModel.PathToOpenFile = value; }

        /// <summary>
        /// имя открываемого для шифрования/дешифрования файла
        /// </summary>
        public string NameOpenFile => encryptIndividualFileModel.NameOpenFile;
        /// <summary>
        /// путь к сохраняемому файлу
        /// </summary>
        public string PathToSaveFile { get => encryptIndividualFileModel.PathToSaveFile; set => encryptIndividualFileModel.PathToSaveFile = value; }
        /// <summary>
        /// имя сохраняемого после шифрования/дешифрования файла
        /// </summary>
        public string NameSaveFile { get => encryptIndividualFileModel.NameSaveFile; set => encryptIndividualFileModel.NameSaveFile = value; }
        /// <summary>
        /// путь к открываемой директории
        /// </summary>
        public string PathToOpenDirectory { get => encryptIndividualFileModel.PathToOpenDirectory; 
            set => encryptIndividualFileModel.PathToOpenDirectory = value; }
        /// <summary>
        /// имя открываемого для шифрования/дешифрования каталога
        /// </summary>
        public string NameOpenDirectory { get => encryptIndividualFileModel.NameOpenDirectory;
            set => encryptIndividualFileModel.NameOpenDirectory = value; }
        /// <summary>
        /// путь к сохраняемой директории
        /// </summary>
        public string PathToSaveDirectory { get => encryptIndividualFileModel.PathToSaveDirectory; 
            set => encryptIndividualFileModel.PathToSaveDirectory = value; }
        /// <summary>
        /// имя директории для сохранения зашфрованного/дешифрованного каталога
        /// </summary>
        public string NameSaveDirectory { get => encryptIndividualFileModel.NameSaveDirectory;
            set => encryptIndividualFileModel.NameSaveDirectory = value; }


        public double ProgressValue { get => encryptIndividualFileModel.ProgressValue;
            set => encryptIndividualFileModel.ProgressValue = value; }



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

        public ICommand SelectSaveFile => selectSaveFile ??= new RelayCommand(encryptIndividualFileModel.Execute_SelectSaveFile,
            encryptIndividualFileModel.CanExecute_SelectSaveFile);
        RelayCommand selectSaveFile;

        public ICommand ClearSaveFile => clearSaveFile ??= new RelayCommand(encryptIndividualFileModel.Execute_ClearSaveFile,
            encryptIndividualFileModel.CanExecute_ClearSaveFile);
        RelayCommand clearSaveFile;
        public ICommand SelectOpenDirectory => selectOpenDirectory ??= new RelayCommand( encryptIndividualFileModel.Execute_SelectOpenDirectory,
            encryptIndividualFileModel.CanExecute_SelectOpenDirectory );
        private RelayCommand selectOpenDirectory;
        public ICommand ClearOpenDirectory => clearOpenDirectory ??= new RelayCommand( encryptIndividualFileModel.Execute_ClearOpenDirectory,
            encryptIndividualFileModel.CanExecute_ClearOpenDirectory);
        private RelayCommand clearOpenDirectory;

        public ICommand SelectSaveDirectory => selectSaveDirectory ??= new RelayCommand(encryptIndividualFileModel.Execute_SelectSaveDirectory,
            encryptIndividualFileModel.CanExecute_SelectSaveDirectory);
        RelayCommand selectSaveDirectory;

        public ICommand ClearSaveDirectory => clearSaveDirectory ??= new RelayCommand(encryptIndividualFileModel.Execute_ClearSaveDirectory,
            encryptIndividualFileModel.CanExecute_ClearSaveDirectory);
        RelayCommand clearSaveDirectory;


        public ICommand ClickButtonEncrypt => clickButtonEncrypt ??= new RelayCommand( encryptIndividualFileModel.Execute_ClickButtonEncrypt,
            encryptIndividualFileModel.CanExecute_ClickButtonEncrypt );
        RelayCommand clickButtonEncrypt;

        public ICommand ClickButtonCancel => clickButtonCancel ??= new RelayCommand(encryptIndividualFileModel.Execute_ClickButtonCancel,
            encryptIndividualFileModel.CanExecute_ClickButtonCancel);
        private RelayCommand clickButtonCancel;

        public ICommand ClickButtonDecrypt => clickButtonDecrypt ??= new RelayCommand( encryptIndividualFileModel.Execute_ClickButtonDecrypt,
            encryptIndividualFileModel.CanExecute_ClickButtonDecrypt );
        RelayCommand clickButtonDecrypt;

        public ICommand SubfoldersCheckBox => subfoldersCheckBox ??= new RelayCommand(encryptIndividualFileModel.Execute_SubfoldersCheckBox,
            encryptIndividualFileModel.CanExecute_SubfoldersCheckBox);
        private RelayCommand subfoldersCheckBox;

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
