using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace NotebookRCv001.Interfaces
{
    public interface IWindowViewModel
    {
        public double WindowWidth { get; set; }
        public double WindowHeight { get; set; }
        public double WindowTop { get; set; }
        public double WindowLeft { get; set; }
        public object WindowState { get; set; }
        public ObservableCollection<string> Headers { get; }
        public ObservableCollection<string> ToolTips { get; }


        public ICommand WindowLoaded { get; }
        public ICommand WindowClosing { get; }
    }
}
