using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using NotebookRCv001.Models;
using NotebookRCv001.Interfaces;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Helpers;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Drawing = System.Drawing;

namespace NotebookRCv001.ViewModels
{
    public class RichTextBoxViewModel : ViewModelBase, IPageViewModel
    {

        private readonly RichTextBoxModel richTextBoxModel;

        public FlowDocument Document { get => richTextBoxModel.Document; set => richTextBoxModel.Document = value; }

        public BehaviorRichTextBox BehaviorRichTextBox => richTextBoxModel.BehaviorRichTextBox;
        public ObservableCollection<double> FontSizes => richTextBoxModel.FontSizes;
        public ObservableCollection<FontFamily> FontFamilies => richTextBoxModel.FontFamilies;

        public FlowDocument CloneDocument => richTextBoxModel.GetCloneDocument();

        public BitmapImage Bitmap => richTextBoxModel.Bitmap;

        /// <summary>
        /// коллекция заголовков для контекстного меню RichTextBox
        /// </summary>
        public ObservableCollection<string> Headers => richTextBoxModel.Headers;

        /// <summary>
        /// коллекция подсказок для контекстного меню RichTextBox
        /// </summary>
        public ObservableCollection<string> ToolTips => richTextBoxModel.ToolTips;

        /// <summary>
        /// коллекция подсказок для ToolBar RichTextBox
        /// </summary>
        public ObservableCollection<string> ToolTipsToolBar => richTextBoxModel.ToolTipsToolBar;

        public TextSelection Selection => richTextBoxModel.Selection;

        internal Visibility Visibility { get => richTextBoxModel.Visibility; set => richTextBoxModel.Visibility = value; }

        #region *****************Список и нумерованный список*******************


        /// <summary>
        /// активация кнопки ToggleNumbering
        /// </summary>
        public bool ButtonOnToggleNumberingIsChecked { get => richTextBoxModel.ButtonOnToggleNumberingIsChecked;
            set =>richTextBoxModel.ButtonOnToggleNumberingIsChecked = value; }

        /// <summary>
        /// активация кнопки ToggleBullets
        /// </summary>
        public bool ButtonOnToggleBulletsIsChecked { get => richTextBoxModel.ButtonOnToggleBulletsIsChecked;
            set => richTextBoxModel.ButtonOnToggleBulletsIsChecked = value; }


        #endregion


        #region __________вес, стиль и начертание шрифта__________


        public bool ButtonBoldIsChecked
        { get => richTextBoxModel.ButtonBoldIsChecked; set => richTextBoxModel.ButtonBoldIsChecked = value; }

        public bool ButtonItalicIsChecked
        { get => richTextBoxModel.ButtonItalicIsChecked; set => richTextBoxModel.ButtonItalicIsChecked = value; }

        public bool ButtonUnderlineIsChecked
        { get => richTextBoxModel.ButtonUnderlineIsChecked; set => richTextBoxModel.ButtonUnderlineIsChecked = value; }


        #endregion


        #region __________выравнивание текста__________

        /// <summary>
        /// выравнивание текста по левому краю
        /// </summary>
        public bool ButtonAlignLeftIsChecked
        {
            get => richTextBoxModel.ButtonAlignLeftIsChecked;
            set => richTextBoxModel.ButtonAlignLeftIsChecked = value;
        }

        /// <summary>
        /// выравнивание текста по центру
        /// </summary>
        public bool ButtonAlignCenterIsChecked
        {
            get => richTextBoxModel.ButtonAlignCenterIsChecked;
            set => richTextBoxModel.ButtonAlignCenterIsChecked = value;
        }

        /// <summary>
        /// выравнивание текста по правому краю
        /// </summary>
        public bool ButtonAlignRightIsChecked
        {
            get => richTextBoxModel.ButtonAlignRightIsChecked;
            set => richTextBoxModel.ButtonAlignRightIsChecked = value;
        }

        /// <summary>
        /// двустороннее выравнивание текста
        /// </summary>
        public bool ButtonAlignJustifyIsChecked
        {
            get => richTextBoxModel.ButtonAlignJustifyIsChecked;
            set => richTextBoxModel.ButtonAlignJustifyIsChecked = value;
        }


        #endregion


        #region _____________MyColors___________________


        /// <summary>
        /// коллекция доступных системных цветов
        /// </summary>
        public ObservableCollection<Drawing.Color> MyColors { get =>richTextBoxModel.MyColors;
            set => richTextBoxModel.MyColors = value; }

