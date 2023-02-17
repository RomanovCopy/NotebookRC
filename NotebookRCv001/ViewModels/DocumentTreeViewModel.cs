using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class DocumentTreeViewModel : ViewModelBase, IWindowViewModel
    {
        private readonly DocumentTreeModel documentTreeModel;


        #region ________________Положение и размеры окна_______________________________

        public double WindowWidth { get => documentTreeModel.WindowWidth; set => documentTreeModel.WindowWidth = value; }
        public double WindowHeight { get => documentTreeModel.WindowHeight; set => documentTreeModel.WindowHeight = value; }
        public double WindowTop { get => documentTreeModel.WindowTop; set => documentTreeModel.WindowTop = value; }
        public double WindowLeft { get => documentTreeModel.WindowLeft; set => documentTreeModel.WindowLeft = value; }
        public object WindowState { get => documentTreeModel.WindowState; set => documentTreeModel.WindowState = value; }

        #endregion

        public ObservableCollection<string> Headers => documentTreeModel.Headers;
        public ObservableCollection<string> ToolTips => documentTreeModel.ToolTips;
        internal object FlowDocumentLastSelected => documentTreeModel.FlowDocumentLastSelected;


        public DocumentTreeViewModel()
        {
            documentTreeModel = new();
            documentTreeModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        public ICommand AddParagraph => addParagraph ??= new RelayCommand(documentTreeModel.Execute_AddParagraph,
            documentTreeModel.CanExecute_AddParagraph);
        RelayCommand addParagraph;

        public ICommand AddFigure => addFigure ??= new RelayCommand(documentTreeModel.Execute_AddFigure,
            documentTreeModel.CanExecute_AddFigure);
        RelayCommand addFigure;

        public ICommand AddIUIContainer => addIUIContainer ??= new RelayCommand(documentTreeModel.Execute_AddIUIContainer,
            documentTreeModel.CanExecute_AddIUIContainer);
        RelayCommand addIUIContainer;

        public ICommand AddBUIContainer => addBUIContainer ??= new RelayCommand(documentTreeModel.Execute_AddBUIContainer,
            documentTreeModel.CanExecute_AddBUIContainer);
        RelayCommand addBUIContainer;


        public ICommand AddFile => addFile ??= new RelayCommand(documentTreeModel.Execute_AddFile,
            documentTreeModel.CanExecute_AddFile);
        RelayCommand addFile;

        public ICommand Delete => delete ??= new RelayCommand(documentTreeModel.Execute_Delete,
            documentTreeModel.CanExecute_Delete);
        RelayCommand delete;

        public ICommand ClearSelection => clearSelection ??= new RelayCommand(documentTreeModel.Execute_ClearSelection,
            documentTreeModel.CanExecute_ClearSelection);
        RelayCommand clearSelection;

        public ICommand TreeViewLoaded => treeViewLoaded ??= new RelayCommand(
            documentTreeModel.Execute_TreeViewLoaded, documentTreeModel.CanExecute_TreeViewLoaded);
        private RelayCommand treeViewLoaded;
        public ICommand WindowLoaded => windowLoaded ??= new RelayCommand(
            documentTreeModel.Execute_WindowLoaded, documentTreeModel.CanExecuteWindowLoaded);
        private RelayCommand windowLoaded;

        public ICommand WindowClosing => windowClosing ??= new RelayCommand(
            documentTreeModel.Execute_WindowClosing, documentTreeModel.CanExecute_WindowClosing);
        private RelayCommand windowClosing;
    }
}
