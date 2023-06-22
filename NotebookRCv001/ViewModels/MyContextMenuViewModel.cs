using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Drawing = System.Drawing;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Models;

namespace NotebookRCv001.ViewModels
{
    public class MyContextMenuViewModel : ViewModelBase
    {
        private readonly MyContextMenuModel myContextMenuModel;
        public ObservableCollection<string> Headers => myContextMenuModel.Headers;
        public ObservableCollection<string> ToolTips => myContextMenuModel.ToolTips;

        public ObservableCollection<Drawing.Color> MyColors => myContextMenuModel.MyColors;



        public MyContextMenuViewModel()
        {
            myContextMenuModel = new();
            myContextMenuModel.PropertyChanged += (s, e) => OnPropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// вставка нового параграфа
        /// </summary>
        public ICommand NewParagraph => newParagraph ??= new RelayCommand(myContextMenuModel.Execute_NewParagraph,
            myContextMenuModel.CanExecute_NewParagraph);
        private RelayCommand newParagraph;

        /// <summary>
        /// операция Copy
        /// </summary>
        public ICommand OnCopyButtonClick => onCopyButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnCopyButtonClick,
            myContextMenuModel.CanExecute_OnCopyButtonClick);
        RelayCommand onCopyButtonClick;

        public ICommand OnCopyImageButtonClick => onCopyImageButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnCopyImageButtonClick,
            myContextMenuModel.CanExecute_OnCopyImageButtonClick);
        RelayCommand onCopyImageButtonClick;

        /// <summary>
        /// Операция "Cut"
        /// </summary>
        public ICommand OnCutButtonClick => onCutButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnCutButtonClick,
            myContextMenuModel.CanExecute_OnCutButtonClick);
        RelayCommand onCutButtonClick;

        public ICommand OnCutImageButtonClick => onCutImageButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnCutImageButtonClick,
            myContextMenuModel.CanExecute_OnCutImageButtonClick);
        RelayCommand onCutImageButtonClick;

        /// <summary>
        /// операция "вставить"
        /// </summary>
        public ICommand OnPasteButtonClick => onPasteButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnPasteButtonClick,
            myContextMenuModel.CanExecute_OnPasteButtonClick);
        RelayCommand onPasteButtonClick;
        public ICommand OnPasteImageButtonClick => onPasteImageButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnPasteImageButtonClick,
            myContextMenuModel.CanExecute_OnPasteImageButtonClick);
        RelayCommand onPasteImageButtonClick;

        public ICommand OpenForReading => openForReading ??= new RelayCommand(myContextMenuModel.Execute_OpenForReading,
            myContextMenuModel.CanExecute_OpenForReading);
        private RelayCommand openForReading;

        public ICommand OpenForEditing => openForEditing ??= new RelayCommand(myContextMenuModel.Execute_OpenForEditing,
            myContextMenuModel.CanExecute_OpenForEditing);
        private RelayCommand openForEditing;

        /// <summary>
        /// операция "Delete"
        /// </summary>
        public ICommand OnDeleteButtonClick => onDeleteButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnDeleteButtonClick,
            myContextMenuModel.CanExecute_OnDeleteButtonClick);
        RelayCommand onDeleteButtonClick;

        public ICommand OnDeleteImageButtonClick => onDeleteImageButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnDeleteImageButtonClick,
            myContextMenuModel.CanExecute_OnDeleteImageButtonClick);
        RelayCommand onDeleteImageButtonClick;

        public ICommand OnChangeImageButtonClick => onChangeImageButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnChangeImageButtonClick,
            myContextMenuModel.CanExecute_OnChangeImageButtonClick);
        RelayCommand onChangeImageButtonClick;

        /// <summary>
        /// контекстное меню - выбор цвет чернил
        /// </summary>
        public ICommand OnInkButtonClick => onInkButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnInkButtonClick,
            myContextMenuModel.CanExecute_OnInkButtonClick);
        RelayCommand onInkButtonClick;

        /// <summary>
        /// контекстное меню - выбор цвет фона шрифта
        /// </summary>
        public ICommand OnBackgroundButtonClick => onBackgroundButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnBackgroundButtonClick,
            myContextMenuModel.CanExecute_OnBackgroundButtonClick);
        RelayCommand onBackgroundButtonClick;

        /// <summary>
        /// контекстное меню - выбор цвет бумаги
        /// </summary>
        public ICommand OnPaperButtonClick => onPaperButtonClick ??= new RelayCommand
            (myContextMenuModel.Execute_OnPaperButtonClick,
            myContextMenuModel.CanExecute_OnPaperButtonClick);
        RelayCommand onPaperButtonClick;

        public ICommand MyContextMenuOpened => myContextMenuOpened ??= new RelayCommand
            (myContextMenuModel.Execute_MyContextMenuOpened, myContextMenuModel.CanExecute_MyContextMenuOpened);
        private RelayCommand myContextMenuOpened;
    }
}