        /// <summary>
        /// коллекция цветов для быстрого выбора
        /// </summary>
        public ObservableCollection<Drawing.Color> QuickSelectColorCollection { get => richTextBoxModel.QuickSelectColorCollection;
            set => richTextBoxModel.QuickSelectColorCollection = value; }

        public Brush MyForeground { get => richTextBoxModel.MyForeground; set => richTextBoxModel.MyForeground = value; }
        public Brush MyFontBackground { get => richTextBoxModel.MyFontBackground; set => richTextBoxModel.MyFontBackground = value; }
        public Brush MyBackground { set => richTextBoxModel.MyBackground = value; get => richTextBoxModel.MyBackground; }

        #endregion

        public ObservableCollection<string> Icons => throw new NotImplementedException();

        public ObservableCollection<string> Images => throw new NotImplementedException();

        public Action BehaviorReady { get => richTextBoxModel.BehaviorReady; set => richTextBoxModel.BehaviorReady = value; }

        public RichTextBoxViewModel ( )
        {
            richTextBoxModel = new RichTextBoxModel ( );
            richTextBoxModel.PropertyChanged += ( s, e ) => OnPropertyChanged ( e.PropertyName );
        }



        /// <summary>
        /// загрузка ComboBox FontFamily
        /// </summary>
        public ICommand FontFamilyComboLoaded => fontFamilyComboLoaded ??= new RelayCommand
            ( richTextBoxModel.Execute_FontFamilyComboLoaded,
            richTextBoxModel.CanExecute_FontFamilyComboLoaded );
        RelayCommand fontFamilyComboLoaded;
        /// <summary>
        /// загрузка ComboBox FontSize
        /// </summary>
        public ICommand FontSizeComboLoaded => fontSizeComboLoaded ??= new RelayCommand
            ( richTextBoxModel.Execute_FontSizeComboLoaded,
            richTextBoxModel.CanExecute_FontSizeComboLoaded );
        RelayCommand fontSizeComboLoaded;
        /// <summary>
        /// выбор размера шрифта FontSize в FontSizeCombo
        /// </summary>
        public ICommand OnFontSizeSelectionChanged => onFontSizeSelectionChanged ??= new RelayCommand
            ( richTextBoxModel.Execute_OnFontSizeSelectionChanged,
            richTextBoxModel.CanExecute_OnFontSizeSelectionChanged );
        RelayCommand onFontSizeSelectionChanged;

        /// <summary>
        /// выбор семейства шрифтов FontFamily в FontFamilyCombo
        /// </summary>
        public ICommand OnFontFamilySelectionChanged => onFontFamilySelectionChanged ??= new RelayCommand
            ( richTextBoxModel.Execute_OnFontFamilySelectionChanged,
            richTextBoxModel.CanExecute_OnFontFamilySelectionChanged );
        RelayCommand onFontFamilySelectionChanged;

        /// <summary>
        /// восстановление прозрачного фона шрифта
        /// </summary>
        public ICommand OnTransparentFontBackground => onTransparentFontBackground ??= new RelayCommand ( richTextBoxModel.Execute_OnTransparentFontBackground,
            richTextBoxModel.CanExecute_OnTransparentFontBackground );
        RelayCommand onTransparentFontBackground;

        /// <summary>
        /// загрузка RichTextBox
        /// </summary>
        public ICommand RichTextBoxLoaded => richTextBoxLoaded ??= new RelayCommand
            ( richTextBoxModel.Execute_RichTextBoxLoaded,
            richTextBoxModel.CanExecute_RichTextBoxLoaded );
        RelayCommand richTextBoxLoaded;
        /// <summary>
        /// Операция "вернуть"
        /// </summary>
        public ICommand OnUndoButtonClick => onUndoButtonClick ??= new RelayCommand
            ( richTextBoxModel.Execute_OnUndoButtonClick,
            richTextBoxModel.CanExecute_OnUndoButtonClick );
        RelayCommand onUndoButtonClick;
        /// <summary>
        /// Операция "Copy"
        /// </summary>
        public ICommand OnCopyButtonClick => onCopyButtonClick ??= new RelayCommand
            ( richTextBoxModel.Execute_OnCopyButtonClick,
            richTextBoxModel.CanExecute_OnCopyButtonClick );
        RelayCommand onCopyButtonClick;

