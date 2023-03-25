using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Controls = System.Windows.Controls;
using System.Windows.Input;

using NotebookRCv001.Infrastructure;
using NotebookRCv001.Helpers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection;
using Drawing = System.Drawing;
using System.Reflection.Metadata;

namespace NotebookRCv001.Models
{
    internal class DocumentTreeModel : ViewModelBase
    {
        #region ________________________Window position and dimensions___________________

        internal object WindowState
        {
            get => windowState;
            set => SetProperty(ref windowState, value);
        }
        object windowState;
        internal double WindowHeight
        {
            get => windowHeight;
            set => SetProperty(ref windowHeight, value);
        }
        double windowHeight;
        internal double WindowWidth
        {
            get => windowWidth;
            set => SetProperty(ref windowWidth, value);
        }
        double windowWidth;
        internal double WindowLeft
        {
            get => windowLeft;
            set => SetProperty(ref windowLeft, value);
        }
        double windowLeft;
        internal double WindowTop
        {
            get => windowTop;
            set => SetProperty(ref windowTop, value);
        }
        double windowTop;

        #endregion

        #region _______________Private Property__________________________

        private readonly ViewModels.MainWindowViewModel mainWindowViewModel;
        private ViewModels.RichTextBoxViewModel richTextBoxViewModel { get; set; }
        private Helpers.BehaviorRichTextBox behaviorRichTextBox { get; set; }
        private Helpers.BehaviorTreeView behaviorTreeView { get; set; }
        private Languages language => mainWindowViewModel.Language;
        private FlowDocument document => behaviorRichTextBox.Document;

        #endregion

        #region ___________________Publick Property______________________________

        internal ObservableCollection<string> Headers => language.HeadersDocumentTree;
        internal ObservableCollection<string> ToolTips => language.ToolTipsDocumentTree;

        internal object FlowDocumentLastSelected
        {
            get => flowDocumentLastSelected;
            set => SetProperty(ref flowDocumentLastSelected, value);
        }
        private object flowDocumentLastSelected;



        #endregion

        #region _________________________Constructors________________________


        internal DocumentTreeModel()
        {
            mainWindowViewModel = (ViewModels.MainWindowViewModel)Application.Current.MainWindow.DataContext;
            language.PropertyChanged += (s, e) => OnPropertyChanged(new string[] { "Headers", "Tooltips" });
            var home = mainWindowViewModel.CurrentPage;
            var richTextBox = (MyControls.RichTextBox)home.FindResource("richtextbox");
            richTextBoxViewModel = (ViewModels.RichTextBoxViewModel)richTextBox.DataContext;
            richTextBoxViewModel.PropertyChanged += RichTextBoxViewModel_PropertyChanged;
            behaviorRichTextBox = richTextBoxViewModel.BehaviorRichTextBox;
            behaviorRichTextBox.PreviewMouseLeftButtonDown += BehaviorRichTextBox_PreviewMouseLeftButtonDown;
            //восстанавливаем размеры окна
            if (Properties.Settings.Default.FirstStart)
            {//первый запуск
                WindowHeight = 40;
                WindowWidth = 50;
                WindowLeft = 20;
                WindowTop = 20;
                Properties.Settings.Default.FirstStart = false;
            }
            else
            {
                WindowHeight = Properties.Settings.Default.DocumentTreeHeight;
                WindowWidth = Properties.Settings.Default.DocumentTreeWidth;
                WindowLeft = Properties.Settings.Default.DocumentTreeLeft;
                WindowTop = Properties.Settings.Default.DocumentTreeTop;
            }
            //восстанавливаем состояние окна
            WindowState = Properties.Settings.Default.WindowState;
        }



        #endregion

        #region ____________________Command Handlers________________________________________

