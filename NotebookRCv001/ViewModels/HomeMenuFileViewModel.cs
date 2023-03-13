using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Models;


namespace NotebookRCv001.ViewModels
{
    public class HomeMenuFileViewModel : ViewModelBase
    {
        internal readonly HomeMenuFileModel HomeMenuFileModel;
        /// <summary>
        /// коллекция с заголовками
        /// </summary>
        public ObservableCollection<string> Headers => HomeMenuFileModel.Headers;
        /// <summary>
        /// коллекция с подсказками
        /// </summary>
        public ObservableCollection<string> ToolTips => HomeMenuFileModel.ToolTips;

        /// <summary>
        /// поддерживаемые расширения файлов
        /// </summary>
        public string[] SupportedFileExtensions => HomeMenuFileModel.SupportedFileExtensions;
        /// <summary>
        /// путь к текущему рабочему каталогу сохранения файла
        /// </summary>
        public string CurrentDirectorySave { get => HomeMenuFileModel.CurrentDirectorySave; set => HomeMenuFileModel.CurrentDirectorySave = value; }
        /// <summary>
        /// путь к текущему рабочему каталогу открытия файла
        /// </summary>
        public string CurrentDirectoryOpen { get => HomeMenuFileModel.CurrentDirectoryOpen; set => HomeMenuFileModel.CurrentDirectoryOpen = value; }
        /// <summary>
        /// путь к последнему открытому или сохраненному файлу 
        /// </summary>
        public string PathToLastFile { get => HomeMenuFileModel.PathToLastFile; set => HomeMenuFileModel.PathToLastFile = value; }
        /// <summary>
        /// Имя последнего открытого или сохраненного файла
        /// </summary>
        public string LastFileName => HomeMenuFileModel.LastFileName;


        /// <summary>
        /// рабочая директория
        /// </summary>
        public string WorkingDirectory 
        { 
            get => HomeMenuFileModel.WorkingDirectory; 
            set => HomeMenuFileModel.WorkingDirectory = value; 
        }

        public string WorkingDirectoryName => HomeMenuFileModel.WorkingDirectoryName;


        public HomeMenuFileViewModel()
        {
            HomeMenuFileModel = new HomeMenuFileModel();
            HomeMenuFileModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// окно открытия файла(входные параметры: фильтр(string);путь к исходному каталогу(string). Выходной параметр: путь для открытия(string))
        /// </summary>
        public Func<string, string, string> OpenFileDialog => HomeMenuFileModel.OpenFileDialog;
        /// <summary>
        /// окно сохранения файла (входные параметры: фильтр(string);путь к исходному каталогу(string). Выходной параметр: путь для сохранения(string))
        /// </summary>
        public Func<string, string, string> SaveFileDialog => HomeMenuFileModel.SaveFileDialog;


        public ICommand NewFile => newFile ??= new RelayCommand(HomeMenuFileModel.Execute_NewFile, HomeMenuFileModel.CanExecute_NewFile);
        private RelayCommand newFile;
        public ICommand OpenFile => openFile ??= new RelayCommand(HomeMenuFileModel.Execute_OpenFile, HomeMenuFileModel.CanExecute_OpenFile);
        private RelayCommand openFile;
        public ICommand SaveFile => saveFile ??= new RelayCommand(HomeMenuFileModel.Execute_SaveFile, HomeMenuFileModel.CanExecute_SaveFile);
        private RelayCommand saveFile;
        public ICommand SaveAsFile => saveAsFile ??= new RelayCommand(HomeMenuFileModel.Execute_SaveAsFile, HomeMenuFileModel.CanExecute_SaveAsFile);
        private RelayCommand saveAsFile;

        public ICommand UploadingFiles => uploadingFiles ??= new RelayCommand(HomeMenuFileModel.Execute_UploadingFiles,
            HomeMenuFileModel.CanExecute_UploadingFiles);
        private RelayCommand uploadingFiles;

        public ICommand FileOverview => fileOverview ??= new RelayCommand( HomeMenuFileModel.Execute_FileOverview,
            HomeMenuFileModel.CanExecute_FileOverview );
        private RelayCommand fileOverview;
        public ICommand SelectingAWorkingDirectory => selectingAWorkingDirectory ??= new RelayCommand(
            HomeMenuFileModel.Execute_SelectingAWorkingDirectory, HomeMenuFileModel.CanExecute_SelectingAWorkingDirectory);
        private RelayCommand selectingAWorkingDirectory;

        /// <summary>
        /// синхронизация рабочего каталога со сторонним
        /// </summary>
        public ICommand WorkingDirectorySynchronization => workingDirectorySynchronization ??= new RelayCommand(
            HomeMenuFileModel.WorkingDirectorySynchronization_Execute, HomeMenuFileModel.WorkingDirectorySynchronization_CanExecute);
        private RelayCommand workingDirectorySynchronization;

    }
}