        /// <summary>
        /// Операция "Cut"
        /// </summary>
        public ICommand OnCutButtonClick => onCutButtonClick ??= new RelayCommand
            ( richTextBoxModel.Execute_OnCutButtonClick,
            richTextBoxModel.CanExecute_OnCutButtonClick );
        RelayCommand onCutButtonClick;

        /// <summary>
        /// операция "вставить"
        /// </summary>
        public ICommand OnPasteButtonClick => onPasteButtonClick ??= new RelayCommand
            ( richTextBoxModel.Execute_OnPasteButtonClick,
            richTextBoxModel.CanExecute_OnPasteButtonClick );
        RelayCommand onPasteButtonClick;

        /// <summary>
        /// операция "Delete"
        /// </summary>
        public ICommand OnDeleteButtonClick => onDeleteButtonClick ??= new RelayCommand
            ( richTextBoxModel.Execute_OnDeleteButtonClick,
            richTextBoxModel.CanExecute_OnDeleteButtonClick );
        RelayCommand onDeleteButtonClick;

        /// <summary>
        /// контекстное меню - выбор цвет чернил
        /// </summary>
        public ICommand OnInkButtonClick => onInkButtonClick ??= new RelayCommand ( richTextBoxModel.Execute_OnInkButtonClick,
            richTextBoxModel.CanExecute_OnInkButtonClick );
        RelayCommand onInkButtonClick;

        /// <summary>
        /// контекстное меню - выбор цвет фона шрифта
        /// </summary>
        public ICommand OnBackgroundButtonClick => onBackgroundButtonClick ??= new RelayCommand ( richTextBoxModel.Execute_OnBackgroundButtonClick,
            richTextBoxModel.CanExecute_OnBackgroundButtonClick );
        RelayCommand onBackgroundButtonClick;

        /// <summary>
        /// контекстное меню - выбор цвет бумаги
        /// </summary>
        public ICommand OnPaperButtonClick => onPaperButtonClick ??= new RelayCommand ( richTextBoxModel.Execute_OnPaperButtonClick,
            richTextBoxModel.CanExecute_OnPaperButtonClick );
        RelayCommand onPaperButtonClick;

        /// <summary>
        /// операция Bold
        /// </summary>
        public ICommand OnBoldButtonClick => onBoldButtonClick ??= new RelayCommand
            ( richTextBoxModel.Execute_OnBoldButtonClick,
            richTextBoxModel.CanExecute_OnBoldButtonClick );
        RelayCommand onBoldButtonClick;
        /// <summary>
        /// операция Italic
        /// </summary>
        public ICommand OnItalicButtonClick => onItalicButtonClick ??= new RelayCommand
            ( richTextBoxModel.Execute_OnItalicButtonClick,
            richTextBoxModel.CanExecute_OnItalicButtonClick );
        RelayCommand onItalicButtonClick;
        /// <summary>
        /// операция "underline"
        /// </summary>
        public ICommand OnUnderlineButtonClick => onUnderlineButtonClick ??= new RelayCommand
            ( richTextBoxModel.Execute_OnUnderlineButtonClick,
            richTextBoxModel.CanExecute_OnUnderlineButtonClick );
        RelayCommand onUnderlineButtonClick;
        /// <summary>
        /// операция "normal text"
        /// </summary>
        public ICommand ButtonOnNormalTextClick => buttonOnNormalTextClick ??= new RelayCommand
            ( richTextBoxModel.Execute_ButtonOnNormalTextClick,
            richTextBoxModel.CanExecute_ButtonOnNormalTextClick );
        RelayCommand buttonOnNormalTextClick;
        /// <summary>
        /// операция уменьшения межстрочного интервала
        /// </summary>
        public ICommand ButtonOnDecreaseClick => buttonOnDecreaseClick ??= new RelayCommand
            ( richTextBoxModel.Execute_ButtonOnDecreaseClick,
            richTextBoxModel.CanExecute_ButtonOnDecreaseClick );
        RelayCommand buttonOnDecreaseClick;
        /// <summary>
        /// операция увеличения межстрочного интервала
        /// </summary>
        public ICommand ButtonOnIncreaseClick => buttonOnIncreaseClick ??= new RelayCommand
            ( richTextBoxModel.Execute_ButtonOnIncreaseClick,
            richTextBoxModel.CanExecute_ButtonOnIncreaseClick );
        RelayCommand buttonOnIncreaseClick;
        /// <summary>
        /// операция "alignleft"
        /// </summary>
        public ICommand ButtonOnAlignLeftButtonClick => buttonOnAlignLeftButtonClick ??= new RelayCommand ( richTextBoxModel.Execute_ButtonOnAlignLeftButtonClick,
            richTextBoxModel.CanExecute_ButtonOnAlignLeftButtonClick );
        RelayCommand buttonOnAlignLeftButtonClick;
        /// <summary>
        /// операция "aligncenter"
        /// </summary>
        public ICommand ButtonOnAlignCenterButtonClick => buttonOnAlignCenterButtonClick ??= new RelayCommand ( richTextBoxModel.Execute_ButtonOnAlignCenterButtonClick,
            richTextBoxModel.CanExecute_ButtonOnAlignCenterButtonClick );
        RelayCommand buttonOnAlignCenterButtonClick;
        /// <summary>
        /// операция "aligncright"
        /// </summary>
        public ICommand ButtonOnAlignRightButtonClick => buttonOnAlignRightButtonClick ??= new RelayCommand ( richTextBoxModel.Execute_ButtonOnAlignRightButtonClick,
            richTextBoxModel.CanExecute_ButtonOnAlignRightButtonClick );
        RelayCommand buttonOnAlignRightButtonClick;
        /// <summary>
        /// операция "alignjustify"
        /// </summary>
        public ICommand ButtonOnAlignJustifyButtonClick => buttonOnAlignJustifyButtonClick ??= new RelayCommand ( richTextBoxModel.Execute_ButtonOnAlignJustifyButtonClick,
            richTextBoxModel.CanExecute_ButtonOnAlignJustifyButtonClick );
        RelayCommand buttonOnAlignJustifyButtonClick;

