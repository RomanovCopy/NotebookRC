using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NotebookRCv001.Models;
using NotebookRCv001.Infrastructure;

namespace NotebookRCv001.ViewModels
{
    public class HomeMenuEncryptionViewModel : ViewModelBase
    {
        public readonly HomeMenuEncryptionModel HomeMenuEncryptionModel;

        public ObservableCollection<string> Headers => HomeMenuEncryptionModel.Headers;
        public ObservableCollection<string> ToolTips => HomeMenuEncryptionModel.ToolTips;

        public string KeyCript { get => HomeMenuEncryptionModel.KeyCrypt; }

        public string EncryptionStatus { get => HomeMenuEncryptionModel.EncryptionStatus; }


        public HomeMenuEncryptionViewModel()
        {
            HomeMenuEncryptionModel = new HomeMenuEncryptionModel();
            HomeMenuEncryptionModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// шифрование
        /// </summary>
        public ICommand DeleteKey => deleteKey ??= new RelayCommand(HomeMenuEncryptionModel.Execute_DeleteKey, HomeMenuEncryptionModel.CanExecute_DeleteKey);
        private RelayCommand deleteKey;


        /// <summary>
        /// установка ключа шифрования
        /// </summary>
        public ICommand InstalKey => installKey ??= new RelayCommand(HomeMenuEncryptionModel.Execute_InstalKey, HomeMenuEncryptionModel.CanExecute_InstalKey);
        private RelayCommand installKey;
    }
}
