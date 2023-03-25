using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Drawing = System.Drawing;
using Media = System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.ViewModels;

namespace NotebookRCv001.Models
{
    public class MenuHomeModel:ViewModelBase
    {
        private HomeMenuFileViewModel homeMenuFileViewModel { get; set; }
        private HomeMenuSettingsViewModel homeMenuSettingsViewModel { get; set; }
        private HomeMenuEncryptionViewModel homeMenuEncryptionViewModel { get; set; }


        internal ObservableCollection <Drawing.Color> HomeMyColors { get => homeMenuSettingsViewModel.HomeMyColors;}
        internal Media.Brush MyForeground { get => homeMenuSettingsViewModel.MyForeground; 
            set => homeMenuSettingsViewModel.MyForeground = value; }
        internal Media.Brush MyFontBackground { get => homeMenuSettingsViewModel.MyFontBackground; 
            set => homeMenuSettingsViewModel.MyFontBackground = value; }
        internal Media.Brush MyBackground { get => homeMenuSettingsViewModel.MyBackground; 
            set => homeMenuSettingsViewModel.MyBackground = value; }
        internal Encoding HomeEncoding { get => homeMenuSettingsViewModel.HomeEncoding;}
        internal string EncryptionStatus { get => homeMenuEncryptionViewModel.EncryptionStatus;}
        internal string WorkingDirectory { get => homeMenuFileViewModel.WorkingDirectory; 
            set => homeMenuFileViewModel.WorkingDirectory = value; }
        internal string WorkingDirectoryName { get => homeMenuFileViewModel.WorkingDirectoryName; }
        internal string PathToLastFile { get => homeMenuFileViewModel?.PathToLastFile; 
            set => homeMenuFileViewModel.PathToLastFile = value; }
        internal string LastFileName { get => homeMenuFileViewModel?.LastFileName; }
        internal string CurrentDirectoryOpen { get => homeMenuFileViewModel.CurrentDirectoryOpen; 
            set => homeMenuFileViewModel.CurrentDirectoryOpen = value; }
        internal string CurrentDirectorySave { get => homeMenuFileViewModel.CurrentDirectorySave; 
            set => homeMenuFileViewModel.CurrentDirectorySave = value; }


        public MenuHomeModel()
        {

        }

        /// <summary>
        /// окончание загрузки Menu
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_MenuLoaded( object obj )
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_MenuLoaded( object obj )
        {
            try
            {
                if(obj is MyControls.MenuHome menuhome)
                {
                    homeMenuFileViewModel = (HomeMenuFileViewModel)menuhome.FindResource("menufile");
                    homeMenuFileViewModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
                    homeMenuEncryptionViewModel = (HomeMenuEncryptionViewModel)menuhome.FindResource("menuencryption");
                    homeMenuEncryptionViewModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
                    homeMenuSettingsViewModel = (HomeMenuSettingsViewModel)menuhome.FindResource("menusettings");
                    homeMenuSettingsViewModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
                    homeMenuSettingsViewModel.HomeEncoding = Encoding.GetEncoding(Properties.Settings.Default.EncodingCodePage);
                    homeMenuFileViewModel.CurrentDirectoryOpen = Properties.Settings.Default.HomeCurrentDirectoryOpen;
                    homeMenuFileViewModel.CurrentDirectorySave = Properties.Settings.Default.HomeCurrentDirectorySave;
                    homeMenuFileViewModel.WorkingDirectory = Properties.Settings.Default.WorkingDirectory;
                    OnPropertyChanged(new string[] { "WorkingDirectoryName", "HomeEncoding", "EncryptionStatus" });
                }
                foreach (EncodingInfo info in Encoding.GetEncodings())
                    homeMenuSettingsViewModel.HomeEncodings.Add(info);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

    }
}
