using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drawing = System.Drawing;
using Media = System.Windows.Media;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class MenuHomeViewModel:ViewModelBase
    {
        private readonly MenuHomeModel menuHomeModel;


        public Encoding HomeEncoding => menuHomeModel.HomeEncoding;
        public ObservableCollection<Drawing.Color> HomeMyColors { get => menuHomeModel.HomeMyColors; }
        public Media.Brush MyForeground { get => menuHomeModel.MyForeground; set => menuHomeModel.MyForeground = value; }
        public Media.Brush MyFontBackground { get => menuHomeModel.MyFontBackground; set => menuHomeModel.MyFontBackground = value; }
        public Media.Brush MyBackground { get => menuHomeModel.MyBackground; set => menuHomeModel.MyBackground = value; }
        public string EncryptionStatus { get => menuHomeModel.EncryptionStatus; }
        public string WorkingDirectory { get => menuHomeModel.WorkingDirectory; set => menuHomeModel.WorkingDirectory = value; }
        public string WorkingDirectoryName { get => menuHomeModel.WorkingDirectoryName; }
        public string PathToLastFile { get => menuHomeModel.PathToLastFile; set => menuHomeModel.PathToLastFile = value; }
        public string LastFileName { get => menuHomeModel.LastFileName; }
        public string CurrentDirectoryOpen { get => menuHomeModel.CurrentDirectoryOpen; set => menuHomeModel.CurrentDirectoryOpen = value; }
        public string CurrentDirectorySave { get => menuHomeModel.CurrentDirectorySave; set => menuHomeModel.CurrentDirectorySave = value; }


        public ObservableCollection<string> Headers => throw new NotImplementedException();

        public ObservableCollection<string> ToolTips => throw new NotImplementedException();



        public MenuHomeViewModel()
        {
            menuHomeModel = new();
            menuHomeModel.PropertyChanged += ( s, e ) => OnPropertyChanged(e.PropertyName);
        }



        public ICommand MenuLoaded => menuLoaded ??= new RelayCommand(menuHomeModel.Execute_MenuLoaded, menuHomeModel.CanExecute_MenuLoaded);
        RelayCommand menuLoaded;

        public ICommand ControlLoaded => throw new NotImplementedException();

        public ICommand ControlClose => throw new NotImplementedException();

    }
}
