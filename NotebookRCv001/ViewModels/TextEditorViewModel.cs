using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class TextEditorViewModel : ViewModelBase, IWindowViewModel, ILastFileName
    {
        private readonly TextEditorModel textEditorModel;

        public double WindowWidth { get => textEditorModel.WindowWidth; set => textEditorModel.WindowWidth = value; }
        public double WindowHeight { get => textEditorModel.WindowHeight; set => textEditorModel.WindowHeight = value; }
        public double WindowTop { get => textEditorModel.WindowTop; set => textEditorModel.WindowTop = value; }
        public double WindowLeft { get => textEditorModel.WindowLeft; set => textEditorModel.WindowLeft=value; }
        public object WindowState { get => textEditorModel.WindowState; set => textEditorModel.WindowState = value; }

        public ObservableCollection<string> Headers => textEditorModel.Headers;
        public ObservableCollection<string> ToolTips => textEditorModel.ToolTips;

        public FlowDocument Document => textEditorModel.Document;


        public string PathToLastFile { get => textEditorModel.PathToLastFile; set => textEditorModel.PathToLastFile = value; }
        public string LastFileName { get => textEditorModel.LastFileName; set => textEditorModel.LastFileName = value; }


        public TextEditorViewModel()
        {
            textEditorModel = new TextEditorModel();
            textEditorModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand(textEditorModel.Execute_WindowLoaded, textEditorModel.CanExecute_WindowLoaded);
        private RelayCommand windowLoaded;

        public ICommand WindowClosing => windowClosing??=new RelayCommand(textEditorModel.Execute_WindowClosing, textEditorModel.CanExecute_WindowClosing);
        private RelayCommand windowClosing;
    }
}
