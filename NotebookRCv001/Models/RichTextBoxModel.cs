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
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.IO;
using Drawing = System.Drawing;
using System.Reflection;
using System.Windows.Media.Imaging;
using NotebookRCv001.ViewModels;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace NotebookRCv001.Models
{
    public class RichTextBoxModel : ViewModelBase
    {

        ViewModels.MainWindowViewModel MainWindowViewModel { get; set; }
        private BehaviorComboBox FontFamilyComboBehavior { get; set; }
        private BehaviorComboBox FontSizeComboBehavior { get; set; }
        internal BehaviorRichTextBox BehaviorRichTextBox { get; set; }

        internal FlowDocument Document
        {
            get => BehaviorRichTextBox?.Document;
            set
            {
                if (BehaviorRichTextBox != null)
                    BehaviorRichTextBox.Document = value;
            }
        }

        internal ObservableCollection<double> FontSizes
        {
            set => SetProperty(ref fontSizes, value);
            get => fontSizes;
        }
        ObservableCollection<double> fontSizes;

        internal ObservableCollection<FontFamily> FontFamilies => fontFmilies ??= new ObservableCollection<FontFamily>();
        ObservableCollection<FontFamily> fontFmilies;

        internal BitmapImage Bitmap => BehaviorRichTextBox.Bitmap;

        internal ObservableCollection<string> Headers => MainWindowViewModel.Language.RichTextBox;
        internal ObservableCollection<string> ToolTips => MainWindowViewModel.Language.ToolTipsRichTextBox;

        /// <summary>
        /// коллекция подсказок для ToolBar RichTextBox
        /// </summary>
        internal ObservableCollection<string> ToolTipsToolBar => MainWindowViewModel.Language.ToolTipsRichTextBoxToolBar;

        internal TextSelection Selection => BehaviorRichTextBox.Selection;

        internal Visibility Visibility { get => BehaviorRichTextBox.Visibility; set => BehaviorRichTextBox.Visibility = value; }

        /// <summary>
        /// Номер ячейки коллекции QuickSelectColorCollection, в которой необходимо заменить цвет
        /// </summary>
        private int changeColorCellNumber { get; set; }


        #region *****************Список и нумерованный список*******************


        /// <summary>
        /// активация кнопки ToggleNumbering
        /// </summary>
        internal bool ButtonOnToggleNumberingIsChecked
        {
            get => buttonOnToggleNumberingIsChecked;
            set => SetProperty(ref buttonOnToggleNumberingIsChecked, value);
        }
        private bool buttonOnToggleNumberingIsChecked;

        /// <summary>
        /// активация кнопки ToggleBullets
        /// </summary>
        internal bool ButtonOnToggleBulletsIsChecked
        {
            get => buttonOnToggleBulletsIsChecked;
            set => SetProperty(ref buttonOnToggleBulletsIsChecked, value);
        }
        private bool buttonOnToggleBulletsIsChecked;


        #endregion


        #region ___________________Colors______________________________

        /// <summary>
        /// коллекция доступных системных цветов
        /// </summary>
        internal ObservableCollection<Drawing.Color> MyColors
        {
            get => myColors ??= new ObservableCollection<Drawing.Color>();
            set => SetProperty(ref myColors, value);
        }
        private ObservableCollection<Drawing.Color> myColors;

        /// <summary>
        /// коллекция цветов для быстрого выбора
        /// </summary>
        internal ObservableCollection<Drawing.Color> QuickSelectColorCollection
        {
            get => quickSelectColorCollection;
            set => SetProperty(ref quickSelectColorCollection, value);
        }
        private ObservableCollection<Drawing.Color> quickSelectColorCollection;

        internal Brush MyForeground { get => BehaviorRichTextBox?.MyForeground; set => BehaviorRichTextBox.MyForeground = value; }
        internal Brush MyFontBackground { get => BehaviorRichTextBox?.MyFontBackground; set => BehaviorRichTextBox.MyFontBackground = value; }
        internal Brush MyBackground { set => BehaviorRichTextBox.MyBackground = value; get => BehaviorRichTextBox?.MyBackground; }

        #endregion


        #region __________вес, стиль и начертание шрифта__________

        /// <summary>
        /// вес шрифта Bold
        /// </summary>
        internal bool ButtonBoldIsChecked
        {
            get => buttonBoldIsChecked;
            set => SetProperty(ref buttonBoldIsChecked, value);
        }
        bool buttonBoldIsChecked;

        /// <summary>
        /// стиль шрифта Italic
        /// </summary>
        internal bool ButtonItalicIsChecked
        {
            get => buttonItalicIsChecked;
            set => SetProperty(ref buttonItalicIsChecked, value);
        }
        bool buttonItalicIsChecked;

        /// <summary>
        /// начертание шрифта Under line
        /// </summary>
        internal bool ButtonUnderlineIsChecked
        {
            get => buttonUnderlineIsChecked;
            set => SetProperty(ref buttonUnderlineIsChecked, value);
        }
        bool buttonUnderlineIsChecked;


        #endregion


        #region __________выравнивание текста__________

        /// <summary>
        /// выравнивание текста по левому краю
        /// </summary>
        internal bool ButtonAlignLeftIsChecked
        {
            get => buttonAlignLeftIsChecked;
            set => SetProperty(ref buttonAlignLeftIsChecked, value);
        }
        bool buttonAlignLeftIsChecked;

        /// <summary>
        /// выравнивание текста по центру
        /// </summary>
        internal bool ButtonAlignCenterIsChecked
        {
            get => buttonAlignCenterIsChecked;
            set => SetProperty(ref buttonAlignCenterIsChecked, value);
        }
        bool buttonAlignCenterIsChecked;

        /// <summary>
        /// выравнивание текста по правому краю
        /// </summary>
        internal bool ButtonAlignRightIsChecked
        {
            get => buttonAlignRightIsChecked;
            set => SetProperty(ref buttonAlignRightIsChecked, value);
        }
        bool buttonAlignRightIsChecked;

        /// <summary>
        /// двустороннее выравнивание текста
        /// </summary>
        internal bool ButtonAlignJustifyIsChecked
        {
            get => buttonAlignJustifyIsChecked;
            set => SetProperty(ref buttonAlignJustifyIsChecked, value);
        }
        bool buttonAlignJustifyIsChecked;



        #endregion


        /// <summary>
        /// делегат выполняемый после определения BehaviorFlowDocumentReader
        /// </summary>
        internal Action<object> BehaviorReady { get => behaviorReady; set => SetProperty(ref behaviorReady, value); }
        private Action<object> behaviorReady;

        public RichTextBoxModel()
        {
            //Properties.Settings.Default.SetColorsForQuickAccess = null;
            MainWindowViewModel = (ViewModels.MainWindowViewModel)Application.Current.MainWindow.DataContext;
            MainWindowViewModel.Language.PropertyChanged += (s, e) => OnPropertyChanged(new string[]
            { "Headers", "ToolTips", "ToolTipsToolBar" });
            FontSizes = new ObservableCollection<double>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
            //получаем все доступные цвета взяв и отсортировав все свойства типа Drawing.Color
            foreach (PropertyInfo info in typeof(Drawing.Color).GetProperties())
            {
                if (info.PropertyType == typeof(Drawing.Color))
                    MyColors.Add((System.Drawing.Color)info.GetValue(null));
            }
            QuickSelectColorCollection = new();
            if (Properties.Settings.Default.SetColorsForQuickAccess == null || Properties.Settings.Default.SetColorsForQuickAccess.Count < 5)
            {//если нет ранее сохраненных цветов быстрого доступа или их колличество меньше нужного. Устанавливаем случайные цвета
                QuickSelectColorCollection = new() { MyColors[5], MyColors[10], MyColors[15], MyColors[20], MyColors[25], };
                Properties.Settings.Default.SetColorsForQuickAccess = new();
            }
            else
            {//если ранее сохраненные цвета быстрого доступа найдены, загружаем их
                foreach (var color in Properties.Settings.Default.SetColorsForQuickAccess)
                {
                    if (color != null)
                    {
                        var newcolor = MyColors.Where((x) => x.Name == color).FirstOrDefault();
                        QuickSelectColorCollection.Add(newcolor);
                    }
                }
            }
            OnPropertyChanged("QuickSelectColorCollection");
        }




        #region **************************** Обработка команд****************************************

        /// <summary>
        /// FontFamilyCombo Loaded
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_FontFamilyComboLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_FontFamilyComboLoaded(object obj)
        {
            try
            {
                if (obj is BehaviorComboBox behavior)
                {
                    FontFamilyComboBehavior = behavior;
                    foreach (var family in Fonts.SystemFontFamilies)
                    {
                        if (family is FontFamily fontFamily)
                            FontFamilies.Add(fontFamily);
                    }
                    //FontFamilyComboBehavior.ItemsSource = Fonts.SystemFontFamilies;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// FontSizeCombo Loaded
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_FontSizeComboLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_FontSizeComboLoaded(object obj)
        {
            try
            {
                if (obj is BehaviorComboBox behavior)
                {
                    FontSizeComboBehavior = behavior;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnFontSizeSelectionChanged(object obj)
        {
            try
            {
                bool c = false;
                if (obj is double size && FontSizes.Any((x) => x == size))
                    c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnFontSizeSelectionChanged(object obj)
        {
            try
            {
                if (obj is double fontsize)
                {
                    double currentFontsize = -1;
                    if (!BehaviorRichTextBox.Selection.IsEmpty)
                        currentFontsize = BehaviorRichTextBox.FontSize;
                    BehaviorRichTextBox.FontSize = fontsize;
                    if (FontSizes.Any((x) => x == currentFontsize))
                        FontSizeComboBehavior.SelectedItem = currentFontsize;
                    BehaviorRichTextBox.RichTextBox.Focus();
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnFontFamilySelectionChanged(object obj)
        {
            try
            {
                bool c = false;
                if (obj is FontFamily family)
                {
                    c = family != BehaviorRichTextBox?.FontFamily;
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnFontFamilySelectionChanged(object obj)
        {
            try
            {
                FontFamily fontFamily = null;
                if (!BehaviorRichTextBox.Selection.IsEmpty)
                    fontFamily = BehaviorRichTextBox.FontFamily;
                BehaviorRichTextBox.FontFamily = (FontFamily)obj;
                if (fontFamily != null)
                    FontFamilyComboBehavior.SelectedItem = fontFamily;
                BehaviorRichTextBox.RichTextBox.Focus();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// восстановление прозрачного фона шрифта
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_OnTransparentFontBackground(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnTransparentFontBackground(object obj)
        {
            try
            {
                MyFontBackground = Brushes.Transparent;
            }
            catch (Exception e) { ErrorWindow(e); }
        }



        /// <summary>
        /// RichTextBox Loaded
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_RichTextBoxLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = obj is BehaviorRichTextBox;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_RichTextBoxLoaded(object obj)
        {
            try
            {
                if (obj is BehaviorRichTextBox textBox)
                {
                    BehaviorRichTextBox = textBox;
                    BehaviorRichTextBox.Visibility = Visibility.Visible;
                    FontSizeComboBehavior.SelectedItem = textBox.FontSize;
                    FontFamilyComboBehavior.SelectedItem = textBox.FontFamily;
                    ButtonBoldIsChecked = textBox.FontWeight == FontWeights.Bold;
                    ButtonItalicIsChecked = textBox.FontStyle == FontStyles.Italic;
                    ButtonUnderlineIsChecked = textBox.TextDecoration == TextDecorations.Underline;
                    ButtonOnToggleBulletsIsChecked = false;
                    ButtonOnToggleNumberingIsChecked = false;
                    OnPropertyChanged(new string[] { "MyForeground", "MyBackground", "MyFontBackground" });
                    textBox.SetFocus();
                    if (BehaviorReady != null)
                    {
                        BehaviorReady.Invoke(obj);
                        BehaviorReady = null;
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// операция Copy
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_OnCopyButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = ApplicationCommands.Copy.CanExecute(null, BehaviorRichTextBox?.RichTextBox);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnCopyButtonClick(object obj)
        {
            try
            {
                ApplicationCommands.Copy.Execute(null, BehaviorRichTextBox.RichTextBox);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// операция Cut
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_OnCutButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = ApplicationCommands.Cut.CanExecute(null, BehaviorRichTextBox?.RichTextBox);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnCutButtonClick(object obj)
        {
            try
            {
                ApplicationCommands.Cut.Execute(null, BehaviorRichTextBox.RichTextBox);
            }
            catch (Exception e) { ErrorWindow(e); }
        }



        /// <summary>
        /// Button Undo
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_OnUndoButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (BehaviorRichTextBox != null)
                {
                    c = ApplicationCommands.Undo.CanExecute(null, BehaviorRichTextBox.RichTextBox);
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnUndoButtonClick(object obj)
        {
            try
            {
                BehaviorRichTextBox.ServiceKey = Key.Return;
                ApplicationCommands.Undo.Execute(null, BehaviorRichTextBox.RichTextBox);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// Button Paste
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_OnPasteButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (BehaviorRichTextBox != null && Clipboard.GetImage() == null)
                {
                    c = ApplicationCommands.Paste.CanExecute(null, BehaviorRichTextBox.RichTextBox);
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnPasteButtonClick(object obj)
        {
            try
            {
                var imagesource = Clipboard.GetImage();
                if (imagesource == null)
                {
                    BehaviorRichTextBox.ServiceKey = Key.Insert;
                    ApplicationCommands.Paste.Execute(null, BehaviorRichTextBox.RichTextBox);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// Button Delete
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_OnDeleteButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = !BehaviorRichTextBox.Selection.IsEmpty;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnDeleteButtonClick(object obj)
        {
            try
            {
                BehaviorRichTextBox.OnDeleteText();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// контекстное меню - смена цвета чернил
        /// </summary>
        /// <param name="obj">выбранный цвет Brush</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_OnInkButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnInkButtonClick(object obj)
        {
            try
            {
                if (obj is Brush brush)
                {
                    MyForeground = brush;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// контекстное меню - смена цвета фона шрифта
        /// </summary>
        /// <param name="obj">выбранный цвет Brush</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_OnBackgroundButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnBackgroundButtonClick(object obj)
        {
            try
            {
                if (obj is Brush brush)
                {
                    MyFontBackground = brush;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// контекстное меню - смена цвета бумаги
        /// </summary>
        /// <param name="obj">выбранный цвет Brush</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_OnPaperButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnPaperButtonClick(object obj)
        {
            try
            {
                if (obj is Brush brush)
                {
                    MyBackground = brush;
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// создание нового параграфа
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ButtonOnNewParagraph(object obj)
        {
            return BehaviorRichTextBox != null;
        }
        internal void Execute_ButtonOnNewParagraph(object obj)
        {
            try
            {
                BehaviorRichTextBox.NewParagraph();
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// TogleButton Bold
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_OnBoldButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnBoldButtonClick(object obj)
        {
            try
            {
                if (ButtonBoldIsChecked)
                    BehaviorRichTextBox.FontWeight = FontWeights.Bold;
                else
                    BehaviorRichTextBox.FontWeight = FontWeights.Normal;
                BehaviorRichTextBox.RichTextBox.Focus();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// TogleButton Italic
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_OnItalicButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnItalicButtonClick(object obj)
        {
            try
            {
                if (ButtonItalicIsChecked)
                    BehaviorRichTextBox.FontStyle = FontStyles.Italic;
                else
                    BehaviorRichTextBox.FontStyle = FontStyles.Normal;
                BehaviorRichTextBox.RichTextBox.Focus();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// TogleButton UnderLine
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_OnUnderlineButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnUnderlineButtonClick(object obj)
        {
            try
            {
                if (ButtonUnderlineIsChecked)
                    BehaviorRichTextBox.TextDecoration = TextDecorations.Underline;
                else
                    BehaviorRichTextBox.TextDecoration = null;
                BehaviorRichTextBox.RichTextBox.Focus();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// Button Normal Text
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ButtonOnNormalTextClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null ? !BehaviorRichTextBox.Selection.IsEmpty : false;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnNormalTextClick(object obj)
        {
            try
            {
                BehaviorRichTextBox.OnNormalText();
                BehaviorRichTextBox.RichTextBox.Focus();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// уменьшение межстрочного интервала
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ButtonOnDecreaseClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null ? !BehaviorRichTextBox.Selection.IsEmpty : false;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnDecreaseClick(object obj)
        {
            try
            {
                BehaviorRichTextBox.OnLineSpacingBottomDecrease();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// Увеличение межстрочного интервала
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ButtonOnIncreaseClick(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null ? !BehaviorRichTextBox.Selection.IsEmpty : false;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnIncreaseClick(object obj)
        {
            try
            {
                BehaviorRichTextBox.OnLineSpacingBottomIncrease();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// TogleButton AlignLeft
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ButtonOnAlignLeftButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnAlignLeftButtonClick(object obj)
        {
            try
            {
                if (ButtonAlignLeftIsChecked)
                {
                    ButtonAlignCenterIsChecked = false;
                    ButtonAlignRightIsChecked = false;
                    ButtonAlignJustifyIsChecked = false;
                    EditingCommands.AlignLeft.Execute(null, BehaviorRichTextBox.RichTextBox);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// TogleButton AlignCenter
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ButtonOnAlignCenterButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnAlignCenterButtonClick(object obj)
        {
            try
            {
                if (ButtonAlignCenterIsChecked)
                {
                    ButtonAlignLeftIsChecked = false;
                    ButtonAlignRightIsChecked = false;
                    ButtonAlignJustifyIsChecked = false;
                    EditingCommands.AlignCenter.Execute(null, BehaviorRichTextBox.RichTextBox);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// TogleButton AlignRight
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ButtonOnAlignRightButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnAlignRightButtonClick(object obj)
        {
            try
            {
                if (ButtonAlignRightIsChecked)
                {
                    ButtonAlignCenterIsChecked = false;
                    ButtonAlignLeftIsChecked = false;
                    ButtonAlignJustifyIsChecked = false;
                    EditingCommands.AlignRight.Execute(null, BehaviorRichTextBox.RichTextBox);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// TogleButton AlignJustify
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ButtonOnAlignJustifyButtonClick(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnAlignJustifyButtonClick(object obj)
        {
            try
            {
                if (ButtonAlignJustifyIsChecked)
                {
                    ButtonAlignCenterIsChecked = false;
                    ButtonAlignRightIsChecked = false;
                    ButtonAlignLeftIsChecked = false;
                    EditingCommands.AlignJustify.Execute(null, BehaviorRichTextBox.RichTextBox);
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// обработка команды список ButtonOnToggleBullets
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ButtonOnToggleBullets(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null ? !BehaviorRichTextBox.Selection.IsEmpty : false;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnToggleBullets(object obj)
        {
            try
            {
                BehaviorRichTextBox.OnToggleBullets();
                ButtonOnToggleBulletsIsChecked = false;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// обработка команды нумерованный список ButtonOnToggleNumbering
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ButtonOnToggleNumbering(object obj)
        {
            try
            {
                bool c = false;
                c = BehaviorRichTextBox != null ? !BehaviorRichTextBox.Selection.IsEmpty : false;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ButtonOnToggleNumbering(object obj)
        {
            try
            {
                BehaviorRichTextBox.OnToggleNumbering();
                ButtonOnToggleNumberingIsChecked = false;
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// реализация быстрого выбора цвета
        /// </summary>
        /// <param name="obj">выбранный цвет(Brush)</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_QuickColorSelection(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_QuickColorSelection(object obj)
        {
            try
            {
                if (CanExecute_OnInkButtonClick(obj))
                    Execute_OnInkButtonClick(obj);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// открытие контекстного меню для установки цвета в панель быстрого выбора цвета
        /// </summary>
        /// <param name="obj">номер ячейки в коллекции QuickSelectColorCollection ( Int )</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_SelectAColorForTheQuickSelectToolbar(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_SelectAColorForTheQuickSelectToolbar(object obj)
        {
            try
            {
                if (obj is string str)
                {
                    changeColorCellNumber = int.Parse(str);
                }

            }
            catch (Exception e) { ErrorWindow(e); }

        }
        /// <summary>
        /// установка выбранного цвета в заданную ранее ячейку коллекции QuickSelectColorCollection
        /// </summary>
        /// <param name="obj">Выбранный цвет ( Brush )</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        internal bool CanExecute_ToSetAColorInTheQuickSelectToolbar(object obj)
        {
            try
            {
                bool c = false;
                c = obj != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ToSetAColorInTheQuickSelectToolbar(object obj)
        {
            try
            {
                if (obj is Brush brush)
                {
                    var convert = (Converters.ColorToColorConverter)Application.Current.MainWindow.FindResource("colorconvert");
                    if (convert != null)
                    {
                        QuickSelectColorCollection[changeColorCellNumber] =
                            (System.Drawing.Color)convert.Convert(brush, typeof(System.Drawing.Color), null, null);
                        OnPropertyChanged("QuickSelectColorCollection");
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        #endregion

        /// <summary>
        /// получение кода документа 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updatexaml_Click(object sender, RoutedEventArgs e)
        {
            TextRange range;

            range = new TextRange(Document.ContentStart, Document.ContentEnd);

            MemoryStream stream = new MemoryStream();
            range.Save(stream, DataFormats.Xaml);
            stream.Position = 0;

            StreamReader r = new StreamReader(stream);

            TextBlock textBlock = new TextBlock();
            textBlock.Text = r.ReadToEnd();
            r.Close();
            stream.Close();
        }

        internal FlowDocument GetCloneDocument()
        {
            try
            {
                return BehaviorRichTextBox.CloneDocument();
            }
            catch (Exception e) { ErrorWindow(e); return new FlowDocument(); }
        }

        internal bool CanExecute_PageLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageLoaded(object obj)
        {
            try
            {
                //QuickSelectColorCollection = new();
                //if (Properties.Settings.Default.SetColorsForQuickAccess == null)
                //{
                //    Properties.Settings.Default.SetColorsForQuickAccess = new()
                //    { MyColors[5].Name, MyColors[10].Name, MyColors[15].Name, MyColors[20].Name, MyColors[25].Name, };
                //}
                //foreach (var color in Properties.Settings.Default.SetColorsForQuickAccess)
                //{
                //    if (color != null)
                //    {
                //        var newcolor = MyColors.Where((x) => x.Name == color).FirstOrDefault();
                //        QuickSelectColorCollection.Add(newcolor);
                //    }
                //    OnPropertyChanged("QuickSelectColorCollection");
                //}
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_PageUnloaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageUnloaded(object obj)
        {
            try
            {
                //Properties.Settings.Default.SetColorsForQuickAccess = new();
                //foreach (var color in QuickSelectColorCollection)
                //{
                //    var newcolor = MyColors.Where((x) => x.A == color.A && x.R == color.R && x.G == color.G && x.B == color.B).FirstOrDefault();
                //    Properties.Settings.Default.SetColorsForQuickAccess.Add(newcolor.Name);
                //}
                //Properties.Settings.Default.Save();
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_PageClear(object obj)
        {
            try
            {
                bool c = false;
                if (BehaviorRichTextBox != null)
                    c = !BehaviorRichTextBox.TextRange.IsEmpty;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClear(object obj)
        {
            try
            {
                BehaviorRichTextBox.Document.Blocks.Clear();
                BehaviorRichTextBox.CurrentFigure = null;
                var home = MainWindowViewModel.CurrentPage;
                var viewmodel = (HomeViewModel)home.DataContext;
                viewmodel.PathToLastFile = "";
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_PageClose(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_PageClose(object obj)
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow(e); }
        }

    }
}
