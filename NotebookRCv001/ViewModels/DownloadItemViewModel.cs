using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.IO;

namespace NotebookRCv001.ViewModels
{
    public class DownloadItemViewModel : ViewModelBase, IDownloadItem
    {
        private readonly DownloadItemModel downloadItemModel;

        public string SuggestedFileName { get => downloadItemModel.SuggestedFileName; set => downloadItemModel.SuggestedFileName = value; }
        public string OriginalUrl { get => downloadItemModel.OriginalUrl; set => downloadItemModel.OriginalUrl = value; }
        public string Url { get => downloadItemModel.Url; set => downloadItemModel.Url = value; }
        public string Id { get => downloadItemModel.Id; set => downloadItemModel.Id = value; }
        public string FullPath { get => downloadItemModel.FullPath; set => downloadItemModel.FullPath = value; }
        /// <summary>
        /// флаг : размер файла не определен ( для ProgressBar )
        /// </summary>
        public bool FileSizeUnknown { get => downloadItemModel.FileSizeUnknown; }
        /// <summary>
        /// имя каталога в который загружается файл
        /// </summary>
        public string DirectoryName { get => downloadItemModel.DirectoryName; }
        public DateTime? EndTime { get => downloadItemModel.EndTime; set =>downloadItemModel.EndTime=value; }
        public DateTime? StartTime { get => downloadItemModel.StartTime; set => downloadItemModel.StartTime=value; }
        public long ReceivedBytes { get => downloadItemModel.ReceivedBytes ; set => downloadItemModel.ReceivedBytes=value ; }
        public long TotalBytes { get => downloadItemModel.TotalBytes; set => downloadItemModel.TotalBytes = value; }
        public int PercentComplete { get => downloadItemModel.PercentComplete; set => downloadItemModel.PercentComplete = value; }
        public long CurrentSpeed {  get => downloadItemModel.CurrentSpeed;  set => downloadItemModel.CurrentSpeed = value;  }
        public bool IsCancelled { get => downloadItemModel.IsCancelled; set => downloadItemModel.IsCancelled = value; }
        public bool IsComplete { get => downloadItemModel.IsComplete; set => downloadItemModel.IsComplete = value; }
        public bool IsInProgress { get => downloadItemModel.IsInProgress; set => downloadItemModel.IsInProgress = value; }
        public bool IsValid { get => downloadItemModel.IsValid; set => downloadItemModel.IsValid = value; }
        public string ContentDisposition { get => downloadItemModel.ContentDisposition; set => downloadItemModel.ContentDisposition = value; }
        public string MimeType { get => downloadItemModel.MimeType ; set => downloadItemModel.MimeType=value ; }
        public string Status { get => downloadItemModel.Status; }


        public DownloadItemViewModel()
        {
            downloadItemModel = new DownloadItemModel();
            downloadItemModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        public ICommand Preparation => preparation ??= new RelayCommand(downloadItemModel.Execute_Preparation, downloadItemModel.CanExecute_Preparation);
        private RelayCommand preparation;
        public ICommand Start => start ??= new RelayCommand(downloadItemModel.Execute_Start, downloadItemModel.CanExecute_Start);
        private RelayCommand start;
        public ICommand Pause => pause ??= new RelayCommand(downloadItemModel.Execute_Pause, downloadItemModel.CanExecute_Pause);
        private RelayCommand pause;
        public ICommand Reload => reload ??= new RelayCommand(downloadItemModel.Execute_Reload, downloadItemModel.CanExecute_Reload);
        private RelayCommand reload;
        /// <summary>
        /// переименование загружаемого файла
        /// </summary>
        public ICommand RenameTheFileYouWantToDownload => renameTheFileYouWantToDownload ??= new RelayCommand(
            downloadItemModel.Execute_RenameTheFileYouWantToDownload, downloadItemModel.CanExecute_RenameTheFileYouWantToDownload);
        private RelayCommand renameTheFileYouWantToDownload;
        public ICommand Remove => remove ??= new RelayCommand(downloadItemModel.Execute_Remove, downloadItemModel.CanExecute_Remove);
        private RelayCommand remove;
        public ICommand Stop => stop ??= new RelayCommand(downloadItemModel.Execute_Stop, downloadItemModel.CanExecute_Stop);
        private RelayCommand stop;
        public ICommand Close => close ??= new RelayCommand(downloadItemModel.Execute_Close, downloadItemModel.CanExecute_Close);
        private RelayCommand close;

    }
}