        /// <summary>
        /// добавление нового Paragraph в документ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_AddParagraph(object obj)
        {
            try
            {
                bool c = false;
                c = !(behaviorTreeView?.SelectedItem?.Tag is Paragraph);
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_AddParagraph(object obj)
        {
            try
            {
                FlowDocumentLastSelected ??= behaviorRichTextBox.Document;
                var type = FlowDocumentLastSelected.GetType();
                var property = type.GetProperty("Blocks");
                if (property != null)
                {
                    var blocks = property.GetValue(FlowDocumentLastSelected);
                    if (blocks is System.Windows.Documents.BlockCollection collection)
                        collection.Add(new Paragraph());
                }
                else
                {
                    behaviorRichTextBox.Document.Blocks.Add(new Paragraph());
                }
                Execute_TreeViewLoaded(behaviorTreeView);
            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// добавление нового Figure в документ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_AddFigure(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_AddFigure(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// добавление нового Inline UI Container в документ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_AddIUIContainer(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_AddIUIContainer(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }
        /// <summary>
        /// добавление нового Block UI Container в документ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_AddBUIContainer(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_AddBUIContainer(object obj)
        {
            try
            {

            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// добавление нового File в документ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_AddFile(object obj)
        {
            try
            {
                bool c = false;
                if (behaviorTreeView?.SelectedItem != null)
                {
                    var type = behaviorTreeView.SelectedItem.Tag.GetType();
                    var inlines = type.GetProperty("Inlines");
                    c = inlines != null;
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_AddFile(object obj)
        {
            try
            {
                var type = behaviorTreeView.SelectedItem.Tag.GetType();
                var inlines = type.GetProperty("Inlines");
                if (inlines?.GetValue(behaviorTreeView.SelectedItem.Tag) is InlineCollection inlines1)
                {
                    //behaviorRichTextBox.TargetToInsert = inlines1;
                    behaviorRichTextBox.TargetToInsert = behaviorTreeView.SelectedItem.Tag;
                }
                var select = new Views.SelectAndPasteWindow();
                select.Owner = Application.Current.MainWindow;
                select.Show();

            }
            catch (Exception e) { ErrorWindow(e); }
        }

        /// <summary>
        /// удаление
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_Delete(object obj)
        {
            try
            {
                bool c = false;
                if (behaviorTreeView?.SelectedItem != null)
                {//условие удаления - родительский элемент (Parent) должен содержать коллекцию Inlines или Blocks
                    var type = behaviorTreeView.SelectedItem.Tag.GetType();//тип удаляемого элемента
                    var parent = type?.GetProperty("Parent");//ищем свойство Parent
                    var value = parent?.GetValue(behaviorTreeView.SelectedItem.Tag);//родительский элемент
                    var typeValue = value?.GetType();//тип родительского элемента
                    var a = typeValue?.GetProperty("Inlines");//ицем свойтсво Inlines
                    var b = typeValue?.GetProperty("Blocks");//ищем свойство Blocks
                    c = (a != null || b != null);
                }
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_Delete(object obj)
        {
            try
            {
                var type = behaviorTreeView.SelectedItem.Tag.GetType();
                var property = type.GetProperty("Parent");
                if (property != null)
                {
                    var value = property.GetValue(behaviorTreeView.SelectedItem.Tag);
                    var valueType = value.GetType();
                    var valueProperty = valueType.GetProperty("Blocks");
                    if (valueProperty != null)
                    {
                        var value1 = valueProperty.GetValue(value);
                        if (value1 is System.Windows.Documents.BlockCollection blocks)
                        {
                            blocks.Remove(behaviorTreeView.SelectedItem.Tag as Block);
                        }
                    }
                    else
                    {
                        valueProperty = valueType.GetProperty("Inlines");
                        if (valueProperty != null)
                        {
                            var value2 = valueProperty.GetValue(value);
                            if (value2 is System.Windows.Documents.InlineCollection inlines)
                            {
                                inlines.Remove(behaviorTreeView.SelectedItem.Tag as Inline);
                            }
                        }
                    }
                }
                FlowDocumentLastSelected = null;//удалеяем ссылку на удаленный элемент
                Execute_TreeViewLoaded(behaviorTreeView);//обновление TreeView
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        /// <summary>
        /// снять все выделения
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal bool CanExecute_ClearSelection(object obj)
        {
            try
            {
                bool c = false;
                c = FlowDocumentLastSelected != null;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_ClearSelection(object obj)
        {
            try
            {
                //убираем выделение из FlowDocument
                foreach (var block in behaviorRichTextBox.Document.Blocks)
                {
                    var type = block.GetType();
                    var borderthickness = type?.GetProperty("BorderThickness");
                    var borderbrush = type?.GetProperty("BorderBrush");
                    if (borderthickness != null && borderbrush != null && block.BorderBrush == (Brush)behaviorRichTextBox.Converter.Convert(Properties.Settings.Default.MyHighlightColor, typeof(Brush), null, null));
                    {
                        borderthickness.SetValue(block, new Thickness(0));
                        borderbrush.SetValue(block, null);
                    }
                }
                //убираем выделение из TreeView
                foreach(TreeViewItem item in behaviorTreeView.TreeView.Items)
                {
                    if (item.IsSelected)
                        item.IsSelected = false;
                }
                FlowDocumentLastSelected = null;
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_TreeViewLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_TreeViewLoaded(object obj)
        {
            try
            {
                Controls.TreeViewItem item = null;
                if (obj is Helpers.BehaviorTreeView behavior)
                {
                    behaviorTreeView = behavior;
                    behaviorTreeView.TreeView.Items.Clear();
                    foreach (var block in document.Blocks)
                    {
                        item = new();
                        var array = block.GetType().ToString().Split('.');
                        item.Header = array[array.Length - 1];
                        item.Tag = block;
                        item.Items.Add('*');
                        item.Selected += Item_Selected;
                        item.Expanded += Item_Expanded;
                        behavior.TreeView.Items.Add(item);
                    }
                }
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecuteWindowLoaded(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowLoaded(object obj)
        {
            try
            {
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        internal bool CanExecute_WindowClosing(object obj)
        {
            try
            {
                bool c = false;
                c = true;
                return c;
            }
            catch (Exception e) { ErrorWindow(e); return false; }
        }
        internal void Execute_WindowClosing(object obj)
        {
            try
            {
                //размеры и положение окна
                if (WindowState.ToString() == "Normal")
                {
                    Properties.Settings.Default.DocumentTreeWidth = WindowWidth;
                    Properties.Settings.Default.DocumentTreeHeight = WindowHeight;
                    Properties.Settings.Default.DocumentTreeLeft = WindowLeft;
                    Properties.Settings.Default.DocumentTreeTop = WindowTop;
                }
                Properties.Settings.Default.Save();
                behaviorRichTextBox.PreviewMouseLeftButtonDown -= BehaviorRichTextBox_PreviewMouseLeftButtonDown;
                if (FlowDocumentLastSelected != null)
                {//убираем выделение с выбранного элемента
                    var type = FlowDocumentLastSelected?.GetType();
                    var borderbrush = type?.GetProperty("BorderBrush");
                    var borderthickness = type?.GetProperty("BorderThickness");
                    borderbrush?.SetValue(FlowDocumentLastSelected, Brushes.Transparent);
                }
                behaviorRichTextBox.SetFocus();
                if (CanExecute_ClearSelection(null))
                    Execute_ClearSelection(null);
            }
            catch (Exception e) { ErrorWindow(e); }
        }


        #endregion

        #region ______________Event Handlers________________________________

        private void Item_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (e.Source is Controls.TreeViewItem item && item.Tag != null)
                {
                    Controls.TreeViewItem treeViewItem = null;
                    object content = GetChildElements(item.Tag);
                    item.Items.Clear();
                    treeViewItem = new();
                    if (content is Array array1)
                    {
                        foreach (var el in array1)
                        {
                            if (el != null)
                            {
                                var array = el.GetType().ToString().Split('.');
                                treeViewItem.Header = array[array.Length - 1];
                                treeViewItem.Tag = el;
                                treeViewItem.Items.Add('*');
                                item.Items.Remove('*');
                                item.Items.Add(treeViewItem);
                                treeViewItem = new();
                            }
                        }
                    }
                    else if (content is Run run)
                    {
                        treeViewItem.Header = run.Text;
                        item.Items.Add(treeViewItem);
                    }
                }
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        private void Item_Selected(object sender, RoutedEventArgs e)
        {
            try
            {
                if (FlowDocumentLastSelected != null)
                {//удаляем обрамление ранее выбранного элемента
                    var lastborderbrush = FlowDocumentLastSelected.GetType().GetProperty("BorderBrush");
                    lastborderbrush?.SetValue(FlowDocumentLastSelected, null);
                }
                if (e.OriginalSource is Controls.TreeViewItem item)
                {//устанавливаем обрамление вокруг нового выбранного элемента
                    var type = behaviorTreeView.SelectedItem.Tag.GetType();
                    var borderbrush = type.GetProperty("BorderBrush");
                    var borderthickness = type.GetProperty("BorderThickness");
                    var brush = (Brush)behaviorRichTextBox.Converter.Convert(Properties.Settings.Default.MyHighlightColor, typeof(Brush), null, null);
                    if (borderbrush != null && borderthickness != null)
                    {
                        borderbrush.SetValue(item.Tag, brush);
                        borderthickness.SetValue(item.Tag, new Thickness(1));
                    }
                    FlowDocumentLastSelected = item.Tag;
                }
                e.Handled = true;
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        private void BehaviorRichTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (!document.Equals(e.Source))
                {
                    var root = GetFlowDocumentRoot(e.Source);
                    var brush = (Brush)behaviorRichTextBox.Converter.Convert(Properties.Settings.Default.MyHighlightColor, typeof(Brush), null, null);
                    if (root != null)
                    {
                        foreach (TreeViewItem item in behaviorTreeView.TreeView.Items)
                        {
                            if (item.Tag == root)
                            {
                                item.Foreground = brush;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }
        private void RichTextBoxViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            try
            {
                Execute_TreeViewLoaded(behaviorTreeView);
            }
            catch (Exception ex) { ErrorWindow(ex); }
        }

        #endregion

        #region ____________________Private Methods______________________________


        /// <summary>
        /// поиск корневого родителя (в Document.Blocks) данного элемента
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private object GetFlowDocumentRoot(object element)
        {
            try
            {
                object rootElement = null;
                object parent = null;
                if (element != null)
                {
                    if (element is FrameworkElement image)
                    {
                        parent = image.Parent;
                    }
                    else if (element is FrameworkContentElement container)
                    {
                        parent = container.Parent;
                    }
                    else
                    {
                        parent = element.GetType().GetProperty("Parent");
                    }
                    if (!document.Equals(parent))
                    {
                        rootElement = GetFlowDocumentRoot(parent);
                    }
                    else
                    {
                        rootElement = rootElement == null ? element : rootElement;
                    }
                }
                return rootElement;
            }
            catch (Exception e) { ErrorWindow(e); return null; }
        }

        private object[] GetChildElements(object parent)
        {
            try
            {
                object[] childs = null;
                var type = parent.GetType();
                var block = type.GetProperty("Blocks");
                var inline = type.GetProperty("Inlines");
                var child = type.GetProperty("Child");
                var runtext = type.GetProperty("Text");
                var array = block != null ? ((BlockCollection)block.GetValue(parent)).ToArray() :
                    inline != null ? ((InlineCollection)inline.GetValue(parent)).ToArray() :
                    child != null ? new object[] { child.GetValue(parent) } :
                    runtext != null ? runtext.GetValue(parent) :
                    null;
                if (array is object[] ch)
                    childs = ch;
                return childs;
            }
            catch (Exception e) { ErrorWindow(e); return null; }
        }

        #endregion

    }
}
