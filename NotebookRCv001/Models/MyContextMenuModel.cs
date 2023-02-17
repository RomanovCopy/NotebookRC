using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Drawing = System.Drawing;

using NotebookRCv001.Helpers;
using NotebookRCv001.Infrastructure;
using NotebookRCv001.Interfaces;
using NotebookRCv001.ViewModels;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Documents;
using System.IO;
using NotebookRCv001.Styles.CustomizedWindow;

namespace NotebookRCv001.Models
{
    internal class MyContextMenuModel : ViewModelBase
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        private Languages language => mainWindowViewModel.Language;
        private RichTextBoxViewModel richTextBoxViewModel { get; set; }
        private BehaviorRichTextBox behaviorRichTextBox { get; set; }

        internal ObservableCollection<string> Headers => language.MyContextMenu;
        internal ObservableCollection<string> ToolTips => language.ToolTipsMyContextMenu;

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
        /// контрол над которым вызвано контекстное меню
        /// </summary>
        private object placementTarget { get; set; }


        //флаги указывающие страницу на которой вызвано контекстное меню
        private bool home { get; set; }//домашняя страница
        private bool textEditor { get; set; }//редактор текста
        private bool reader { get; set; }//страница чтения
        private bool search { get; set; }//страница результатов поиска


        internal MyContextMenuModel()
        {
            mainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "ToolTips" });
            foreach (PropertyInfo info in typeof(Drawing.Color).GetProperties())
            {
                if (info.PropertyType == typeof(Drawing.Color))
                    MyColors.Add((Drawing.Color)info.GetValue(null));
            }
        }


        internal bool CanExecute_OnCopyButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = richTextBoxViewModel.OnCopyButtonClick.CanExecute(null) && behaviorRichTextBox.SelectedImage == null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnCopyButtonClick(object obj)
        {
            try
            {
                richTextBoxViewModel.OnCopyButtonClick.Execute(null);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnCopyImageButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = behaviorRichTextBox.SelectedImage != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnCopyImageButtonClick(object obj)
        {
            try
            {
                behaviorRichTextBox.SelectedImageCopy();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnCutButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = richTextBoxViewModel.OnCutButtonClick.CanExecute(null) && behaviorRichTextBox.SelectedImage == null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnCutButtonClick(object obj)
        {
            try
            {
                richTextBoxViewModel.OnCutButtonClick.Execute(null);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnCutImageButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = behaviorRichTextBox.SelectedImage != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnCutImageButtonClick(object obj)
        {
            try
            {
                behaviorRichTextBox.SelectedImageCut();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnPasteButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = richTextBoxViewModel.OnPasteButtonClick.CanExecute(null) && behaviorRichTextBox.SelectedImage == null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnPasteButtonClick(object obj)
        {
            try
            {
                richTextBoxViewModel.OnPasteButtonClick.Execute(null);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnPasteImageButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = Clipboard.GetImage() != null && obj is string && behaviorRichTextBox.SelectedImage != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnPasteImageButtonClick(object obj)
        {
            try
            {
                behaviorRichTextBox.PasteImageFromClipboard(obj);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// открыть файл для чтения
        /// </summary>
        /// <param name="obj">путь к файлу(string)</param>
        /// <returns></returns>
        internal bool CanExecute_OpenForReading(object obj)
        {
            try
            {
                bool c = false;
                c = search && obj is System.Windows.Controls.ContextMenu;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OpenForReading(object obj)
        {
            try
            {
                if (obj is System.Windows.Controls.ContextMenu menu)
                {
                    if (menu.PlacementTarget is System.Windows.Controls.TextBlock textblock && File.Exists(textblock.Text))
                    {
                        var page = mainWindowViewModel.FrameList.Where((x) => x is Views.FlowDocumentReader).LastOrDefault();
                        if (page == null)
                        {
                            page = new Views.FlowDocumentReader() { KeepAlive = true };
                            mainWindowViewModel.FrameListAddPage.Execute(page);
                        }
                        else
                        {
                            mainWindowViewModel.CurrentPage = page;
                        }
                        ((ViewModels.FlowDocumentReaderViewModel)page.DataContext).BehaviorReady = new Action(() =>
                        {
                            var home = mainWindowViewModel.FrameList.Where((x) => x is Views.Home).LastOrDefault();
                            var menuhome = (MyControls.MenuHome)home.FindResource("menuhome");
                            var menufile = (ViewModels.HomeMenuFileViewModel)menuhome.FindResource("menufile");
                            menufile.OpenFile.Execute(textblock.Text);
                        });
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// открыть файл для редактирования
        /// </summary>
        /// <param name="obj">путь к файлу(string)</param>
        /// <returns></returns>
        internal bool CanExecute_OpenForEditing(object obj)
        {
            try
            {
                bool c = false;
                c = search && obj is System.Windows.Controls.ContextMenu;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OpenForEditing(object obj)
        {
            try
            {
                if (obj is System.Windows.Controls.ContextMenu menu)
                {
                    if (menu.PlacementTarget is System.Windows.Controls.TextBlock textblock && File.Exists(textblock.Text))
                    {
                        var page = mainWindowViewModel.FrameList.Where((x) => x is Views.Home).LastOrDefault();
                        if (page == null)
                        {
                            page = new Views.Home() { KeepAlive = true };
                            mainWindowViewModel.FrameListAddPage.Execute(page);
                        }
                        else
                        {
                            mainWindowViewModel.CurrentPage = page;
                        }
                        ((ViewModels.HomeViewModel)page.DataContext).BehaviorReady = new Action(() =>
                        {
                            var home = mainWindowViewModel.FrameList.Where((x) => x is Views.Home).LastOrDefault();
                            var menuhome = (MyControls.MenuHome)home.FindResource("menuhome");
                            var menufile = (ViewModels.HomeMenuFileViewModel)menuhome.FindResource("menufile");
                            menufile.OpenFile.Execute(textblock.Text);
                        });
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnDeleteButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor || search)
                {
                    if (home || textEditor)
                        c = richTextBoxViewModel.OnDeleteButtonClick.CanExecute(null) && behaviorRichTextBox.SelectedImage == null;
                    else if (search && placementTarget is TextBlock textBlock)
                        c = File.Exists(textBlock.Text);
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnDeleteButtonClick(object obj)
        {
            try
            {
                if (home || textEditor)
                    richTextBoxViewModel.OnDeleteButtonClick.Execute(null);
                else if (search && placementTarget is TextBlock textBlock)
                {
                    if (mainWindowViewModel.CurrentPage is Views.SearchForFiles page)
                    {
                        var viewmodel = (SearchForFilesViewModel)page.DataContext;
                        if (viewmodel.DeleteFile.CanExecute(textBlock.Text))
                        {
                            viewmodel.DeleteFile.Execute(textBlock.Text);
                        }
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnDeleteImageButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home)
                    c = behaviorRichTextBox.SelectedImage != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnDeleteImageButtonClick(object obj)
        {
            try
            {
                behaviorRichTextBox.SelectedImageDelete();
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnChangeImageButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home)
                    c = behaviorRichTextBox.SelectedImage != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnChangeImageButtonClick(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnInkButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = richTextBoxViewModel.OnInkButtonClick.CanExecute(obj) && behaviorRichTextBox.SelectedImage == null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnInkButtonClick(object obj)
        {
            try
            {
                richTextBoxViewModel.OnInkButtonClick.Execute(obj);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnBackgroundButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = richTextBoxViewModel.OnBackgroundButtonClick.CanExecute(obj) && behaviorRichTextBox.SelectedImage == null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnBackgroundButtonClick(object obj)
        {
            try
            {
                richTextBoxViewModel.OnBackgroundButtonClick.Execute(obj);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_OnPaperButtonClick(object obj)
        {
            try
            {
                bool c = false;
                if (home || textEditor)
                    c = richTextBoxViewModel.OnPaperButtonClick.CanExecute(obj);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_OnPaperButtonClick(object obj)
        {
            try
            {
                richTextBoxViewModel.OnPaperButtonClick.Execute(obj);
            }
            catch (Exception e) { ErrorWindow(e); }
        }

        internal bool CanExecute_MyContextMenuOpened(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_MyContextMenuOpened(object obj)
        {
            try
            {
                if (obj is ContextMenu menu)
                {
                    home = false;
                    textEditor = false;
                    reader = false;
                    search = false;
                    placementTarget = menu.PlacementTarget;
                    if (placementTarget is RichTextBox textbox)
                    {
                        var list = new Window[Application.Current.Windows.Count];
                        Application.Current.Windows.CopyTo(list, 0);
                        textEditor = list.Where((x) => x is Views.TextEditor).FirstOrDefault() != null;
                        home = !textEditor;
                        richTextBoxViewModel = (RichTextBoxViewModel)textbox.DataContext;
                        behaviorRichTextBox = richTextBoxViewModel.BehaviorRichTextBox;
                    }
                    else
                    {
                        reader = mainWindowViewModel.CurrentPage is Views.FlowDocumentReader;
                        search = mainWindowViewModel.CurrentPage is Views.SearchForFiles;
                    }

                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }



        private void ErrorWindow(Exception e, [CallerMemberName] string name = "")
        {
            Thread thread = new(() => MessageBox.Show(e.Message, $"MyContextMenuModel.{name}"));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

    }
}