        /// <summary>
        /// операция "ToggleBullets"
        /// </summary>
        public ICommand ButtonOnToggleBullets => buttonOnToggleBullets ??= new RelayCommand(richTextBoxModel.Execute_ButtonOnToggleBullets,
            richTextBoxModel.CanExecute_ButtonOnToggleBullets);
        RelayCommand buttonOnToggleBullets;

        /// <summary>
        /// операция "ToggleNumbering"
        /// </summary>
        public ICommand ButtonOnToggleNumbering => buttonOnToggleNumbering ??= new RelayCommand(richTextBoxModel.Execute_ButtonOnToggleNumbering,
            richTextBoxModel.CanExecute_ButtonOnToggleNumbering);
        RelayCommand buttonOnToggleNumbering;

        /// <summary>
        /// быстрый выбор цвета
        /// </summary>
        public ICommand QuickColorSelection => quickColorSelection ??= new RelayCommand(richTextBoxModel.Execute_QuickColorSelection,
            richTextBoxModel.CanExecute_QuickColorSelection);
        RelayCommand quickColorSelection;

        public ICommand SelectAColorForTheQuickSelectToolbar => selectAColorForTheQuickSelectToolbar ??= new RelayCommand(
            richTextBoxModel.Execute_SelectAColorForTheQuickSelectToolbar, richTextBoxModel.CanExecute_SelectAColorForTheQuickSelectToolbar);
        RelayCommand selectAColorForTheQuickSelectToolbar;

        public ICommand ToSetAColorInTheQuickSelectToolbar => toSetAColorInTheQuickSelectToolbar ??= new RelayCommand(
            richTextBoxModel.Execute_ToSetAColorInTheQuickSelectToolbar, richTextBoxModel.CanExecute_ToSetAColorInTheQuickSelectToolbar);
        RelayCommand toSetAColorInTheQuickSelectToolbar;

        public ICommand PageLoaded => pageLoaded ??= new RelayCommand
            ( richTextBoxModel.Execute_PageLoaded, richTextBoxModel.CanExecute_PageLoaded );
        RelayCommand pageLoaded;

        public ICommand PageUnloaded => pageUnloaded ??= new RelayCommand(richTextBoxModel.Execute_PageUnloaded, richTextBoxModel.CanExecute_PageUnloaded);
        RelayCommand pageUnloaded;
        public ICommand PageClear => pageClear ??= new RelayCommand ( richTextBoxModel.Execute_PageClear,
            richTextBoxModel.CanExecute_PageClear );
        RelayCommand pageClear;

        /// <summary>
        /// закрытие страницы
        /// </summary>
        public ICommand PageClose => pageClose ??= new RelayCommand
            ( richTextBoxModel.Execute_PageClose, richTextBoxModel.CanExecute_PageClose );
        RelayCommand pageClose;

    }
}
